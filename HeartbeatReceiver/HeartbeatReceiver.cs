using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HeartbeatReceiver
{
    class HeartbeatReceiver : IHeartbeat
    {
        private IDictionary<string, DateTime> registeredClients;
        private IDictionary<string, int> clientMisses;
        private int threshold;

        public HeartbeatReceiver()
        {
            this.registeredClients = new ConcurrentDictionary<string, DateTime>();
            this.clientMisses = new ConcurrentDictionary<string, int>();
            this.threshold = 500;

            Thread monitorThread = new Thread(new ThreadStart(this.Monitor));
            monitorThread.IsBackground = true;
            monitorThread.Start();
        }

        public string Register()
        {
            string guid = Guid.NewGuid().ToString();

            this.registeredClients.Add(guid, DateTime.Now);
            this.clientMisses.Add(guid, 0);
            Console.WriteLine("Registered new client: {0}", guid);

            return guid;
        }

        public void Unregister(string id)
        {
            this.registeredClients.Remove(id);
            this.clientMisses.Remove(id);
            Console.WriteLine("Unregistered client {0}", id);
        }

        public void Beat(string id)
        {
            if (this.registeredClients.ContainsKey(id))
            {
                this.registeredClients[id] = DateTime.Now;
                this.clientMisses[id] = 0;
                Console.WriteLine("Received heartbeat from {0} on {1}", id, DateTime.Now);
            }
            else
            {
                Console.WriteLine("WARNING: Received heartbeat from unknown client {0}", id);
            }
        }

        public void Monitor()
        {
            while (true)
            {
                Thread.Sleep(this.threshold);

                DateTime now = DateTime.Now;
                foreach(KeyValuePair<string, DateTime> pair in this.registeredClients)
                {
                    TimeSpan span = now - pair.Value;
                    int ms = (int) span.TotalMilliseconds;

                    if (ms > this.threshold)
                    {
                        Console.WriteLine("Client {0} has missed a heartbeat!", pair.Key);
                        this.clientMisses[pair.Key] += 1;

                        if (this.clientMisses[pair.Key] >= 3)
                        {
                            Console.WriteLine(
                                "Client {0} has missed three or more heartbeats! Time of death: {1}",
                                pair.Key,
                                DateTime.Now
                            );

                            this.Unregister(pair.Key);
                        }
                    }
                }
            }
        }
    }
}
