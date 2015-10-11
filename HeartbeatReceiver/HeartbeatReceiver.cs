using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HeartbeatReceiver
{
    class HeartbeatReceiver : IHeartbeat
    {
        private static string active = null;
        private static ConcurrentQueue<string> standbys = new ConcurrentQueue<string>();
        private static IDictionary<string, DateTime> registeredClients = new ConcurrentDictionary<string, DateTime>();
        private static IDictionary<string, int> clientMisses = new ConcurrentDictionary<string, int>();
        private int threshold;

        public HeartbeatReceiver()
        {
            this.threshold = 500;

            Thread monitorThread = new Thread(new ThreadStart(this.Monitor));
            monitorThread.IsBackground = true;
            monitorThread.Start();
        }

        public Tuple<bool, string> Register()
        {
            string guid = Guid.NewGuid().ToString();

            Tuple<bool, string> result;
            if (HeartbeatReceiver.active == null)
            {
                result = new Tuple<bool, string>(true, guid);
                HeartbeatReceiver.active = guid;
                Console.WriteLine("Registered new active client: {0}", guid);
            }
            else
            {
                result = new Tuple<bool, string>(false, guid);
                HeartbeatReceiver.standbys.Enqueue(guid);
                Console.WriteLine("Registered new standby client: {0}", guid);
            }

            HeartbeatReceiver.registeredClients.Add(guid, DateTime.Now);
            HeartbeatReceiver.clientMisses.Add(guid, 0);

            return result;
        }

        public void Unregister(string id)
        {
            HeartbeatReceiver.registeredClients.Remove(id);
            HeartbeatReceiver.clientMisses.Remove(id);
            Console.WriteLine("Unregistered client {0}", id);

            if (HeartbeatReceiver.active == id)
            {
                this.Promote();
            }
        }

        public bool Beat(string id)
        {
            bool isActive = false;

            if (HeartbeatReceiver.registeredClients.ContainsKey(id))
            {
                HeartbeatReceiver.registeredClients[id] = DateTime.Now;
                HeartbeatReceiver.clientMisses[id] = 0;
                Console.WriteLine("Received heartbeat from {0} on {1}", id, DateTime.Now);

                if (HeartbeatReceiver.active == id)
                {
                    isActive = true;
                }
            }
            else
            {
                Console.WriteLine("WARNING: Received heartbeat from unknown client {0}", id);
            }

            return isActive;
        }

        public void Monitor()
        {
            while (true)
            {
                Thread.Sleep(this.threshold);

                DateTime now = DateTime.Now;
                foreach(KeyValuePair<string, DateTime> pair in HeartbeatReceiver.registeredClients)
                {
                    TimeSpan span = now - pair.Value;
                    int ms = (int) span.TotalMilliseconds;

                    if (ms > this.threshold)
                    {
                        Console.WriteLine("Client {0} has missed a heartbeat!", pair.Key);
                        if (HeartbeatReceiver.clientMisses.ContainsKey(pair.Key))
                        {
                            HeartbeatReceiver.clientMisses[pair.Key] += 1;

                            if (HeartbeatReceiver.clientMisses[pair.Key] >= 3)
                            {
                                Console.WriteLine(
                                    "Client {0} has missed three or more heartbeats! Time of death: {1}",
                                    pair.Key,
                                    DateTime.Now
                                );
                                
                                this.Unregister(pair.Key);

                                if (HeartbeatReceiver.active == pair.Key)
                                {
                                    this.Promote();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void Promote()
        {
            Console.WriteLine("Lost our active, promoting the next standby to active.");

            if (HeartbeatReceiver.standbys.Count == 0)
            {
                Console.WriteLine("No standbys available!");
                HeartbeatReceiver.active = null;
            }
            else
            {
                string newActive;
                if (HeartbeatReceiver.standbys.TryDequeue(out newActive))
                {
                    Console.WriteLine("Promoted client {0} to active.", active);
                    HeartbeatReceiver.active = newActive;
                }
                else
                {
                    Console.WriteLine("Failed to promote client {0} to active.", newActive);
                }
            }
        }
    }
}
