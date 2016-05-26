using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using func;

namespace funcexe
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("addup :{0:G}",Class1.addup(1,2));
            Console.WriteLine("minus :{0:G}", Class1.minus(1,2));
            Console.ReadKey();
        }
    }
}
