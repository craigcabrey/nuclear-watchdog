using System;
using System.Threading;
using NuclearWatchdog.Heartbeat;

namespace NuclearWatchdog
{
    class Reactor
    {
        const int MINIMUM_TEMPERATURE = 100;
        const int MAXIMUM_TEMPERATURE = 400;

        private Random randomGenerator;

        public int Temperature
        {
            get;
            set;
        }

        public Reactor()
        {
            this.randomGenerator = new Random();
            this.Temperature = this.randomGenerator.Next(MINIMUM_TEMPERATURE, MAXIMUM_TEMPERATURE);
        }

        public void React()
        {
            while (true)
            {
                Thread.Sleep(100);

                this.Temperature = this.randomGenerator.Next(MINIMUM_TEMPERATURE, MAXIMUM_TEMPERATURE);
                if (this.Temperature == MINIMUM_TEMPERATURE)
                {
                    this.Temperature = 0;
                }
            }
        }
    }

    class ReactorMonitor
    {
        private HeartbeatSender heartbeatSender;
        private Reactor reactor;
        private double threshold;

        public ReactorMonitor(HeartbeatSender heartbeatSender, Reactor reactor, double threshold)
        {
            this.heartbeatSender = heartbeatSender;
            this.reactor = reactor;
            this.threshold = threshold;
        }

        public void Monitor()
        {
            while (true)
            {
                Thread.Sleep(500);

                Console.WriteLine("Reactor temperature is now: {0}", this.reactor.Temperature);
                if (this.reactor.Temperature > this.threshold)
                {
                    Console.WriteLine("Reactor is critical!");
                }
            }
        }
    }

    class HeartbeatSender
    {
        private HeartbeatClient client;

        public HeartbeatSender(HeartbeatClient client)
        {
            this.client = client;
        }

        public void Register()
        {
            Console.WriteLine("Registering with heartbeat receiver...");

            this.client.Register();
        }

        public void Unregister()
        {
            Console.WriteLine("Unregistering from heartbeat receiver...");

            this.client.Unregister();
        }

        public void Beat()
        {
            while (true)
            {
                Thread.Sleep(500);
                Console.WriteLine("Sending heartbeat...");

                this.client.Beat();
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
            heartbeatSender.Register();

            Reactor reactor = new Reactor();
            ReactorMonitor reactorMonitor = new ReactorMonitor(heartbeatSender, reactor, 350);

            Thread reactorThread = new Thread(new ThreadStart(reactor.React));
            reactorThread.Start();

            Thread reactorMonitorThread = new Thread(new ThreadStart(reactorMonitor.Monitor));
            reactorMonitorThread.Start();

            Thread heartbeatThread = new Thread(new ThreadStart(heartbeatSender.Beat));
            heartbeatThread.Start();

            // Run for 30 seconds
            Thread.Sleep(30000);
            
            reactorMonitorThread.Abort();
            reactorMonitorThread.Join();

            reactorThread.Abort();
            reactorThread.Join();

            heartbeatThread.Abort();
            heartbeatThread.Join();

            heartbeatSender.Unregister();

            Console.WriteLine("Stopping Nuclear Watchdog");
        }
    }
}
