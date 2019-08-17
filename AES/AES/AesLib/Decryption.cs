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
        string ConvertMessageToBinaryOnDecryption(string message)
        {
            List<int> originalMsg = lib.ConvertStringOfNumbersToBinary(message);
            return BuildString(originalMsg);
        }

        string ConvertKeyToBinaryOnDecryption(string key)
        {
            List<int> originalKey = lib.ConvertStringOfNumbersToBinary(key);
            return BuildString(originalKey);
        }

        /* Step 2: Permutation */
        public static string PermutateKeyOnDecryption(string keyString)
        {
            List<int> originalKey = new List<int>();
            foreach (var x in keyString)
                originalKey.Add(x - 48);

            List<int> step2PermutateResult = lib.PermutationOne(originalKey);
            return BuildString(step2PermutateResult);
        }

        /* Step 3: XOR the original message with the key */
        string XorEvaluationOnDecryption(string inputString, string keyString)
        {
            List<int> originalMsg = new List<int>();
            foreach (var x in inputString)
                originalMsg.Add(x - 48);

            List<int> step2PermutateResult = new List<int>();
            foreach (var x in keyString)
                step2PermutateResult.Add(x - 48);

            List<int> step3XorResult = lib.ExorEvaluation(originalMsg, step2PermutateResult);
            return BuildString(step3XorResult);
        }

        /* Step 4: Mix column vertically upward */        
        string MixColumnVerticallyUpwardOnDecryption(string inputString)
        {
            List<int> step3XorResult = new List<int>();
            foreach (var x in inputString)
                step3XorResult.Add(x - 48);

            List<int>step4SubsResult = lib.MixColumnVerticallyUpwardOneRound(step3XorResult);
            return BuildString(step4SubsResult);
        }

        /* Step 5: Shift rows */        
        string ShiftingRowsOnDecryption(string inputString)
        {
            List<int> step4SubsResult = new List<int>();
            foreach (var x in inputString)
                step4SubsResult.Add(x - 48);

            List<int> step5ShiftResult = lib.ShiftRowsRightSingleRound(step4SubsResult);
            return BuildString(step5ShiftResult);
        }

        /* Step 6: Shifting left 1 bit */        
        string ShiftingLeftOneBitOnDecryption(string inputString)
        {
            List<int> step5ShiftResult = new List<int>();
            foreach (var x in inputString)
                step5ShiftResult.Add(x - 48);

            List<int> step6ShiftResult = lib.ShiftOneLeft(step5ShiftResult);
            return BuildString(step6ShiftResult);
        }

        /* Step 7: XOR Step6Result with Original key */
        List<int> step7XorResult = new List<int>();
        string XorStepSixWithOriginalKeyOnDecryption(string inputString, string keyString)
        {
            List<int> step6ShiftResult = new List<int>();
            foreach (var x in inputString)
                step6ShiftResult.Add(x - 48);

            List<int> originalKey = new List<int>();
            foreach (var x in keyString)
                originalKey.Add(x - 48);

            step7XorResult = lib.ExorEvaluation(step6ShiftResult, originalKey);
            return BuildString(step7XorResult);
        }

        string ShowDecryptedMessage()
        {
            List<string> step8Result = lib.ConvertBinaryToHexString(step7XorResult);
            StringBuilder builder = new StringBuilder();
            foreach (var x in step8Result)
                builder.Append(x);
            return builder.ToString();
        }

        public static string BuildString(List<int> inputList)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var x in inputList)
                builder.Append(x);

            return builder.ToString();
        }
    }
}
