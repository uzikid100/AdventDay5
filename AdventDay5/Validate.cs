using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventDay5
{
    abstract class Validate : IValidate
    {
        protected List<char> specialChars;
        protected Validate(List<char> specialChars)
        {
            this.specialChars = specialChars;
        }
        public virtual bool isValid()
        {
            return specialChars != null ? specialChars.Count == 5 : false;
        }
        public class SimpleValidate : Validate
        {
            public SimpleValidate(List<char> specialChars)
                :base(specialChars)
            {
            }
        }

        public class ExtensiveValidate : Validate
        {
            private char charToValidate;
            public ExtensiveValidate(List<char> specialChars, char charToValidate)
                :base(specialChars)
            {
                this.charToValidate = charToValidate;
            }

            public override bool isValid()
            {
                int num;
                bool converted = int.TryParse(charToValidate.ToString(), out num);

                if (specialChars.Count == 5 && converted)
                {
                    if(num>=0 && num<=7)
                    return true;
                }
                return false;
            }
        }
    }

}
