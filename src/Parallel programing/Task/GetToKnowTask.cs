namespace MyTask {

    public class GetToKnowTask {
        public static void Write(char c) {
            int i = 1000;
            while (i-- > 0) {
                Console.Write(c);
            }
        }

        public static void Write(object o)
        {
            int i = 1000;
            while (i-- > 0)
            {
                Console.Write(o);
            }
        }

        public static int TextLength(object o)
        {
            Console.WriteLine($"\nTask with id {Task.CurrentId} is currently processing object {o}...");
            return o.ToString().Length;
        }


        public static void Run() {
            // Making a task, AND starting it!
            //Task.Factory.StartNew(() => Write('.'));

            // Making a task, but not starting it!
            //var t = new Task(() => Write('?'));
            // Now starts it:
            //t.Start();


            //Task t1 = new Task(Write, "hello");
            //t1.Start();

            //Task.Factory.StartNew(Write, 123);

            string text1 = "testing", text2 = "this";

            var task1 = new Task<int>(TextLength, text1);
            task1.Start();

            Task<int> task2 = Task.Factory.StartNew(TextLength, text2);

            Console.WriteLine($"Length of {text1} is {task1.Result}");
            Console.WriteLine($"Length of {text2} is {task2.Result}");

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}