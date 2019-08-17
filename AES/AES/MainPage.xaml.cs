using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AES.AesLib;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AES
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private int currStep = 0;

        private int[,] currPlainNumbers = new int[4, 4];
        private int[,] currKeyNumbers = new int[4, 4];
        private int[,] currPermutatedKeyNums = new int[4, 4];
        private int[,] currXorNums = new int[4, 4];
        private string plainBinNums;
        private string keyBinNums;
        public MainPage()
        {
            this.InitializeComponent();

        }

        private void EncodeBtn_Click(object sender, RoutedEventArgs e)
        {
            var currTextPos = 0;

            switch (currStep)
            {
                case 0:
                    // Setup the key + plaintext
                    plainBinNums = Hex2Binary(plainTextInput.Text);
                    keyBinNums = Hex2Binary(keyTextInput.Text);
                    PopulateFourByFourPlain(currTextPos);
                    PopulateFourByFourKey(currTextPos);
                    AlignPlainLetters();
                    AlignKeyLetters();
                    currStep++;
                    encodeBtn.Content = "Create a permuted key of original key";
                    break;
                case 1:
                    var permedKey = Encryption.PermutateKeyOnEncryption(keyBinNums);
                    currPermutatedKeyNums = StringToMatrix(permedKey);
                    AlignPermLetters();
                    encodeBtn.Content = "XOR The PlainText and Key (Shows In Left Box)";
                    currStep++;
                    break;
                case 2:
                    // XOR Plaintext + Key
                    XorTables();
                    AlignXorLetters();
                    currPlainNumbers = currXorNums;
                    currStep++;
                    encodeBtn.Content = "Substitute the bytes (Shift 1 to the right)";
                    break;
                case 3:
                    ShiftLettersOneOver();
                    AlignPlainLetters();
                    encodeBtn.Content = "Shift The Rows";
                    currStep++;
                    break;
                case 4:
                    ShiftRows();
                    AlignPlainLetters();
                    encodeBtn.Content = "Mix Columns";
                    currStep++;
                    break;
                case 5:
                    MixColumns();
                    AlignPlainLetters();
                    encodeBtn.Content = "XOR with Permutated Key";
                    currStep++;
                    break;
                case 6:
                    XorWithPermutatedKey();
                    AlignPlainLetters();
                    currStep++;
                    break;
            }
        }

        private void XorWithPermutatedKey()
        {
            var xoredPlainTxt = Encryption.XorEvaluationOnEncryption(MatrixToString(currPlainNumbers),
                MatrixToString(currPermutatedKeyNums));
            currPlainNumbers = StringToMatrix(xoredPlainTxt);
        }

        private void MixColumns()
        {
            var newStr = Encryption.MixColumnVerticallyDownOnEncryption(MatrixToString(currPlainNumbers));
            currPlainNumbers = StringToMatrix(newStr);
        }

        private void ShiftRows()
        {
            var newStr = Encryption.ShiftingRowsToLeftOnEncryption(MatrixToString(currPlainNumbers));
            currPlainNumbers = StringToMatrix(newStr);
        }

        private void ShiftLettersOneOver()
        {
            var newStr = Encryption.ShiftOneRightOnEncryption(MatrixToString(currPlainNumbers));
            currPlainNumbers = StringToMatrix(newStr);
        }



        private void XorTables()
        {
            var currTextPos = 0;
            for (var row = 0; row < 4; row++)
            {
                for (var col = 0; col < 4; col++)
                {
                    currXorNums[row, col] = (plainBinNums[currTextPos] - 48) ^ (keyBinNums[currTextPos] - 48) ;
                    currTextPos++;
                }
            }
        }

        private int[,] StringToMatrix(string inputString)
        {
            var currTextPos = 0;
            int[,] convertedMatrix = new int[4, 4];
            for (var row = 0; row < 4; row++)
            {
                for (var col = 0; col < 4; col++)
                {
                    convertedMatrix[row, col] = inputString[currTextPos] - 48;
                    currTextPos++;
                }
            }

            return convertedMatrix;
        }

        private string MatrixToString(int[,] matrix)
        {
            return string.Join("", matrix.OfType<int>()
                .Select((value, index) => new {value, index})
                .GroupBy(x => x.index / matrix.GetLength(1))
                .Select(x => $"{string.Join("", x.Select(y => y.value))}"));
        }

        private string Binary2Hex(string binary)
        {
            string hexVal = "";
            hexVal = Convert.ToString(Convert.ToInt32(binary, 16), 2);
            return hexVal;
        }
        private string Hex2Binary(string hexvalue)
        {
            var binaryval = Convert.ToString(Convert.ToInt32(hexvalue, 16), 2);
            if (binaryval.Length < 16)
            {
                binaryval = binaryval.PadLeft(16, '0');
                binaryval = binaryval.ToString();
            }
            return binaryval;
        }

        private void PopulateFourByFourPlain(int currTextPos)
        {
            for (var row = 0; row < 4; row++)
            {
                for (var col = 0; col < 4; col++)
                {
                    currPlainNumbers[row, col] = plainBinNums[currTextPos] - 48;
                    currTextPos++;
                }
            }
        }

        private void PopulateFourByFourKey(int currTextPos)
        {
            for (var row = 0; row < 4; row++)
            {
                for (var col = 0; col < 4; col++)
                {
                    currKeyNumbers[row, col] = keyBinNums[currTextPos] - 48;
                    currTextPos++;
                }
            }
        }

        private void AlignPermLetters()
        {
            // 1st column
            OneByOnePK.Text = Convert.ToString(currPermutatedKeyNums[0, 0]);
            OneByTwoPK.Text = Convert.ToString(currPermutatedKeyNums[0, 1]);
            OneByThreePK.Text = Convert.ToString(currPermutatedKeyNums[0, 2]);
            OneByFourPK.Text = Convert.ToString(currPermutatedKeyNums[0, 3]);
            // 2nd column
            TwoByOnePK.Text = Convert.ToString(currPermutatedKeyNums[1, 0]);
            TwoByTwoPK.Text = Convert.ToString(currPermutatedKeyNums[1, 1]);
            TwoByThreePK.Text = Convert.ToString(currPermutatedKeyNums[1, 2]);
            TwoByFourPK.Text = Convert.ToString(currPermutatedKeyNums[1, 3]);
            // 3rd column
            ThreeByOnePK.Text = Convert.ToString(currPermutatedKeyNums[2, 0]);
            ThreeByTwoPK.Text = Convert.ToString(currPermutatedKeyNums[2, 1]);
            ThreeByThreePK.Text = Convert.ToString(currPermutatedKeyNums[2, 2]);
            ThreeByFourPK.Text = Convert.ToString(currPermutatedKeyNums[2, 3]);
            // 4th column
            FourByOnePK.Text = Convert.ToString(currPermutatedKeyNums[3, 0]);
            FourByTwoPK.Text = Convert.ToString(currPermutatedKeyNums[3, 1]);
            FourByThreePK.Text = Convert.ToString(currPermutatedKeyNums[3, 2]);
            FourByFourPK.Text = Convert.ToString(currPermutatedKeyNums[3, 3]);
        }

        private void AlignPlainLetters()
        {
            // 1st column
            OneByOne.Text = Convert.ToString(currPlainNumbers[0, 0]);
            OneByTwo.Text = Convert.ToString(currPlainNumbers[0, 1]);
            OneByThree.Text = Convert.ToString(currPlainNumbers[0, 2]);
            OneByFour.Text = Convert.ToString(currPlainNumbers[0, 3]);
            // 2nd column
            TwoByOne.Text = Convert.ToString(currPlainNumbers[1, 0]);
            TwoByTwo.Text = Convert.ToString(currPlainNumbers[1, 1]);
            TwoByThree.Text = Convert.ToString(currPlainNumbers[1, 2]);
            TwoByFour.Text = Convert.ToString(currPlainNumbers[1, 3]);
            // 3rd column
            ThreeByOne.Text = Convert.ToString(currPlainNumbers[2, 0]);
            ThreeByTwo.Text = Convert.ToString(currPlainNumbers[2, 1]);
            ThreeByThree.Text = Convert.ToString(currPlainNumbers[2, 2]);
            ThreeByFour.Text = Convert.ToString(currPlainNumbers[2, 3]);
            // 4th column
            FourByOne.Text = Convert.ToString(currPlainNumbers[3, 0]);
            FourByTwo.Text = Convert.ToString(currPlainNumbers[3, 1]);
            FourByThree.Text = Convert.ToString(currPlainNumbers[3, 2]);
            FourByFour.Text = Convert.ToString(currPlainNumbers[3, 3]);
        }

        private void AlignXorLetters()
        {
            // 1st column
            OneByOne.Text = Convert.ToString(currXorNums[0, 0]);
            OneByTwo.Text = Convert.ToString(currXorNums[0, 1]);
            OneByThree.Text = Convert.ToString(currXorNums[0, 2]);
            OneByFour.Text = Convert.ToString(currXorNums[0, 3]);
            // 2nd column
            TwoByOne.Text = Convert.ToString(currXorNums[1, 0]);
            TwoByTwo.Text = Convert.ToString(currXorNums[1, 1]);
            TwoByThree.Text = Convert.ToString(currXorNums[1, 2]);
            TwoByFour.Text = Convert.ToString(currXorNums[1, 3]);
            // 3rd column
            ThreeByOne.Text = Convert.ToString(currXorNums[2, 0]);
            ThreeByTwo.Text = Convert.ToString(currXorNums[2, 1]);
            ThreeByThree.Text = Convert.ToString(currXorNums[2, 2]);
            ThreeByFour.Text = Convert.ToString(currXorNums[2, 3]);
            // 4th column
            FourByOne.Text = Convert.ToString(currXorNums[3, 0]);
            FourByTwo.Text = Convert.ToString(currXorNums[3, 1]);
            FourByThree.Text = Convert.ToString(currXorNums[3, 2]);
            FourByFour.Text = Convert.ToString(currXorNums[3, 3]);
        }

        private void AlignKeyLetters()
        {
            // 1st column
            OneByOneK.Text = Convert.ToString(currKeyNumbers[0, 0]);
            OneByTwoK.Text = Convert.ToString(currKeyNumbers[0, 1]);
            OneByThreeK.Text = Convert.ToString(currKeyNumbers[0, 2]);
            OneByFourK.Text = Convert.ToString(currKeyNumbers[0, 3]);
            // 2nd column
            TwoByOneK.Text = Convert.ToString(currKeyNumbers[1, 0]);
            TwoByTwoK.Text = Convert.ToString(currKeyNumbers[1, 1]);
            TwoByThreeK.Text = Convert.ToString(currKeyNumbers[1, 2]);
            TwoByFourK.Text = Convert.ToString(currKeyNumbers[1, 3]);
            // 3rd column
            ThreeByOneK.Text = Convert.ToString(currKeyNumbers[2, 0]);
            ThreeByTwoK.Text = Convert.ToString(currKeyNumbers[2, 1]);
            ThreeByThreeK.Text = Convert.ToString(currKeyNumbers[2, 2]);
            ThreeByFourK.Text = Convert.ToString(currKeyNumbers[2, 3]);
            // 4th column
            FourByOneK.Text = Convert.ToString(currKeyNumbers[3, 0]);
            FourByTwoK.Text = Convert.ToString(currKeyNumbers[3, 1]);
            FourByThreeK.Text = Convert.ToString(currKeyNumbers[3, 2]);
            FourByFourK.Text = Convert.ToString(currKeyNumbers[3, 3]);
        }
    }
}
