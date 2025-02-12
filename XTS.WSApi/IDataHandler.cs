using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XTS.WSApi.EventMessages;

namespace XTS.WSApi
{
    public interface IDataHandler
    {
        bool PrintCredentialsToLog { get; }
        void TraceInfo(string message);
        void TraceWarning(string message);
        void TraceError(string message);

        void OnNewTouchlineMessage(TouchlineEventMessage message, int pendingBacklog);
    }
}
