using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventDay5
{
    class ConsoleController
    {
        private static string adventCode;

        //status events here..
        public static void GetCodeFromUser()
        {
            Console.WriteLine("Enter Advent Code: ");
            adventCode = Console.ReadLine();

            HashParser parser = new HashParser(new HashBuilder(adventCode));
            parser.InParallel();
            PrintCache();
        }

        public static void PrintCache()
        {
            if(Cache.passCode == null || Cache.passCode.Count == 0)
                Console.WriteLine("No special Hashes have been found!");
            else
            {
                foreach (var element in Cache.passCode)
                {
                    Console.Write(element.Value.ToString());
                }
                Console.WriteLine();
            }

        }
    }
}
