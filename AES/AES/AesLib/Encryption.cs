using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AES.AesLib
{
    public class Encryption
    {
        private static readonly LibrarySAES lib = new LibrarySAES();
        public Encryption() { }

        /* Step 1: Convert numerical to binary */
        List<int> originalMsg = new List<int>();
        List<int> originalKey = new List<int>();
        string ConvertMessageToBinaryOnEncryption(string message)
        {
            originalMsg = lib.ConvertStringOfNumbersToBinary(message);
            return BuildString(originalMsg);
        }

        string ConvertKeyToBinaryOnEncryption(string key)
        {
            originalKey = lib.ConvertStringOfNumbersToBinary(key);
            return BuildString(originalKey);
        }

        /* Step 2: Permutation */
        List<int> step2PermutateResult = new List<int>();
        string PermutateKeyOnEncryption()
        {
            step2PermutateResult = lib.PermutationOne(originalKey);
            return BuildString(step2PermutateResult);
        }

        /* Step 3: XOR the original message with the key */
        List<int> step3XorResult = new List<int>();
        string XorEvaluationOnEncryption()
        {
            step3XorResult = lib.ExorEvaluation(originalMsg, originalKey);
            return BuildString(step3XorResult);
        }

        /* Step 4: Substitute bytes, shift-1-right */
        List<int> step4SubsResult = new List<int>();
        string ShiftOneRightOnEncryption()
        {
            step4SubsResult = lib.ShiftOneRight(step3XorResult);
            return BuildString(step4SubsResult);
        }

        /* Step 5: Shift rows */
        List<int> step5ShiftResult = new List<int>();
        string ShiftingRowsToLeftOnEncryption()
        {
            step5ShiftResult = lib.ShiftRowsSingleRound(step4SubsResult);
            return BuildString(step5ShiftResult);
        }
        
        /* Step 6: Shift columns */
        List<int> step6ShiftResult = new List<int>();
        string MixColumnVerticallyDownOnEncryption()
        {
            step6ShiftResult = lib.MixColumnVerticallyOneRound(step5ShiftResult);
            return BuildString(step6ShiftResult);
        }

        /* Step 7: XOR Step6Result with PermutatedKey */
        List<int> step7XorResult = new List<int>();
        void XorWithPermutatedKeyOnEncryption()
        {
            step7XorResult = lib.ExorEvaluation(step6ShiftResult, step2PermutateResult);
        }
                          
        string ShowEncryptedMessage()
        {
            return BuildString(step7XorResult);
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
