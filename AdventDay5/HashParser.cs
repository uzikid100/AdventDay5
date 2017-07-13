using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AdventDay5
{
    class HashParser 
    {
        private HashBuilder mBuilder = null;
        private StringBuilder mFatalErr = null;
        private bool printedSimpleValidation = false;
        private bool printedExtensiveValidation = false;
        public StringBuilder FatalErr => mFatalErr;
        //raise parsing event.. report parsing status

        public HashParser(HashBuilder builder)
        {
            this.mBuilder = builder;
        }

        private string CreateSerializableHash(int increment)
        {
            if (mBuilder != null) { return mBuilder.Build(increment); }
            else throw new InvalidOperationException
                    ("Failed to create Serializable Hash");
        }

        public void Parse()
        {
            object dummy = new object();
            List<char> zerosList = null;
            int searchRange = 100000;
            int startingVal = 0;
            mFatalErr = new StringBuilder
                ("UNEXPECTED ERROR: ");

            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            while (!Completed())
            {
                try
                {
                    Parallel.For(startingVal, searchRange, index =>
                    {
                        if (Cache.ExtensiveCode.Count == 8) {source.Cancel();}
                        if (token.IsCancellationRequested)
                            token.ThrowIfCancellationRequested();
                        
                        lock (dummy)
                        {
                            string hash = CreateSerializableHash(index);
                            zerosList = new List<char>();
                            int i = 0;
                            while (i < 5)
                            {
                                if (hash[i] == '0')
                                    zerosList.Add(hash[i]);
                                else return;
                                i++;
                            }
                            if (new Validate.SimpleValidate(zerosList).isValid())
                                Cache.Save(hash, hash[5], index);
                            if (new Validate.ExtensiveValidate(zerosList, hash[5]).isValid())
                                Cache.Save(hash, hash[5], hash[6], index);
                            index++;
                        }
                    });
                }
                catch (Exception ex)
                {
                    if (ex.GetType() == typeof(OperationCanceledException)) { }
                    else
                    {
                        mFatalErr.Append(ex.Message);
                    }
                }
                if (Completed()) break;
                startingVal = searchRange;
                searchRange += 50000;
            }
        }

        private bool Completed()
        {
            if (Cache.SimpleCode.Count >= 8)
            {
                if (!printedSimpleValidation)
                {
                    Cache.Sort();
                    ConsoleController.PrintCache(Cache.SimpleCode);
                    printedSimpleValidation = true;
                }
            }

            if(Cache.ExtensiveCode.Count == 8 && !printedExtensiveValidation)
            {
                Cache.Sort();
                ConsoleController.PrintCache(Cache.ExtensiveCode);
                printedExtensiveValidation = true;
            }
            return printedExtensiveValidation && printedSimpleValidation ? true : false;
        }
    }

    static class Cache
    {
        private static List<string> specialHashes = new List<string>();
        private static Dictionary<int, char> extensiveCode = new Dictionary<int, char>();
        private static readonly Dictionary<int, char> simpleCode = new Dictionary<int, char>();

        public static Dictionary<int, char> SimpleCode => simpleCode;
        public static Dictionary<int, char> ExtensiveCode => extensiveCode;


        public static void Save(string specHash, char specChar, int indexFound)
        {
            specialHashes.Add(specHash);
            simpleCode.Add(indexFound, specChar);
        }

        public static void Save(string specHash, char key, char val, int indexFound)
        {
            specialHashes.Add(specHash);
            int num = int.Parse(key.ToString());
            extensiveCode.Add(num, val);
        }

        public static void Sort()
        {
            extensiveCode.OrderBy(i => i.Key);
            simpleCode.OrderBy(i => i.Key);
        }

    }
}
