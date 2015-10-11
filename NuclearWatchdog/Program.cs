using System;
using System.Threading;
using NuclearWatchdog.Heartbeat;
using NuclearWatchdog.Reactor;

namespace NuclearWatchdog
{
    class ReactorMonitor
    {
        private Random random;
        private HeartbeatSender heartbeatSender;
        private ReactorClient reactor;
        private double threshold;

        public ReactorMonitor(HeartbeatSender heartbeatSender, ReactorClient reactor, double threshold)
        {
            this.random = new Random();
            this.heartbeatSender = heartbeatSender;
            this.reactor = reactor;
            this.threshold = threshold;
        }

        public void Monitor()
        {
            while (true)
            {
                Thread.Sleep(500);

                int temperature = this.reactor.GetTemperature();
                Console.WriteLine("Reactor temperature is now: {0}", temperature);
                if (temperature > this.threshold)
                {
                    Console.WriteLine("[ALARM] Reactor is critical!");
                    if (HeartbeatSender.isActive && this.random.NextDouble() > 0.5)
                    {
                        Thread.Sleep(5000);
                        throw new Exception("system crash");
                    }
                }
            }
        }
    }

    class HeartbeatSender
    {
        private HeartbeatClient client;
        private string guid;
        public static bool isActive;

        public HeartbeatSender(HeartbeatClient client)
        {
            this.client = client;
            this.guid = null;
            isActive = false;
        }

        public void Register()
        {
            Console.Write("Registering with heartbeat receiver... ");
            Tuple<bool, string> result = this.client.Register();

            isActive = result.Item1;
            this.guid = result.Item2;

            Console.WriteLine("ID: {0}", this.guid);

            if (isActive)
            {
                Console.WriteLine("[ALERT] Registered in active mode.");
            }
            else
            {
                Console.WriteLine("[ALERT] Registered in standby mode.");
            }
        }

        public void Unregister()
        {
            Console.WriteLine("Unregistering from heartbeat receiver...");

            this.client.Unregister(this.guid);
        }

        public void Beat()
        {
            while (true)
            {
                Thread.Sleep(500);
                Console.WriteLine("Sending heartbeat...");

                bool isPromoted = this.client.Beat(this.guid);
                if (!isActive && isPromoted)
                {
                    isActive = true;
                    Console.WriteLine("[ALERT] Promoted to active!");
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Nuclear Watchdog");

            HeartbeatClient client = new HeartbeatClient();
            HeartbeatSender heartbeatSender = new HeartbeatSender(client);

            try {
                heartbeatSender.Register();
            } catch (System.ServiceModel.EndpointNotFoundException)
            {
                Console.WriteLine("Failed to connect to the heartbeat receiver service.");
                Console.WriteLine("Are you sure the service is running?");
                Environment.Exit(1);
            }

            ReactorClient reactor = new ReactorClient();
            ReactorMonitor reactorMonitor = new ReactorMonitor(heartbeatSender, reactor, 350);

            Thread reactorMonitorThread = new Thread(new ThreadStart(reactorMonitor.Monitor));
            reactorMonitorThread.Start();

            Thread heartbeatThread = new Thread(new ThreadStart(heartbeatSender.Beat));
            heartbeatThread.Start();

            // Run for 60 seconds
            Thread.Sleep(60000);
            
            reactorMonitorThread.Abort();
            reactorMonitorThread.Join();

            heartbeatThread.Abort();
            heartbeatThread.Join();

            heartbeatSender.Unregister();

            Console.WriteLine("Stopping Nuclear Watchdog");
        }
    }
}
