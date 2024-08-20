using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.Parallel_programing.Coordination.BarrierExample
{
    public class Demo
    {
        static Barrier barrier = new Barrier(2, b =>
        {
            Console.WriteLine($"Phase {b.CurrentPhaseNumber} is finished");
            //Console.WriteLine($"Participants remaining: {b.ParticipantsRemaining}");
            // Also more APIs: b.AddParticipant
        });

        public static void Water()
        {
            Console.WriteLine("Putting the kettle on (takes a bit longer)"); // phase 0
            Thread.Sleep(2000);
            barrier.SignalAndWait(); // counter will be 2
            Console.WriteLine("Pouring water into the cup"); // phase 1
            barrier.SignalAndWait();
            Console.WriteLine("Putting the kettle away"); // phase 2
        }

        public static void Cup()
        {
            Console.WriteLine("Finding the nicest cup of tea (fast)"); // phase 0
            barrier.SignalAndWait(); // signal wait. count will be 1 cause waiting for water
            Console.WriteLine("Adding tea."); // phase 1
            barrier.SignalAndWait();
            Console.WriteLine("Adding sugar"); // phase 2
        }

        public static void Run()
        {
            var water = Task.Factory.StartNew(Water);
            var cup = Task.Factory.StartNew(Cup);

            Task.Factory.ContinueWhenAll(new[] { water, cup }, tasks =>
            {
                Console.WriteLine("Enjoy your cup of tea.");
            }).Wait();


        }
    }
}
