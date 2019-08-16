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
                    encodeBtn.Content = "XOR The PlainText and Key (Shows In Left Box)";
                    break;
                case 1:
                    // XOR Plaintext + Key
                    XorTables();
                    AlignXorLetters();
                    currStep++;
                    encodeBtn.Content = "Substitute the bytes (Shift 1 to the right)";
                    break;
                case 2:
                    //ShiftLettersOneOver();
                    AlignPlainLetters();
                    encodeBtn.Content = "Shift The Rows";
                    break;
                case 3:
                    break;
                case 4:
                    break;
            }
        }

        private void SHiftLettersOneOver()
        {

        }

        private void XorTables()
        {
            var currTextPos = 0;
            for (var col = 0; col < 4; col++)
            {
                for (var row = 0; row < 4; row++)
                {
                    currXorNums[row, col] = (plainBinNums[currTextPos] - 48) ^ (keyBinNums[currTextPos] - 48) ;
                    currTextPos++;
                }
            }
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
            for (var col = 0; col < 4; col++)
            {
                for (var row = 0; row < 4; row++)
                {
                    currPlainNumbers[row, col] = plainBinNums[currTextPos] - 48;
                    currTextPos++;
                }
            }
        }

        private void PopulateFourByFourKey(int currTextPos)
        {
            for (var col = 0; col < 4; col++)
            {
                for (var row = 0; row < 4; row++)
                {
                    currKeyNumbers[row, col] = keyBinNums[currTextPos] - 48;
                    currTextPos++;
                }
            }
        }
        private void AlignPlainLetters()
        {
            // 1st column
            OneByOne.Text = Convert.ToString(currPlainNumbers[0, 0]);
            TwoByOne.Text = Convert.ToString(currPlainNumbers[1, 0]);
            ThreeByOne.Text = Convert.ToString(currPlainNumbers[2, 0]);
            FourByOne.Text = Convert.ToString(currPlainNumbers[3, 0]);
            // 2nd column
            OneByTwo.Text = Convert.ToString(currPlainNumbers[0, 1]);
            TwoByTwo.Text = Convert.ToString(currPlainNumbers[1, 1]);
            ThreeByTwo.Text = Convert.ToString(currPlainNumbers[2, 1]);
            FourByTwo.Text = Convert.ToString(currPlainNumbers[3, 1]);
            // 3rd column
            OneByThree.Text = Convert.ToString(currPlainNumbers[0, 2]);
            TwoByThree.Text = Convert.ToString(currPlainNumbers[1, 2]);
            ThreeByThree.Text = Convert.ToString(currPlainNumbers[2, 2]);
            FourByThree.Text = Convert.ToString(currPlainNumbers[3, 2]);
            // 4th column
            OneByFour.Text = Convert.ToString(currPlainNumbers[0, 3]);
            TwoByFour.Text = Convert.ToString(currPlainNumbers[1, 3]);
            ThreeByFour.Text = Convert.ToString(currPlainNumbers[2, 3]);
            FourByFour.Text = Convert.ToString(currPlainNumbers[3, 3]);
        }

        private void AlignXorLetters()
        {
            // 1st column
            OneByOne.Text = Convert.ToString(currXorNums[0, 0]);
            TwoByOne.Text = Convert.ToString(currXorNums[1, 0]);
            ThreeByOne.Text = Convert.ToString(currXorNums[2, 0]);
            FourByOne.Text = Convert.ToString(currXorNums[3, 0]);
            // 2nd column
            OneByTwo.Text = Convert.ToString(currXorNums[0, 1]);
            TwoByTwo.Text = Convert.ToString(currXorNums[1, 1]);
            ThreeByTwo.Text = Convert.ToString(currXorNums[2, 1]);
            FourByTwo.Text = Convert.ToString(currXorNums[3, 1]);
            // 3rd column
            OneByThree.Text = Convert.ToString(currXorNums[0, 2]);
            TwoByThree.Text = Convert.ToString(currXorNums[1, 2]);
            ThreeByThree.Text = Convert.ToString(currXorNums[2, 2]);
            FourByThree.Text = Convert.ToString(currXorNums[3, 2]);
            // 4th column
            OneByFour.Text = Convert.ToString(currXorNums[0, 3]);
            TwoByFour.Text = Convert.ToString(currXorNums[1, 3]);
            ThreeByFour.Text = Convert.ToString(currXorNums[2, 3]);
            FourByFour.Text = Convert.ToString(currXorNums[3, 3]);
        }

        private void AlignKeyLetters()
        {
            // 1st column
            OneByOneK.Text = Convert.ToString(currKeyNumbers[0, 0]);
            TwoByOneK.Text = Convert.ToString(currKeyNumbers[1, 0]);
            ThreeByOneK.Text = Convert.ToString(currKeyNumbers[2, 0]);
            FourByOneK.Text = Convert.ToString(currKeyNumbers[3, 0]);
            // 2nd column
            OneByTwoK.Text = Convert.ToString(currKeyNumbers[0, 1]);
            TwoByTwoK.Text = Convert.ToString(currKeyNumbers[1, 1]);
            ThreeByTwoK.Text = Convert.ToString(currKeyNumbers[2, 1]);
            FourByTwoK.Text = Convert.ToString(currKeyNumbers[3, 1]);
            // 3rd column
            OneByThreeK.Text = Convert.ToString(currKeyNumbers[0, 2]);
            TwoByThreeK.Text = Convert.ToString(currKeyNumbers[1, 2]);
            ThreeByThreeK.Text = Convert.ToString(currKeyNumbers[2, 2]);
            FourByThreeK.Text = Convert.ToString(currKeyNumbers[3, 2]);
            // 4th column
            OneByFourK.Text = Convert.ToString(currKeyNumbers[0, 3]);
            TwoByFourK.Text = Convert.ToString(currKeyNumbers[1, 3]);
            ThreeByFourK.Text = Convert.ToString(currKeyNumbers[2, 3]);
            FourByFourK.Text = Convert.ToString(currKeyNumbers[3, 3]);
        }
    }
}
