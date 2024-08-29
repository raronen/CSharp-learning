using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLearnings.src.C__Performance.Intermediate.FastClassFactory
{
    public class FastClassFactory
    {
        private const int repetitions = 10000000;

        private static Dictionary<string, ClassCreator> classCreatorsDic = new Dictionary<string, ClassCreator>();
        private class ClassCreator //: Delegate
        {
        }

        // Creates object via Switch-case   
        // Advantage: speed
        // Disadvantage: Must know all typeNames in advance!
        public static long MeasureA(string typeName)
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            for (int i = 0; i < repetitions; i++)
            {
                switch (typeName)
                {
                    case "System.Text.StringBuilder":
                        new StringBuilder();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        // Creates object via Reflection
        public static long MeasureB(string typeName)
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            for (int i = 0; i < repetitions; i++)
            {
                Activator.CreateInstance(Type.GetType(typeName));
            }
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        // Creates object via Delegate
        // Slow only for the first time, afterwards it's real fast.
        // Don't need to know all types in advance.
        public static long MeasureC(string typeName)
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            for (int i = 0; i < repetitions; i++)
            {
                ClassCreator classCreator = GetClassCreator(typeName);
                //classCreator();
            }
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        private static ClassCreator GetClassCreator(string typeName)
        {
            // get delegate from dictionary
            if (classCreatorsDic.ContainsKey(typeName))
                return classCreatorsDic[typeName];

            // get the default constructor of the type
            Type t = Type.GetType(typeName);
            ConstructorInfo ctor = t.GetConstructor(new Type[0]);

            // create a new dynamic method that constructs and return the type
            string methodName = t.Name + "Ctor";
            DynamicMethod dm = new DynamicMethod(methodName, t, new Type[0], typeof(Object).Module);
            ILGenerator lGenerator = dm.GetILGenerator();
            lGenerator.Emit(OpCodes.Newobj, ctor);
            lGenerator.Emit(OpCodes.Ret);
            
            // add delegate to dictionary and return
            //ClassCreator creator = (ClassCreator)dm.CreateDelegate(typeof(ClassCreator));
            //classCreatorsDic.Add(typeName, creator);
            return null;


        }

        public static void Run()
        {

        }
    }
}
