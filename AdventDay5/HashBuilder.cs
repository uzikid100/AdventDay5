using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace AdventDay5
{
    class HashBuilder
    {
        private StringBuilder mAdventCode = null;
        private string hash = null;
        //raise building event...report building status
        public string adventCode => mAdventCode.ToString();

        public HashBuilder(string adventCode)
        {
            this.mAdventCode = new StringBuilder(adventCode);
        }

        public string Build(int increment)
        {
            StringBuilder tempCode = new StringBuilder(mAdventCode.ToString());
            //Trap bad hash...
            if(tempCode == null)
            { throw new InvalidOperationException("Cannot build hash from type: NULL"); }
            else
            {
                try
                {
                    string source = tempCode.Append(increment.ToString()).ToString();
                    using (MD5 md5 = MD5.Create())
                    {
                        hash = GetMd5Hash(md5, source);
                    }
                }
                catch (Exception e) { throw; }
            }
            return hash;
        }

        private string GetMd5Hash(MD5 md5, string source)
        {
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(source));
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(data[i].ToString("x2"));
            }
            return builder.ToString();
        }

    }
}
