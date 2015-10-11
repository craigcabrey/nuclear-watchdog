using System;
using System.ServiceModel;

namespace HeartbeatReceiver
{
    [ServiceContract(Namespace = "http://SWEN.755.Heartbeat")]
    public interface IHeartbeat
    {
        [OperationContract]
        Tuple<bool, string> Register();

        [OperationContract]
        void Unregister(string id);

        [OperationContract]
        bool Beat(string id);
    }
}
