using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Reactor
{
    class Reactor : IReactor
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

            Thread reactorThread = new Thread(new ThreadStart(this.React));
            reactorThread.Start();
        }

        public void React()
        {
            while (true)
            {
                Thread.Sleep(100);

                this.Temperature = this.randomGenerator.Next(MINIMUM_TEMPERATURE, MAXIMUM_TEMPERATURE);
                Console.WriteLine("Reactor temperature is now: {0}", this.Temperature);
            }
        }

        public int GetTemperature()
        {
            return this.Temperature;
        }
    }
}
