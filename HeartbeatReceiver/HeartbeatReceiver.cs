using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HeartbeatReceiver
{
    class HeartbeatReceiver : IHeartbeat
    {
        public void Register()
        {
            Console.WriteLine("Registered!");
        }

        public void Unregister()
        {
            Console.WriteLine("Unregistered!");
        }

        public void Beat()
        {
            Console.WriteLine("Received heartbeat!");
        }

        public void Monitor()
        {
            while (true)
            {
                Thread.Sleep(100);
            }
        }
    }
}
