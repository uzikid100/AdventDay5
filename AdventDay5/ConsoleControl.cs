using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventDay5
{
    class ConsoleController
    {
        private static string adventCode;
        //status events here..
        public static void Init()
        {
            Console.WriteLine("Enter Advent Code: ");
            adventCode = Console.ReadLine();

            HashParser parser = new HashParser(new HashBuilder(adventCode));
            parser.Parse();
        }



        public static void PrintCache(Dictionary<int, char> code)
        {
            StringBuilder sb;
            if(code == null || code.Count == 0)
                Console.WriteLine("No special Hashes have been found!");
            else
            {
                sb = new StringBuilder("Hash Found! ");
                foreach (var element in code.OrderBy(i => i.Key))
                    sb.Append(element.Value.ToString());
                Console.WriteLine(sb.ToString());
            }
        }
    }
}
