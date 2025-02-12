using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XTS.WSApi;
using XTS.WSApi.Enums;
using XTS.WSApi.EventMessages;

namespace XTS.WebSocket.MarketFeedTest
{
    public partial class Form1 : Form, IDataHandler
    {
        private XTSSocketIo _xtScoket = null;
        private StreamWriter _swLog = null;
        private string _messageToWrite = string.Empty;
        private bool _printTouchlineToLog = false;
        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            txtBaseUrl.Text = "https://ttblaze.iifl.com/";// http://192.168.52.52:3000/";
            txtClientId.Text = "SHOBHIRE79";
            txtSecretKey.Text = "Deej832$Nc";//"Dlxs330#n2";
            txtAppKey.Text = "15cb10473a729b424cf932";//"8252b796ad556d674c3723";

            _printTouchlineToLog = chkPrintTouchlineMode.Checked;

            _swLog = CreateFile(new FileInfo($"{DateTime.Now.ToString("yyyMMddhhmmssfff")}_Data.log"));

            if (_xtScoket == null)
                _xtScoket = new XTSSocketIo(this);

            timerUpdate.Start();
        }

        private void OnConnectButtonClicked(object sender, EventArgs e)
        {
            _xtScoket.Connect(txtBaseUrl.Text.Trim(), txtClientId.Text.Trim(),
                txtSecretKey.Text.Trim(), txtAppKey.Text.Trim());
        }

        #region IDataHandler Members

        bool IDataHandler.PrintCredentialsToLog => true;

        void IDataHandler.OnNewTouchlineMessage(TouchlineEventMessage message, int pendingBacklog)
        {
            if (_printTouchlineToLog)
                ((IDataHandler)this).TraceInfo($"SOCKET.IO     EVENT[Touchline], Buffered[{pendingBacklog}] => {message.ExchangeInstrumentID.ToString()+"," + message.Touchline.LastTradedPrice.ToString()}");
        }

        void IDataHandler.TraceError(string message)
        {
            _messageToWrite = $"{DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss")}     Error     {message}";
            _swLog.WriteLine(_messageToWrite);
        }

        void IDataHandler.TraceInfo(string message)
        {
            _messageToWrite = $"{DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss")}     Info      {message}";
            _swLog.WriteLine(_messageToWrite);
        }

        void IDataHandler.TraceWarning(string message)
        {
            _messageToWrite = $"{DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss")}     Warning   {message}";
            _swLog.WriteLine(_messageToWrite);
        }

        #endregion

        private StreamWriter CreateFile(FileInfo fileInfo)
        {
            FileStream fs = null;
            if (fileInfo.Exists)
                fs = new FileStream(fileInfo.FullName, FileMode.Append, FileAccess.ReadWrite);
            else
            {
                if (!fileInfo.Directory.Exists)
                    Directory.CreateDirectory(fileInfo.Directory.FullName);
                fs = new FileStream(fileInfo.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            }

            StreamWriter sw = new StreamWriter(fs);
            sw.AutoFlush = true;
            return sw;
        }

        private void OnTimerUpdate(object sender, EventArgs e)
        {
            if (_xtScoket != null)
            {
                lblInitializedAt.Text = _xtScoket.SocketInitializedAt.ToString("hh:mm:ss");
                lblJoinedAt.Text = _xtScoket.LastSuccessfullyJoinedAt.ToString("hh:mm:ss");
                lblLastEvent.Text = _xtScoket.LastSocketEvent;
                lblLatTouchlineAt.Text = _xtScoket.LastTouchlinePacketReceivedAt.ToString("hh:mm:ss");
                lblLastDiscibbectedAt.Text = _xtScoket.LastDisconnectedAt.ToString("hh:mm:ss");
                lblConnectedSince.Text = $"{_xtScoket.ConnectedSince.Hours}:{_xtScoket.ConnectedSince.Minutes}:{_xtScoket.ConnectedSince.Seconds}";
                lblDisconnectedCount.Text = _xtScoket.TotalDisconnectCountSinceFirstConnect.ToString();
            }
        }

        private void OnSubscribeNSECMTokensClicked(object sender, EventArgs e)
        {
            SubscribeTokens(Segment.NSECM, txtTokensToSubscribeNSECM.Text.Trim());
        }

        private void OnSubscribeNSEFOTokensClicked(object sender, EventArgs e)
        {
            SubscribeTokens(Segment.NSEFO, txtTokensToSubscribeNSEFO.Text.Trim());
        }

        private async void SubscribeTokens(Segment segment, string tokens)
        {
            if (string.IsNullOrEmpty(tokens))
                return;

            string[] stringTokenArray = tokens.Split(new char[] { ',' });
            if (stringTokenArray.Length <= 0)
                return;

            Symbol[] symbolList = stringTokenArray
                .Where(iterator =>
                {
                    if (int.TryParse(iterator, out int token))
                        return true;
                    return false;
                })
                .Select(iterator =>
                {
                    return new Symbol(segment, long.Parse(iterator));
                })
                .ToArray();

            if (symbolList.Length <= 0)
                return;

            if (_xtScoket != null)
            {
                bool result = await _xtScoket.SubscibeToken(symbolList);
                if (result)
                    MessageBox.Show("Instrument(s) Subscribed successfully.");
                else
                    MessageBox.Show("Instrument(s) Not Subscribed.");
            }
        }

        private void OnPrintTouchlineModeChanged(object sender, EventArgs e)
        {
            _printTouchlineToLog = chkPrintTouchlineMode.Checked;
        }
    }
}
