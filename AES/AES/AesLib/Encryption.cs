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
        string ConvertMessageToBinaryOnEncryption(string message)
        {
            List<int> originalMsg = lib.ConvertStringOfNumbersToBinary(message);
            return BuildString(originalMsg);
        }

        string ConvertKeyToBinaryOnEncryption(string key)
        {
            List<int> originalKey = lib.ConvertStringOfNumbersToBinary(key);
            return BuildString(originalKey);
        }

        /* Step 2: Permutation */        
        string PermutateKeyOnEncryption(string inputString)   
        {
            List<int> originalKey = new List<int>();
            foreach (var x in inputString)
                originalKey.Add(x-48);

            List<int> step2PermutateResult = lib.PermutationOne(originalKey);
            return BuildString(step2PermutateResult);
        }

        /* Step 3: XOR the original message with the key */
        string XorEvaluationOnEncryption(string inputString, string keyString)
        {
            List<int> originalMsg = new List<int>();
            foreach (var x in inputString)
                originalMsg.Add(x - 48);

            List<int> originalKey = new List<int>();
            foreach (var x in keyString)
                originalKey.Add(x - 48);

            List<int> step3XorResult = lib.ExorEvaluation(originalMsg, originalKey);
            return BuildString(step3XorResult);
        }

        /* Step 4: Substitute bytes, shift-1-right */
        public static string ShiftOneRightOnEncryption(string inputString)
        {
            List<int> step3XorResult = new List<int>();
            foreach (var x in inputString)
                step3XorResult.Add(x-48);

            List<int> step4SubsResult = lib.ShiftOneRight(step3XorResult);
            return BuildString(step4SubsResult);
        }

        /* Step 5: Shift rows */
        string ShiftingRowsToLeftOnEncryption(string inputString)
        {
            List<int> step4SubsResult = new List<int>();
            foreach (var x in inputString)
                step4SubsResult.Add(x - 48);

            List<int> step5ShiftResult = lib.ShiftRowsSingleRound(step4SubsResult);
            return BuildString(step5ShiftResult);
        }
        
        /* Step 6: Shift columns */
        string MixColumnVerticallyDownOnEncryption(string inputString)
        {
            List<int> step5ShiftResult = new List<int>();
            foreach (var x in inputString)
                step5ShiftResult.Add(x - 48);

            List<int> step6ShiftResult = lib.MixColumnVerticallyOneRound(step5ShiftResult);
            return BuildString(step6ShiftResult);
        }

        /* Step 7: XOR Step6Result with PermutatedKey */
        string XorWithPermutatedKeyOnEncryption(string inputString, string keyString)
        {
            List<int> step6ShiftResult = new List<int>();
            foreach (var x in inputString)
                step6ShiftResult.Add(x - 48);

            List<int> step2PermutateResult = new List<int>();
            foreach (var x in keyString)
                step2PermutateResult.Add(x - 48);

            List<int> step7XorResult = lib.ExorEvaluation(step6ShiftResult, step2PermutateResult);
            return BuildString(step7XorResult);
        }          

        public static string BuildString(List<int> inputList)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var x in inputList)
                builder.Append(x);

            return builder.ToString();
        }

        string Convert2DMatrixToString(int[,] inputMatrix)
        {
            StringBuilder builder = new StringBuilder();
            for(int i = 0; i < 16; i++) {
                for(int j = 0; j < 16; j++) {
                    builder.Append(inputMatrix[i,j]);
                }
            }
            return builder.ToString();
        }
        
        int[,] ConvertStringTo2DMatirx(string inputString)
        {
            int[,] outputMatrix = new int[4,4];
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 16; i++) {
                for (int j = 0; j < 16; j++) {
                    outputMatrix[i, j] = inputString[i];
                }
            }
            return outputMatrix;
        }

    }
}
