using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Reactor
{
    [ServiceContract(Namespace = "http://SWEN.755.Reactor")]
    interface IReactor
    {
        [OperationContract]
        int GetTemperature();
    }
}
