﻿using System;
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
    
        public void InParallel()
        {
            object dummy = new object();
            List<char> zerosList = null;
            mFatalErr = new StringBuilder
                ("UNEXPECTED ERROR: ");
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            try
            {
                Parallel.For(0, int.MaxValue/3, increment =>
                {
                    if (Cache.passCode.Count == 8) { source.Cancel(); }
                    if (token.IsCancellationRequested)
                        return;
                        //token.ThrowIfCancellationRequested();

                    string hash = CreateSerializableHash(increment);
                    lock (dummy)
                    {
                        int digit = 0;
                        zerosList = new List<char>();
                        while (digit < 5)
                        {
                            if (hash[digit] == '0') { zerosList.Add(hash[digit]); }
                            digit++;
                        }
                        if (isValid(zerosList))
                        {
                            Cache.Save(hash, hash[5], increment);
                        }
                        increment++;
                    }
                });
            }
            catch (Exception ex)
            {
                if(ex.GetType() == typeof(OperationCanceledException))
                {
                    return;
                }
                mFatalErr.Append(ex.Message);
                Console.WriteLine(mFatalErr);
            }
        }
    }

    static class Cache
    {
        private static List<string> specialHashes = new List<string>();
        //private static List<char> specialChars = new List<char>();
        private static Dictionary<int, char> indexFound = new Dictionary<int, char>();
        //public static List<char> passCode => specialChars;
        public static Dictionary<int, char> passCode => indexFound;
        public static void Save(string specHash, char specChar, int incr)
        {
            specialHashes.Add(specHash);
            //specialChars.Add(specChar);
            indexFound.Add(incr, specChar);
        }

        private static void sort()
        {
            if(indexFound.Count == 8)
            {
            }
        }
    }
}
