using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Quobject.SocketIoClientDotNet.Client;
using RestSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XTS.WSApi.Enums;
using XTS.WSApi.EventMessages;
using XTS.WSApi.Lib;

namespace XTS.WSApi
{
    public class XTSSocketIo : IDisposable
    {
        private const byte NO_COMPRESSION = 0;
        private const byte COMPRESSED_DATA = 1;

        //private const string pathRoute = "twsmarketdata";
        private const string pathRoute = "apibinarymarketdata";
        private string _clientId = string.Empty;
        private string _baseURI = string.Empty; //"https://ttblaze.iifl.com/";
        private string _tokenGenerationUrl = string.Empty;
        private string _wsSocketPath = string.Empty;
        private string _secretKey = string.Empty;
        private string _appKey = string.Empty;
        private ConcurrentQueue<TouchlineEventMessage> _touchlineEventQueue = new ConcurrentQueue<TouchlineEventMessage>();
        private const int EnqueueEventTimeout = 1000;
        private bool _stopProcessing = false;
        private SemaphoreSlim _internalEnqueueEvent = new SemaphoreSlim(0, int.MaxValue);

        private const int _packetHeaderSize = 8;
        private string _currentValidToken = string.Empty;
        private static JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy(),
            },
            Formatting = Formatting.Indented
        };

        private Quobject.SocketIoClientDotNet.Client.Socket _wsSocket = null;
        private RestClient _restClientForTokenSubscription = null;
        private List<DataMessage> _tempDataMessageList = new List<DataMessage>();

        private IDataHandler _eventHandler;
        private ConcurrentDictionary<string, Action<object>> _socketOnEventCallbackMap = new ConcurrentDictionary<string, Action<object>>();
        private bool disposedValue;

        public XTSSocketIo(IDataHandler handler)
        {
            _eventHandler = handler;
            Task.Factory.StartNew(() =>
            {
                StartTouchlineEventDispatchThread();
            });
        }

        public DateTime SocketInitializedAt { get; private set; } = DateTime.MinValue;
        public DateTime LastSuccessfullyJoinedAt { get; private set; } = DateTime.MinValue;
        public string LastSocketEvent { get; private set; } = string.Empty;
        public DateTime LastTouchlinePacketReceivedAt { get; private set; } = DateTime.MinValue;
        public DateTime LastDisconnectedAt { get; private set; } = DateTime.MinValue;
        public TimeSpan ConnectedSince
        {
            get
            {
                if (LastSuccessfullyJoinedAt == DateTime.MinValue)
                    return TimeSpan.Zero;
                return DateTime.Now.Subtract(LastSuccessfullyJoinedAt);
            }
        }
        public int TotalDisconnectCountSinceFirstConnect { get; private set; } = 0;

        public bool Connect(string baseUri, string clientId, string secretKey, string appKey)
        {
            _baseURI = baseUri;
            if (_baseURI.EndsWith("/"))
                _tokenGenerationUrl = $"{_baseURI}{pathRoute}/auth/login";
            else
                _tokenGenerationUrl = $"{_baseURI}/{pathRoute}/auth/login";

            _wsSocketPath = $"/{pathRoute}/socket.io";

            _clientId = clientId;
            _secretKey = secretKey;
            _appKey = appKey;

            _restClientForTokenSubscription = new RestClient($"{_baseURI}{pathRoute}/instruments/subscription");

            return ConnectInternal();
        }

        public async Task<bool> SubscibeToken(Symbol[] symbols)
        {
            var request = new RestRequest("", Method.Post);
            request.AddHeader("Authorization", _currentValidToken);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("xtsMessageCode", ((int)SubscribtionCode.TouchlineEvent).ToString());
            request.AddParameter("instruments", JsonConvert.SerializeObject(symbols));

            RestResponse response = await _restClientForTokenSubscription.ExecuteAsync(request);
            if (response.IsSuccessful)
            {
                _eventHandler.TraceInfo(response.Content);

                JObject jObjects = JObject.Parse(response.Content);
                return jObjects["type"].ToString().Equals("success", StringComparison.InvariantCultureIgnoreCase);
            }
            else
            {
                return false;
            }
        }

        #region Private Methods

        private bool ConnectInternal()
        {
            _currentValidToken = string.Empty;

            _eventHandler.TraceInfo($"Connecting With User {_clientId}");
            _eventHandler.TraceInfo($"Fetching Access Token From URL [{_tokenGenerationUrl}]");
            if (_eventHandler.PrintCredentialsToLog)
            {
                _eventHandler.TraceInfo($"SecretKey:[{_secretKey}] AppKey:[{_appKey}]");
            }
            GetAccessTokenData();
            if (string.IsNullOrEmpty(_currentValidToken))
            {
                _eventHandler.TraceError($"Process failed While fetcing token.");
                return false;
            }
            else
            {
                _eventHandler.TraceInfo($"Token Fetched.");
                if (_eventHandler.PrintCredentialsToLog)
                {
                    _eventHandler.TraceInfo($"Current valid token : [{_currentValidToken}]");
                }

                _eventHandler.TraceInfo($"Connecting Socket.io with token. URL:[{_baseURI}] Path:[{_wsSocketPath}]");

                return ConnectWS();
            }
        }

        private void GetAccessTokenData()
        {
            _currentValidToken = string.Empty;
            Dictionary<string, string> parameterList = new Dictionary<string, string>();
            parameterList.Add("secretKey", _secretKey);
            parameterList.Add("appKey", _appKey);

            try
            {
                using (RestClient restClient = new RestClient(_tokenGenerationUrl))
                {
                    var request = new RestRequest("", Method.Post);
                    request.AddParameter("application/json", SerializeJson(parameterList), ParameterType.RequestBody);
                    RestResponse response = restClient.Execute(request);
                    if (response.IsSuccessful)
                    {
                        JObject token1 = JObject.Parse(response.Content);
                        _currentValidToken = token1["result"]["token"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                _eventHandler.TraceError(ex.Message);
                _eventHandler.TraceError(ex.StackTrace);
            }
        }

        private bool ConnectWS()
        {
            IO.Options options = new IO.Options()
            {
                IgnoreServerCertificateValidation = true,
                Path = _wsSocketPath,
                Query = new Dictionary<string, string>()
                    {
                        { "token", _currentValidToken},
                        { "userID", _clientId },
                        { "source", "WebAPI"},
                        { "publishFormat", PublishFormat.Binary.ToString() },
                        { "broadcastMode", BroadcastMode.Full.ToString() }
                    }
            };

            SocketInitializedAt = DateTime.Now;

            if (_wsSocket == null)
                _wsSocket = IO.Socket(_baseURI, options);
            else
            {
                _wsSocket = null;
                System.Threading.Thread.Sleep(1000);
                _wsSocket = IO.Socket(_baseURI, options);
            }
            options.ExtraHeaders.Clear();
            RegisterSocketEvents();

            return true;
        }

        private void RegisterSocketEvents()
        {
            string socketEventLoggerPrefix = "Web Socket Event ";
            string socketError = "socketError";
            RegisterSocketOnCallback("joined", (socketInfo) =>
            {
                _eventHandler.TraceInfo($"{socketEventLoggerPrefix} Joined. [Details : { socketInfo }]");

                if (_wsSocket != null)
                    _wsSocket.Emit("join", _clientId, _currentValidToken);

                _touchlineEventQueue = new ConcurrentQueue<TouchlineEventMessage>();
                LastSocketEvent = "joined";
                LastSuccessfullyJoinedAt = DateTime.Now;
            });
            RegisterSocketOnCallback(Socket.EVENT_CONNECT, (socketInfo) => { LastSocketEvent = Socket.EVENT_CONNECT; _eventHandler.TraceInfo($"{socketEventLoggerPrefix}{Socket.EVENT_CONNECT} [Details : {socketInfo }]"); });
            RegisterSocketOnCallback(Socket.EVENT_CONNECT_ERROR, (socketInfo) => { LastSocketEvent = Socket.EVENT_CONNECT_ERROR; _eventHandler.TraceError($"{socketEventLoggerPrefix + Socket.EVENT_CONNECT_ERROR } [Details : { socketInfo }]"); });
            RegisterSocketOnCallback(Socket.EVENT_CONNECT_TIMEOUT, (socketInfo) => { LastSocketEvent = Socket.EVENT_CONNECT_TIMEOUT; _eventHandler.TraceWarning($"{socketEventLoggerPrefix + Socket.EVENT_CONNECT_TIMEOUT } [Details : { socketInfo }]"); });
            RegisterSocketOnCallback(Socket.EVENT_DISCONNECT, (socketInfo) =>
            {
                LastSocketEvent = Socket.EVENT_DISCONNECT;
                TotalDisconnectCountSinceFirstConnect++;
                _eventHandler.TraceError($"{socketEventLoggerPrefix + Socket.EVENT_DISCONNECT } [Details : { socketInfo }]");
            });
            RegisterSocketOnCallback(Socket.EVENT_ERROR, (socketInfo) => { LastSocketEvent = Socket.EVENT_ERROR; _eventHandler.TraceError($"{socketEventLoggerPrefix + Socket.EVENT_ERROR } [Details : { socketInfo }]"); });
            RegisterSocketOnCallback(Socket.EVENT_RECONNECT, (socketInfo) => { LastSocketEvent = Socket.EVENT_RECONNECT; _eventHandler.TraceInfo($"{socketEventLoggerPrefix + Socket.EVENT_RECONNECT } [Details : { socketInfo }]"); });
            RegisterSocketOnCallback(Socket.EVENT_RECONNECT_ATTEMPT, (socketInfo) => { LastSocketEvent = Socket.EVENT_RECONNECT_ATTEMPT; _eventHandler.TraceInfo($"{socketEventLoggerPrefix + Socket.EVENT_RECONNECT_ATTEMPT } [Details : { socketInfo }]"); });
            RegisterSocketOnCallback(Socket.EVENT_RECONNECT_ERROR, (socketInfo) => { LastSocketEvent = Socket.EVENT_RECONNECT_ERROR; _eventHandler.TraceError($"{socketEventLoggerPrefix + Socket.EVENT_RECONNECT_ERROR } [Details : { socketInfo }]"); });
            RegisterSocketOnCallback(Socket.EVENT_RECONNECT_FAILED, (socketInfo) => { LastSocketEvent = Socket.EVENT_RECONNECT_FAILED; _eventHandler.TraceError($"{socketEventLoggerPrefix + Socket.EVENT_RECONNECT_FAILED } [Details : { socketInfo }]"); });
            RegisterSocketOnCallback(Socket.EVENT_RECONNECTING, (socketInfo) => { LastSocketEvent = Socket.EVENT_RECONNECTING; _eventHandler.TraceInfo($"{socketEventLoggerPrefix + Socket.EVENT_CONNECT } [Details : { socketInfo }]"); });
            RegisterSocketOnCallback(socketError, (socketInfo) => { LastSocketEvent = socketError; _eventHandler.TraceInfo($"{socketEventLoggerPrefix }Socket Error [Details with Token: { socketInfo } { _currentValidToken }]"); });

            RegisterSocketOnCallback("xts-binary-packet", OnNewBinaryPacketMessage);
        }

        private void OnNewBinaryPacketMessage(object message)
        {
            byte[] byteArray = (byte[])message;
            //int sourceIndex = 1;
            //int byteIndexRead = 0;
            //byte compressedOrUncompress = byteArray[0];
            _tempDataMessageList.Clear();
            _tempDataMessageList = GetDataMessages(byteArray).ToList();
            //while (byteIndexRead < byteArray.Length)
            //{
            //    if (compressedOrUncompress == NO_COMPRESSION)
            //    {
            //        //here expected that all businees messages are received without packet split over network
            //        GetDataMessage(byteArray.Skip(1).ToArray(), byteArray.Length - 1, _tempDataMessageList);                    
            //        break;
            //    }
            //    else
            //    {
            //        if (byteArray.Length < (sourceIndex + _packetHeaderSize))
            //        {
            //            _eventHandler.TraceWarning($"Received packet length [{byteArray.Length}] is less than expected length [{(sourceIndex + _packetHeaderSize)}].");
            //            break;
            //        }

            //        int CompressLength = BitConverter.ToInt32(byteArray, sourceIndex + byteIndexRead); //First 4 bytes                        
            //        int UnCompressLength = BitConverter.ToInt32(byteArray, sourceIndex + byteIndexRead + 4); //Next 4 bytes
            //        if (byteArray.Length < (_packetHeaderSize + CompressLength))
            //        {
            //            _eventHandler.TraceWarning($"Received market data packet length [{byteArray.Length}] is less than expected length [{(_packetHeaderSize + CompressLength)}]. Market Data packet Length: [{CompressLength}].");
            //            break;
            //        }

            //        byte[] compressedByteArray = byteArray.Skip(sourceIndex + _packetHeaderSize + byteIndexRead).Take(CompressLength).ToArray();
            //        byte[] decompressedByteArray = DecompressZLib(compressedByteArray);
            //        GetDataMessage(decompressedByteArray, UnCompressLength, _tempDataMessageList);                    
            //        byteIndexRead += sourceIndex + _packetHeaderSize + CompressLength;
            //    }
            //    sourceIndex = 0;
            //    if (byteArray == null || byteArray.Length == 0)
            //        break;
            //}

            if (_tempDataMessageList.Count > 0)
            {
                foreach (DataMessage iterator in _tempDataMessageList)
                {
                    if (iterator.MessageCode == (ushort)SubscribtionCode.TouchlineEvent)
                    {
                        _touchlineEventQueue.Enqueue((TouchlineEventMessage)iterator);
                        LastTouchlinePacketReceivedAt = DateTime.Now;
                        _internalEnqueueEvent.Release();
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void RegisterSocketOnCallback(string subscriptionKey, Action<object> callback)
        {
            MethodInfo callbackMethodInfo = callback.GetMethodInfo();
            string newCallbackInfo = $"{callbackMethodInfo.DeclaringType.Namespace }.{ callbackMethodInfo.Name}";

            if (_socketOnEventCallbackMap.ContainsKey(subscriptionKey))
            {
                Action<object> oldCallback;
                _socketOnEventCallbackMap.TryRemove(subscriptionKey, out oldCallback);

                MethodInfo oldCallbackMethodInfo = oldCallback.GetMethodInfo();
                string oldCallbackInfo = $"{oldCallbackMethodInfo.DeclaringType.Namespace }.{ oldCallbackMethodInfo.Name}";
                _eventHandler.TraceWarning($"Supplied SubscriptionKey [{ subscriptionKey }] is already registered with previous callback [{oldCallbackInfo }]. Un-Registering with the same.");
                _wsSocket.Off(subscriptionKey);
            }

            _wsSocket.On(subscriptionKey, callback);
            _socketOnEventCallbackMap.TryAdd(subscriptionKey, callback);
            _eventHandler.TraceWarning($"SocketIO Subscribing with [{ subscriptionKey }] On callback [{ newCallbackInfo }].");
        }

        private string SerializeJson(object objectToSerialize)
        {
            return JsonConvert.SerializeObject(objectToSerialize, _jsonSerializerSettings);
        }

        //Dhanraj Scanner New Logic
        public static MarketDataEventMessage GetMarketDataEventMessageSinglePacket(byte[] dataByteArray, out string errorString)
        {
            try
            {
                if (dataByteArray.Length > 0)
                {
                    using (MemoryStream baseStream = new MemoryStream())
                    using (BinaryStreamReader baseStreamReader = new BinaryStreamReader(new BinaryReader(baseStream)))
                    {
                        baseStream.Write(dataByteArray, 0, dataByteArray.Length);
                        baseStream.Position = 0;
                        byte isCompress = baseStreamReader.ReadByte();//IsCompress
                        ushort xtsMessageCode = baseStreamReader.ReadUInt16();//Message Code
                        _ = baseStreamReader.ReadInt16();//ExchangeSgment
                        _ = baseStreamReader.ReadInt32();//ExchangeInstrumentID
                        _ = baseStreamReader.ReadInt16();//BookType
                        _ = baseStreamReader.ReadInt16();//MarketType
                        ushort packetLength = baseStreamReader.ReadUInt16();//Single Packet Length

                        if (isCompress == 1)
                        {
                            ushort compressedPacketSize = baseStreamReader.ReadUInt16();//Compress Packet Length

                            using (MemoryStream compressStream = new MemoryStream())
                            {
                                compressStream.Write(baseStream.GetBuffer(), (int)baseStream.Position, compressedPacketSize);
                                compressStream.Position = 0;
                                baseStream.Position += compressedPacketSize;
                                using (MemoryStream decompressedDataStream = new MemoryStream())
                                {
                                    using (DeflateStream deflateStream = new DeflateStream(compressStream, CompressionMode.Decompress, true)) //  Be careful ：  The first parameter here is also to fill in the compressed data , But this time it's data as input                                 
                                        deflateStream.CopyTo(decompressedDataStream);
                                    decompressedDataStream.Position = 0;
                                    using (BinaryStreamReader compressStreamReader = new BinaryStreamReader(new BinaryReader(decompressedDataStream)))
                                        return GetParsedMarketDataEventMessage(xtsMessageCode, compressStreamReader, out errorString);
                                }
                            }
                        }
                        else
                        {
                            return GetParsedMarketDataEventMessage(xtsMessageCode, baseStreamReader, out errorString);
                        }
                    }
                }
                else
                {
                    errorString = $"Byte data length is zero.";
                }
            }
            catch (Exception exception)
            {
                errorString = $"Error Occured While Processing GetMarketDataEventMessageSinglePacket. Error Message : {exception.Message}.";
                //throw;
            }
            return null;
        }

        private static MarketDataEventMessage GetParsedMarketDataEventMessage(ushort messageCode, BinaryStreamReader streamReader, out string errorString)
        {
            switch (messageCode)
            {
                case (ushort)SubscribtionCode.TouchlineEvent:
                    return GetParsedMarketDataEventMessage<TouchlineEventMessage>(streamReader, out errorString);
                //Type messageType = typeof(TouchlineEventMessage);
                //return GetParsedMarketDataEventMessage(messageType, streamReader, out errorString);

                //case (ushort)SubscribtionCode.MarketDepthEvent:
                //    return GetParsedMarketDataEventMessage(streamReader, out errorString);

                //case (ushort)SubscribtionCode.IndexDataEvent:
                //    return GetParsedMarketDataEventMessage<MarketIndexDataEventMessage>(streamReader, out errorString);

                default:
                    errorString = $"Implementation For MessageCode [{messageCode}] Not Found.";
                    return null;
            }
        }
        private static T GetParsedMarketDataEventMessage<T>(BinaryStreamReader streamReader, out string errorString) where T : DataMessage
        {
            T dataMessage = (T)FormatterServices.GetUninitializedObject(typeof(T));
            if (dataMessage.Deserialize(streamReader, out errorString))
                return dataMessage;
            return null;
        }
        //private static DataMessage GetParsedMarketDataEventMessage(Type messageType, BinaryStreamReader streamReader, out string errorString)
        //{
        //    //T dataMessage = ObjectExtensions.GetUninitializedObject<T>();
        //    DataMessage dataMessage = (DataMessage)FormatterServices.GetUninitializedObject(messageType);
        //    if (dataMessage.Deserialize(streamReader, out errorString))
        //        return dataMessage;
        //    return null;
        //}
        private DataMessage[] GetDataMessages(byte[] byteArray)
        {
            //HashSet<DataMessage> dataMessages = new HashSet<DataMessage>();
            List<DataMessage> dataMessagesMap = new List<DataMessage>();
            try
            {
                if (byteArray.Length > 0)
                {
                    using (MemoryStream originalMessageStream = new MemoryStream(byteArray))
                    using (BinaryStreamReader originalMessageStreamReader = new BinaryStreamReader(new BinaryReader(originalMessageStream)))
                    using (BinaryReader originalMessageBinaryReader = new BinaryReader(originalMessageStream))
                    {
                        ushort compressedPacketSize = 0;

                        ushort packetSize = 0;
                        byte isCompress;
                        ushort messageCode = 0;

                        originalMessageStream.SetLength(0);
                        originalMessageStream.Write(byteArray, 0, byteArray.Length);
                        originalMessageStream.Position = 0;
                        long packetSizePosition = 0;
                        int singlePacketSize = 0;

                        while (originalMessageStream.Length > originalMessageStream.Position)
                        {
                            packetSize = 0;
                            compressedPacketSize = 0;

                            packetSizePosition = originalMessageStream.Position;
                            isCompress = originalMessageStreamReader.ReadByte();//IsCompress = 1
                            messageCode = originalMessageStreamReader.ReadUInt16();//MessageCode = 2
                            _ = originalMessageStreamReader.ReadInt16();//ExchangeSegment = 2
                            _ = originalMessageStreamReader.ReadInt32();//ExchangeInstrumentId = 4
                            _ = originalMessageStreamReader.ReadInt16();//BookType = 2
                            _ = originalMessageStreamReader.ReadInt16();//MarketType = 2
                            packetSize = originalMessageStreamReader.ReadUInt16(); //Normal Packet Size = 2
                            if (isCompress == 1)
                            {
                                compressedPacketSize = originalMessageStreamReader.ReadUInt16(); //Compress Packet Size = 2
                                singlePacketSize = compressedPacketSize + 17;
                            }
                            else
                            {
                                singlePacketSize = packetSize + 15;
                            }
                            originalMessageStream.Position = packetSizePosition;
                            byte[] newbyteArray = originalMessageBinaryReader.ReadBytes((int)singlePacketSize);
                            DataMessage dataMessage = GetMarketDataEventMessageSinglePacket(newbyteArray, out string errorMessage);
                            if (dataMessage != null)
                            {
                                dataMessagesMap.Add(dataMessage);
                            }
                            else
                            {
                                _eventHandler.TraceInfo($"Scanner --> Unhandled MessageCode:[{messageCode}] Data:[NULL]  Error:[{errorMessage}].");
                            }
                        }
                    }
                }
                else
                {
                    _eventHandler.TraceInfo($"Scanner -->Invalid data length recevied on Web Socket. Data length: [{byteArray.Length}].");
                }
            }
            catch (Exception oEx)
            {
                _eventHandler.TraceError($"Error occured while parsing datamessage received from Web Socket. Error:{oEx.Message}");
                _eventHandler.TraceError(oEx.StackTrace);
            }
            return dataMessagesMap.ToArray();
        }

        private void GetDataMessage(byte[] byteArray, int byteLength, List<DataMessage> dataMessages)
        {
            try
            {
                if (byteArray.Length < byteLength)
                {
                    _eventHandler.TraceError($"Invalid uncompressed length recevied on Web Socket. byteArray.Length{byteArray.Length} exepected:{byteLength}");
                }
                else
                {
                    using (MemoryStream ms = new MemoryStream(byteArray))
                    {
                        using (BinaryReader binaryReader = new BinaryReader(ms))
                        {
                            using (BinaryStreamReader binaryStreamReader = new BinaryStreamReader(binaryReader))
                            {
                                do
                                {
                                    ushort messageCode = binaryStreamReader.ReadUInt16();
                                    Type messageType = null;

                                    if (messageCode == (int)SubscribtionCode.TouchlineEvent)
                                        messageType = typeof(TouchlineEventMessage);

                                    if (messageType == null)
                                        break;

                                    ms.Position = ms.Position - 2;

                                    DataMessage dataMessage = (DataMessage)FormatterServices.GetUninitializedObject(messageType);

                                    bool success = dataMessage.Deserialize(binaryStreamReader, out string errorString);
                                    if (success)
                                    {
                                        dataMessages.Add(dataMessage);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                } while (ms.Position < byteArray.Length);
                            }
                        }
                    }
                }
            }
            catch (Exception oEx)
            {
                _eventHandler.TraceError($"Error occured while parsing datamessage received from Web Socket. Error:{oEx.Message}");
                _eventHandler.TraceError(oEx.StackTrace);
            }
        }

        private byte[] DecompressZLib(byte[] dataToDeCompress)
        {
            MemoryStream compressed = new MemoryStream(dataToDeCompress);
            MemoryStream decompressed = new MemoryStream();
            DeflateStream deflateStream;
            deflateStream = new DeflateStream(compressed, CompressionMode.Decompress); //  Be careful ：  The first parameter here is also to fill in the compressed data , But this time it's data as input                                 
            deflateStream.CopyTo(decompressed);
            return decompressed.ToArray();
        }

        private async void StartTouchlineEventDispatchThread()
        {
            _eventHandler.TraceInfo($"Touchline Event Dispatched Thread Initiated.");
            try
            {
                while (!_stopProcessing)
                {
                    // Time out added for wait one so that the queue doesn't keep on waiting for enqueue event if there are not elements to process 
                    // but disposed has been signalled.
                    if (await _internalEnqueueEvent.WaitAsync(EnqueueEventTimeout))
                    {
                        if (_touchlineEventQueue.IsEmpty || _touchlineEventQueue.Count == 0)
                            continue;

                        if (_touchlineEventQueue.TryDequeue(out TouchlineEventMessage dataItem))
                        {
                            try
                            {
                                _eventHandler.OnNewTouchlineMessage(dataItem, _touchlineEventQueue.Count);
                            }
                            catch (Exception oEx)
                            {
                                _eventHandler.TraceError("Exception occurred while handling Message Processor callback.");
                                _eventHandler.TraceError(oEx.Message);
                                _eventHandler.TraceError(oEx.StackTrace);
                            }
                        }
                    }
                }
                while (!_touchlineEventQueue.IsEmpty)
                {
                    _touchlineEventQueue = new ConcurrentQueue<TouchlineEventMessage>();
                }
                _eventHandler.TraceInfo($"Touchline Event Dispatched Thread Stopped (Existed From While Loop).");
            }
            catch (Exception oEx)
            {
                _eventHandler.TraceError(oEx.Message);
                _eventHandler.TraceError(oEx.StackTrace);
            }
            _eventHandler.TraceWarning($"Touchline Event Dispatched Thread Stopped.");
        }



        #endregion

        #region IDisposable Members

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _stopProcessing = true;
                    _restClientForTokenSubscription.Dispose();
                    _wsSocket.Disconnect();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~XTSSocketIo()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
