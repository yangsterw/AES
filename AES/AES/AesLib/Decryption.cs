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
        string ConvertMessageToBinaryOnDecryption(string message)
        {
            originalMsg = lib.ConvertStringOfNumbersToBinary(message);
            return BuildString(originalMsg);
        }

        string ConvertKeyToBinaryOnDecryption(string key)
        {
            originalKey = lib.ConvertStringOfNumbersToBinary(key);
            return BuildString(originalKey);
        }

        /* Step 2: Permutation */
        List<int> step2PermutateResult = new List<int>();
        string PermutateKeyOnDecryption()
        {
            step2PermutateResult = lib.PermutationOne(originalKey);
            return BuildString(step2PermutateResult);
        }

        /* Step 3: XOR the original message with the key */
        List<int> step3XorResult = new List<int>();
        string XorEvaluationOnDecryption()
        {
            step3XorResult = lib.ExorEvaluation(originalMsg, step2PermutateResult);
            return BuildString(step3XorResult);
        }

        /* Step 4: Mix column vertically upward */
        List<int> step4SubsResult = new List<int>();
        string MixColumnVerticallyUpwardOnDecryption()
        {
            step4SubsResult = lib.MixColumnVerticallyUpwardOneRound(step3XorResult);
            return BuildString(step4SubsResult);
        }

        /* Step 5: Shift rows */
        List<int> step5ShiftResult = new List<int>();
        string ShiftingRowsOnDecryption()
        {
            step5ShiftResult = lib.ShiftRowsRightSingleRound(step4SubsResult);
            return BuildString(step5ShiftResult);
        }

        /* Step 6: Shifting left 1 bit */
        List<int> step6ShiftResult = new List<int>();
        string ShiftingLeftOneBitOnDecryption()
        {
            step6ShiftResult = lib.ShiftOneLeft(step5ShiftResult);
            return BuildString(step6ShiftResult);
        }

        /* Step 7: XOR Step6Result with Original key */
        List<int> step7XorResult = new List<int>();
        string XorStepSixWithOriginalKeyOnDecryption()
        {
            step7XorResult = lib.ExorEvaluation(step6ShiftResult, originalKey);
            return BuildString(step7XorResult);
        }

        List<string> step8Result = new List<string>();
        string ShowDecryptedMessage()
        {
            step8Result = lib.ConvertBinaryToHexString(step7XorResult);
            StringBuilder builder = new StringBuilder();
            foreach (var x in step8Result)
                builder.Append(x);
            return builder.ToString();
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
