using System;
using System.Collections.Generic;

namespace AES.AesLib
{
    public class LibrarySAES
    {
        public LibrarySAES() { }

        public List<int> ConvertStringOfNumbersToBinary(string inputText)
        {
            List<int> output = new List<int>();
            for (int i = 0; i < inputText.Length; i++)
            {
                int val = (int)Char.GetNumericValue(inputText[i]);
                string binary = Convert.ToString(val, 2);
                if (binary.Length != 4)
                {
                    string newBinary = binary.PadLeft(4, '0');
                    foreach (var s in newBinary)
                    {
                        int temp = (int)Char.GetNumericValue(s);
                        output.Add(temp);
                    }
                }
                else
                {
                    foreach (var s in binary)
                    {
                        int temp = (int)Char.GetNumericValue(s);
                        output.Add(temp);
                    }
                }
            }
            return output;
        }

        public List<int> PermutationOne(List<int> inputList)
        {
            List<int> output = new List<int>();
            output.Add(inputList[14]);  // 0>14
            output.Add(inputList[5]);   // 1>5
            output.Add(inputList[0]);   // 2>0
            output.Add(inputList[11]);  // 3>11
            output.Add(inputList[7]);   // 4>7
            output.Add(inputList[12]);  // 5>12
            output.Add(inputList[2]);   // 6>2
            output.Add(inputList[13]);  // 7>13
            output.Add(inputList[10]);  // 8>10
            output.Add(inputList[8]);   // 9>8
            output.Add(inputList[4]);   // 10>4
            output.Add(inputList[6]);   // 11>6
            output.Add(inputList[1]);   // 12>1
            output.Add(inputList[15]);  // 13>15
            output.Add(inputList[9]);   // 14>9
            output.Add(inputList[3]);   // 15>3
            return output;
        }

        public List<int> ExorEvaluation(List<int> messageBinary, List<int> keyBinary)
        {
            List<int> output = new List<int>();
            for (int i = 0; i < messageBinary.Count; i++)
            {
                var tempVal = 0;
                if (messageBinary[i] == 0 && keyBinary[i] == 1)
                    tempVal = 1;
                else if (messageBinary[i] == 1 && keyBinary[i] == 0)
                    tempVal = 1;
                else
                    tempVal = 0;

                output.Add(tempVal);
            }
            return output;
        }

        public List<int> ShiftOneRight(List<int> inputList)
        {
            List<int> output = new List<int>();
            output.Add(inputList[15]);
            for (int i = 1; i < inputList.Count; i++)
            {
                output.Add(inputList[i - 1]);
            }
            return output;
        }

        public List<int> ShiftRowsSingleRound(List<int> inputList)
        {
            List<int> output = new List<int>();
            List<int> row0 = new List<int>();
            List<int> row1 = new List<int>();
            List<int> row2 = new List<int>();
            List<int> row3 = new List<int>();

            for (int i = 0; i < inputList.Count; i++)
            {
                if (i < 4)
                    row0.Add(inputList[i]);
                else if (i > 3 && i < 8)
                    row1.Add(inputList[i]);
                else if (i > 7 && i < 12)
                    row2.Add(inputList[i]);
                else
                    row3.Add(inputList[i]);
            }

            output = row0;
            List<int> shiftedRow1 = ShiftOneLeftRowOne(row1);
            output.AddRange(shiftedRow1);
            List<int> shiftedRow2 = ShiftTwoLeftRowTwo(row2);
            output.AddRange(shiftedRow2);
            List<int> shiftedRow3 = ShiftThreeLeftRowThree(row3);
            output.AddRange(shiftedRow3);

            return output;
        }

        public List<int> ShiftOneLeftRowOne(List<int> inputList)
        {
            List<int> output = new List<int>();
            for (int i = 0; i < inputList.Count; i++)
            {
                if (i == 3)
                    output.Add(inputList[0]);
                else
                    output.Add(inputList[i + 1]);
            }
            return output;
        }

        public List<int> ShiftTwoLeftRowTwo(List<int> inputList)
        {
            List<int> output = new List<int>();
            for (int i = 0; i < inputList.Count; i++)
            {
                if (i < 2)
                    output.Add(inputList[i + 2]);
                else
                    output.Add(inputList[i - 2]);
            }
            return output;
        }

        public List<int> ShiftThreeLeftRowThree(List<int> inputList)
        {
            List<int> output = new List<int>();
            for (int i = 0; i < inputList.Count; i++)
            {
                if (i < 1)
                    output.Add(inputList[i + 3]);
                else
                    output.Add(inputList[i - 1]);
            }
            return output;
        }

