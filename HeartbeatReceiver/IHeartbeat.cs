using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace HeartbeatReceiver
{
    [ServiceContract(Namespace = "http://SWEN.755.Heartbeat")]
    public interface IHeartbeat
    {
        [OperationContract]
        string Register();
        [OperationContract]
        void Unregister(string id);
        [OperationContract]
        void Beat(string id);
    }
}
