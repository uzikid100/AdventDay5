using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventDay5
{
    public interface IValidateHash
    {
        bool isValid(string hash);
    }

    public interface IParser
    {
        IEnumerable<char> Parse();
    }

}
