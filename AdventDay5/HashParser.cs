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

        private bool isValid(List<char> zerosList)//string hash
        {
            return (zerosList.Count == 5);
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

            while (Cache.PassCode.Count < 9)
            {
                try
                {
                    Parallel.For(startingVal, searchRange, increment =>
                    {
                        if (Cache.PassCode.Count == 8) {source.Cancel();}
                        if (token.IsCancellationRequested)
                            token.ThrowIfCancellationRequested();
                        
                        lock (dummy)
                        {
                            string hash = CreateSerializableHash(increment);
                            zerosList = new List<char>();
                            int digit = 0;
                            while (digit < 5)
                            {
                                if (hash[digit] == '0')
                                    zerosList.Add(hash[digit]);
                                else return;
                                digit++;
                            }
                            if (isValid(zerosList))
                                Cache.Save(hash, hash[5], increment);
                            increment++;
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
                if (Cache.PassCode.Count == 8) break;
                startingVal = searchRange;
                searchRange += 50000;
            }
        }
    }

    static class Cache
    {
        private static List<string> specialHashes = new List<string>();
        private static readonly Dictionary<int, char> IndexFound = new Dictionary<int, char>();
        public static Dictionary<int, char> PassCode => IndexFound;

        public static void Save(string specHash, char specChar, int incr)
        {
            specialHashes.Add(specHash);
            IndexFound.Add(incr, specChar);
        }
    }
}
