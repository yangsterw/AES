using System;
using System.Collections.Generic;
using System.Text;

namespace AES.AesLib
{
    public class LibrarySAES
    {
        public LibrarySAES() { }

        public List<int> ConvertStringOfNumbersToBinary(string inputText)
        {
            List<int> output = new List<int>();
            string binary = string.Empty;
            int valueInt = 0;
            int temp = 0;
            for (int i = 0; i < inputText.Length; i++) {
                if (IsLetter(inputText[i])) {
                    binary = inputText[i].ToString().ToUpper();
                    switch (binary) {
                        case "A":
                            valueInt = 10;
                            break;
                        case "B":
                            valueInt = 11;
                            break;
                        case "C":
                            valueInt = 12;
                            break;
                        case "D":
                            valueInt = 13;
                            break;
                        case "E":
                            valueInt = 14;
                            break;
                        case "F":
                            valueInt = 15;
                            break;
                    }
                } else {  valueInt = (int)Char.GetNumericValue(inputText[i]); }

                binary = Convert.ToString(valueInt, 2);
                if (binary.Length != 4) {
                    string newBinary = binary.PadLeft(4, '0');
                    foreach (var s in newBinary) {
                        temp = (int)Char.GetNumericValue(s);
                        output.Add(temp);
                    }
                } else {
                    foreach (var s in binary) {
                        temp = (int)Char.GetNumericValue(s);
                        output.Add(temp);
                    }
                }
            }
            return output;
        }

        bool IsLetter(char s) { return (s >= 'A' && s <= 'Z') || (s >= 'a' && s <= 'z'); }

        public List<string> ConvertBinaryToHexString(List<int> inputList)
        {
            List<string> output = new List<string>();
            StringBuilder binStr = new StringBuilder();
            for (int i = 0; i < inputList.Count; i++) {
                binStr.Append(inputList[i].ToString());
                if (binStr.Length == 4) {
                    string tempVal = Convert.ToUInt16(binStr.ToString(), 2).ToString();
                    int tempValInt = Convert.ToInt32(tempVal);
                    if (tempValInt > 9)  {
                        switch (tempValInt) {
                            case 10:
                                tempVal = "A";
                                break;
                            case 11:
                                tempVal = "B";
                                break;
                            case 12:
                                tempVal = "C";
                                break;
                            case 13:
                                tempVal = "D";
                                break;
                            case 14:
                                tempVal = "E";
                                break;
                            case 15:
                                tempVal = "F";
                                break;
                        }
                    }
                    output.Add(tempVal);
                    binStr.Clear();
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
            for (int i = 0; i < messageBinary.Count; i++) {
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

        public List<int> PermutationBackward(List<int> inputList)
        {
            List<int> output = new List<int>();
            output.Add(inputList[2]);   // 0 < 2
            output.Add(inputList[12]);  // 1 < 12 
            output.Add(inputList[6]);   // 2 < 6
            output.Add(inputList[15]);  // 3 < 15
            output.Add(inputList[10]);  // 4 < 10
            output.Add(inputList[1]);   // 5 < 1
            output.Add(inputList[11]);  // 6 < 11
            output.Add(inputList[4]);   // 7 < 4
            output.Add(inputList[9]);   // 8 < 9
            output.Add(inputList[14]);  // 9 < 14
            output.Add(inputList[8]);   // 10 < 8
            output.Add(inputList[3]);   // 11 < 3
            output.Add(inputList[5]);   // 12 < 5
            output.Add(inputList[7]);   // 13 < 7
            output.Add(inputList[0]);   // 14 < 0
            output.Add(inputList[13]);  // 15 < 13
            return output;
        }

        public List<int> MixColumnVerticallyUpwardOneRound(List<int> inputList)
        {
            List<int> output = new List<int>();
            List<int> col0 = new List<int>();
            List<int> col1 = new List<int>();
            List<int> col2 = new List<int>();
            List<int> col3 = new List<int>();
            for (int i = 0; i < inputList.Count; i++)
            {
                if (i < 4)
                    col0.Add(inputList[i]);
                else if (i > 3 && i < 8)
                    col1.Add(inputList[i]);
                else if (i > 7 && i < 12)
                    col2.Add(inputList[i]);
                else
                    col3.Add(inputList[i]);
            }

            // col0 given no shift!
            List<int> shiftedCol1 = ShiftOneUpColumnOne(col1);
            List<int> shiftedCol2 = ShiftTwoUpColumnTwo(col2);
            List<int> shiftedCol3 = ShiftThreeUpColumnThree(col3);

            //transposed into rows
            List<int> row0 = new List<int>();
            List<int> row1 = new List<int>();
            List<int> row2 = new List<int>();
            List<int> row3 = new List<int>();
            for (int i = 0; i < inputList.Count; i++)
            {
                if (i == 0)
                    row0.Add(col0[0]);
                if (i == 1)
                    row0.Add(shiftedCol1[0]);
                if (i == 2)
                    row0.Add(shiftedCol2[0]);
                if (i == 3)
                    row0.Add(shiftedCol3[0]);
                if (i == 4)
                    row1.Add(col0[1]);
                if (i == 5)
                    row1.Add(shiftedCol1[1]);
                if (i == 6)
                    row1.Add(shiftedCol2[1]);
                if (i == 7)
                    row1.Add(shiftedCol3[1]);
                if (i == 8)
                    row2.Add(col0[2]);
                if (i == 9)
                    row2.Add(shiftedCol1[2]);
                if (i == 10)
                    row2.Add(shiftedCol2[2]);
                if (i == 11)
                    row2.Add(shiftedCol3[2]);
                if (i == 12)
                    row3.Add(col0[3]);
                if (i == 13)
                    row3.Add(shiftedCol1[3]);
                if (i == 14)
                    row3.Add(shiftedCol2[3]);
                if (i == 15)
                    row3.Add(shiftedCol3[3]);
            }
            output = row0;
            output.AddRange(row1);
            output.AddRange(row2);
            output.AddRange(row3);

            return output;
        }

        public List<int> ShiftOneUpColumnOne(List<int> inputList)
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

        public List<int> ShiftTwoUpColumnTwo(List<int> inputList)
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

        public List<int> ShiftThreeUpColumnThree(List<int> inputList)
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

        public List<int> ShiftRowsRightSingleRound(List<int> inputList)
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
            List<int> shiftedRow1 = ShiftOneRightRowOne(row1);
            output.AddRange(shiftedRow1);
            List<int> shiftedRow2 = ShiftTwoRightRowTwo(row2);
            output.AddRange(shiftedRow2);
            List<int> shiftedRow3 = ShiftThreeRightRowThree(row3);
            output.AddRange(shiftedRow3);

            return output;
        }

        public List<int> ShiftOneRightRowOne(List<int> inputList)
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

        public List<int> ShiftTwoRightRowTwo(List<int> inputList)
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

        public List<int> ShiftThreeRightRowThree(List<int> inputList)
        {
            List<int> output = new List<int>();
            for (int i = 0; i < inputList.Count; i++)
            {
                if (i < 3)
                    output.Add(inputList[i + 1]);
                else
                    output.Add(inputList[0]);
            }
            return output;
        }

        public List<int> ShiftOneLeft(List<int> inputList)
        {
            List<int> output = new List<int>();
            for (int i = 0; i < inputList.Count; i++)
            {
                if (i == 15)
                    output.Add(inputList[0]);
                else
                    output.Add(inputList[i + 1]);
            }
            return output;
        }

    }
}
