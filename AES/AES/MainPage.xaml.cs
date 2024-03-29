﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
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
        private string permutatedBinNums;
        public MainPage()
        {
            this.InitializeComponent();

        }

        private async void EncodeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (plainTextInput.Text == "" || plainTextInput.Text.Length > 4 && keyTextInput.Text.Length > 4 || keyTextInput.Text == "")
            {
                await new MessageDialog("Plain text and a key must be filled out, and no longer than 4 digits.", "Error").ShowAsync();
                return;
            }
            var currTextPos = 0;
            decodeBtn.IsEnabled = false;
            switch (currStep)
            {
                case 0:
                    // Setup the key + plaintext
                    PlainTextRectBox.Stroke = new SolidColorBrush(Colors.Red);
                    KeyRectBox.Stroke = new SolidColorBrush(Colors.Red);
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
                    PermRectBox.Stroke = new SolidColorBrush(Colors.Red);
                    var permedKey = Encryption.PermutateKeyOnEncryption(keyBinNums);
                    currPermutatedKeyNums = StringToMatrix(permedKey);
                    AlignPermLetters();
                    encodeBtn.Content = "XOR The PlainText and Key (Shows In Left Box)";
                    currStep++;
                    break;
                case 2:
                    // XOR Plaintext + Key
                    PermRectBox.Stroke = new SolidColorBrush(Colors.Aqua);
                    xorLabel.Text = "⊕";
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
                    PermRectBox.Stroke = new SolidColorBrush(Colors.Red);
                    KeyRectBox.Stroke = new SolidColorBrush(Colors.Aqua);
                    MixColumns();
                    AlignPlainLetters();
                    encodeBtn.Content = "XOR with Permutated Key";
                    currStep++;
                    break;
                case 6:
                    KeyRectBox.Stroke = new SolidColorBrush(Colors.Aqua);
                    PermRectBox.Stroke = new SolidColorBrush(Colors.Aqua);
                    PlainTextRectBox.Stroke = new SolidColorBrush(Colors.Aqua);
                    XorWithPermutatedKey();
                    AlignPlainLetters();
                    currStep = 0;
                    encodeBtn.Content = "Finished Encrypting";
                    encodeBtn.IsEnabled = false;
                    decodeBtn.IsEnabled = true;
                    decodeBtn.Content = "Decrypt The Encrypted Text";
                    xorLabel.Text = "";
                    break;
            }
        }

        private void XorWithPermutatedKey()
        {
            var xoredPlainTxt = Encryption.XorEvaluationOnEncryption(MatrixToString(currPlainNumbers),
                MatrixToString(currPermutatedKeyNums));
            currPlainNumbers = StringToMatrix(xoredPlainTxt);

            encryptedResult.Text = Binary2Hex(xoredPlainTxt);
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
            hexVal = Convert.ToString(Convert.ToInt32(binary, 2), 16);
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

        private async void DecodeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (decryptTextInput.Text == "" || decryptTextInput.Text.Length > 4 && keyTextInput.Text.Length > 4 || keyTextInput.Text == "")
            {
                await new MessageDialog("Decrypt text and a key must be filled out, and no longer than 4 digits.", "Error").ShowAsync();
                return;
            }

            encodeBtn.IsEnabled = false;

            switch (currStep)
            {
                case 0:
                    KeyRectBox.Stroke = new SolidColorBrush(Colors.Aqua);
                    PermRectBox.Stroke = new SolidColorBrush(Colors.Red);
                    PlainTextRectBox.Stroke = new SolidColorBrush(Colors.Red);
                    StartDecrypt();
                    currStep++;
                    decodeBtn.Content = "XOR Permutated Key With Text";
                    break;
                case 1:
                    xorLabel.Text = "⊕";
                    KeyRectBox.Stroke = new SolidColorBrush(Colors.Aqua);
                    PermRectBox.Stroke = new SolidColorBrush(Colors.Red);
                    PlainTextRectBox.Stroke = new SolidColorBrush(Colors.Red);
                    XorDecodeTable();
                    AlignPlainLetters();
                    currStep++;
                    decodeBtn.Content = "Now Re-Mix (Shift Up)";
                    break;
                case 2:
                    KeyRectBox.Stroke = new SolidColorBrush(Colors.Aqua);
                    PermRectBox.Stroke = new SolidColorBrush(Colors.Aqua);
                    PlainTextRectBox.Stroke = new SolidColorBrush(Colors.Red);
                    MixColumnUp();
                    AlignPlainLetters();
                    currStep++;
                    decodeBtn.Content = "Shift rows right";
                    break;
                case 3:
                    ShiftRowsDecode();
                    AlignPlainLetters();
                    currStep++;
                    decodeBtn.Content = "Substitute Bytes (Shift 1 left)";
                    break;
                case 4:
                    KeyRectBox.Stroke = new SolidColorBrush(Colors.Red);
                    PermRectBox.Stroke = new SolidColorBrush(Colors.Aqua);
                    PlainTextRectBox.Stroke = new SolidColorBrush(Colors.Red);
                    DecodeSubstituteBytes();
                    AlignPlainLetters();
                    currStep++;
                    decodeBtn.Content = "XOR with Original Key";
                    break;
                case 5:
                    KeyRectBox.Stroke = new SolidColorBrush(Colors.Aqua);
                    PermRectBox.Stroke = new SolidColorBrush(Colors.Aqua);
                    PlainTextRectBox.Stroke = new SolidColorBrush(Colors.Aqua);
                    DecodeXorWithOrigKey();
                    currStep = 0;
                    AlignPlainLetters();
                    decryptedResult.Text = Binary2Hex(MatrixToString(currPlainNumbers));
                    decodeBtn.Content = "Finished Decoding";
                    decodeBtn.IsEnabled = false;
                    encodeBtn.IsEnabled = true;
                    encodeBtn.Content = "Encrypt and Populate Matrixes In Binary Format";
                    break;
            }
        }

        private void DecodeXorWithOrigKey()
        {
            var result = Decryption.XorStepSixWithOriginalKeyOnDecryption(MatrixToString(currPlainNumbers), keyBinNums);
            currPlainNumbers = StringToMatrix(result);
        }

        private void DecodeSubstituteBytes()
        {
            var result = Decryption.ShiftingLeftOneBitOnDecryption(MatrixToString(currPlainNumbers));
            currPlainNumbers = StringToMatrix(result);
        }

        private void ShiftRowsDecode()
        {
            var test = Decryption.ShiftingRowsOnDecryption(MatrixToString(currPlainNumbers));
            currPlainNumbers = StringToMatrix(test);

        }

        private void MixColumnUp()
        {
            var result = Decryption.MixColumnVerticallyUpwardOnDecryption(MatrixToString(currPlainNumbers));
            currPlainNumbers = StringToMatrix(result);
        }
        private void XorDecodeTable()
        {
            var resultString = Decryption.XorEvaluationOnDecryption(plainBinNums, permutatedBinNums);
            currPlainNumbers = StringToMatrix(resultString);
        }

        private void StartDecrypt()
        {
            plainBinNums = Hex2Binary(decryptTextInput.Text);
            keyBinNums = Hex2Binary(keyTextInput.Text);
            permutatedBinNums = Decryption.PermutateKeyOnDecryption(keyBinNums);

            currPlainNumbers = StringToMatrix(plainBinNums);
            AlignPlainLetters();

            currKeyNumbers = StringToMatrix(keyBinNums);
            AlignKeyLetters();

            currPermutatedKeyNums = StringToMatrix(permutatedBinNums);
            AlignPermLetters();
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            KeyRectBox.Stroke = new SolidColorBrush(Colors.Aqua);
            PermRectBox.Stroke = new SolidColorBrush(Colors.Aqua);
            PlainTextRectBox.Stroke = new SolidColorBrush(Colors.Aqua);
            xorLabel.Text = "";
            currStep = 0;
            encodeBtn.Content = "Encrypt and Populate Matrixes In Binary Format";
            decodeBtn.Content = "Decrypt The Encrypted Text";
            encodeBtn.IsEnabled = true;
            decodeBtn.IsEnabled = true;
            plainTextInput.Text = "";
            keyTextInput.Text = "";
            decryptTextInput.Text = "";
            encryptedResult.Text = "Empty";
            decryptedResult.Text = "Empty";

            // 1st column
            OneByOnePK.Text = "X";
            OneByTwoPK.Text = "X";
            OneByThreePK.Text = "X";
            OneByFourPK.Text = "X";
            // 2nd column
            TwoByOnePK.Text = "X";
            TwoByTwoPK.Text = "X";
            TwoByThreePK.Text = "X";
            TwoByFourPK.Text = "X";
            // 3rd column
            ThreeByOnePK.Text = "X";
            ThreeByTwoPK.Text = "X";
            ThreeByThreePK.Text = "X";
            ThreeByFourPK.Text = "X";
            // 4th column
            FourByOnePK.Text = "X";
            FourByTwoPK.Text = "X";
            FourByThreePK.Text = "X";
            FourByFourPK.Text = "X";

            // 1st column
            OneByOneK.Text = "X";
            OneByTwoK.Text = "X";
            OneByThreeK.Text = "X";
            OneByFourK.Text = "X";
            // 2nd column
            TwoByOneK.Text = "X";
            TwoByTwoK.Text = "X";
            TwoByThreeK.Text = "X";
            TwoByFourK.Text = "X";
            // 3rd column
            ThreeByOneK.Text = "X";
            ThreeByTwoK.Text = "X";
            ThreeByThreeK.Text = "X";
            ThreeByFourK.Text = "X";
            // 4th column
            FourByOneK.Text = "X";
            FourByTwoK.Text = "X";
            FourByThreeK.Text = "X";
            FourByFourK.Text = "X";

            // 1st column
            OneByOne.Text = "X";
            OneByTwo.Text = "X";
            OneByThree.Text = "X";
            OneByFour.Text = "X";
            // 2nd column
            TwoByOne.Text = "X";
            TwoByTwo.Text = "X";
            TwoByThree.Text = "X";
            TwoByFour.Text = "X";
            // 3rd column
            ThreeByOne.Text = "X";
            ThreeByTwo.Text = "X";
            ThreeByThree.Text = "X";
            ThreeByFour.Text = "X";
            // 4th column
            FourByOne.Text = "X";
            FourByTwo.Text = "X";
            FourByThree.Text = "X";
            FourByFour.Text = "X";

        }
    }
}
