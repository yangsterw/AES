using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AES.AesLib
{
    public class Decryption
    {
        private static readonly LibrarySAES lib = new LibrarySAES();

        public Decryption() { }

        /* Step 1: Convert numerical to binary */
        List<int> originalMsg = new List<int>();
        List<int> originalKey = new List<int>();
        string ConvertMessageToBinaryClick(string message)
        {
            originalMsg = lib.ConvertStringOfNumbersToBinary(message);
            return BuildString(originalMsg);
        }

        string ConvertKeyToBinary(string key)
        {
            originalKey = lib.ConvertStringOfNumbersToBinary(key);
            return BuildString(originalKey);
        }



        string BuildString(List<int> inputList)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var x in inputList)
                builder.Append(x);

            return builder.ToString();
        }
    }
}
