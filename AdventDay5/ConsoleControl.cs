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
            parser.Parse();
            PrintCache();
        }

        public static void PrintCache()
        {
            if(Cache.PassCode == null || Cache.PassCode.Count == 0)
                Console.WriteLine("No special Hashes have been found!");
            else
            {
                Cache.PassCode.Keys.ToList().Sort();
                foreach (var element in Cache.PassCode)
                {
                    Console.Write(element.Value.ToString());
                }
                Console.WriteLine();
            }

        }
    }
}
