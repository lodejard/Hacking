using System;
using System.Reflection;

namespace ConsoleDelta
{
    public class Program
    {
        public void Main(string[] args)
        {
            var x = new LibraryGamma.Class1();
            var y = new LibraryBeta.Class1();
            var z = new LibraryAlpha.Class1();

            Console.WriteLine(
                "Hello " + x.ToString() + " " + y.ToString() + " " + z.ToString());

            Console.ReadLine();
        }
    }
}