        public List<int> MixColumnVerticallyOneRound(List<int> inputList)
        {
            List<int> output = new List<int>();

            // feed in to the columns
            List<int> col0 = new List<int>();
            List<int> col1 = new List<int>();
            List<int> col2 = new List<int>();
            List<int> col3 = new List<int>();
            for (int i = 0; i < inputList.Count; i++)
            {
                if (i == 0)
                    col0.Add(inputList[i]);
                if (i == 1)
                    col1.Add(inputList[i]);
                if (i == 2)
                    col2.Add(inputList[i]);
                if (i == 3)
                    col3.Add(inputList[i]);
                if (i == 4)
                    col0.Add(inputList[i]);
                if (i == 5)
                    col1.Add(inputList[i]);
                if (i == 6)
                    col2.Add(inputList[i]);
                if (i == 7)
                    col3.Add(inputList[i]);
                if (i == 8)
                    col0.Add(inputList[i]);
                if (i == 9)
                    col1.Add(inputList[i]);
                if (i == 10)
                    col2.Add(inputList[i]);
                if (i == 11)
                    col3.Add(inputList[i]);
                if (i == 12)
                    col0.Add(inputList[i]);
                if (i == 13)
                    col1.Add(inputList[i]);
                if (i == 14)
                    col2.Add(inputList[i]);
                if (i == 15)
                    col3.Add(inputList[i]);
            }

            // todo: delete later
            //Console.WriteLine("Columns before shifted:");
            //Console.Write("col0: ");
            //foreach (var a in col0)
            //    Console.Write(a);
            //Console.WriteLine();
            //Console.Write("col1: ");
            //foreach (var b in col1)
            //    Console.Write(b);
            //Console.WriteLine();
            //Console.Write("col2: ");
            //foreach (var c in col2)
            //    Console.Write(c);
            //Console.WriteLine();
            //Console.Write("col3: ");
            //foreach (var d in col3)
            //    Console.Write(d);

            output = col0;
            List<int> shiftedCol1 = ShiftOneDownColumnOne(col1);
            output.AddRange(shiftedCol1);
            List<int> shiftedCol2 = ShiftTwoDownColumnTwo(col2);
            output.AddRange(shiftedCol2);
            List<int> shiftedCol3 = ShiftThreeDownColumnThree(col3);
            output.AddRange(shiftedCol3);

            return output;
        }

        public List<int> ShiftOneDownColumnOne(List<int> inputList)
        {
            List<int> output = new List<int>();
            for (int i = 0; i < inputList.Count; i++)
            {
                if (i == 0)
                    output.Add(inputList[3]);
                else
                    output.Add(inputList[i - 1]);
            }
            return output;
        }

        public List<int> ShiftTwoDownColumnTwo(List<int> inputList)
        {
            List<int> output = new List<int>();
            for (int i = 0; i < inputList.Count; i++)
            {
                if (i < 2)
                    output.Add(inputList[i + 2]);
                else
                    output.Add(inputList[i - 2]);
            }
            return output;
        }

        public List<int> ShiftThreeDownColumnThree(List<int> inputList)
        {
            List<int> output = new List<int>();
            for (int i = 0; i < inputList.Count; i++)
            {
                if (i == 0)
                    output.Add(inputList[i + 3]);
                else if (i == 1)
                    output.Add(inputList[i + 1]);
                else if (i == 2)
                    output.Add(inputList[i - 1]);
                else
                    output.Add(inputList[i - 3]);
            }
            return output;
        }

        // Note: keeping it here just incase we need it
        public List<int> PermutationTwo(List<int> inputList)
        {
            List<int> output = new List<int>();
            output.Add(inputList[3]);   // 0>3
            output.Add(inputList[15]);  // 1>15
            output.Add(inputList[10]);  // 2>10
            output.Add(inputList[12]);  // 3>12
            output.Add(inputList[5]);   // 4>5
            output.Add(inputList[13]);  // 5>13
            output.Add(inputList[4]);   // 6>4
            output.Add(inputList[8]);   // 7>8
            output.Add(inputList[1]);   // 8>1
            output.Add(inputList[11]);  // 9>11
            output.Add(inputList[2]);   // 10>2
            output.Add(inputList[0]);   // 11>0
            output.Add(inputList[7]);   // 12>7
            output.Add(inputList[14]);  // 13>14
            output.Add(inputList[6]);   // 14>6
            output.Add(inputList[9]);   // 15>9
            return output;
        }
    }
}
