using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace RNutzenbergerICA3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            _btnOpen.Click += _btnOpen_Click;
        }

        private void _btnOpen_Click(object sender, EventArgs e)
        {
            LoadFile();
        }

        public void LoadFile()
        {
            //Download the letters file found on Moodle in the appropriate section(your instructor will
            //provide additional details).
            //Use an appropriate method to run - time locate the file(OpenFileDialog).So, !ConsoleApp.
            //Using a StreamReader and ReadToEnd, load the entire contents of the file into a string.
            var ofd = new OpenFileDialog();
            if(ofd.ShowDialog() != DialogResult.OK)
            {
                return;   
            }
            var sr = new StreamReader(ofd.FileName);
            var sInfo = sr.ReadToEnd();
            sr.Close();

            //Use Split, an inline character array definition, and ToList, to split the contents of the string
            //into a string array that is split on cr / new line characters(\r \n).
            var temp = sInfo.Split(new[] { "\r\n" },StringSplitOptions.None).ToList();

            //Use a single lambda expression with RemoveAll to purge any empty lines, or lines that contain
            //only whitespace.
            temp.RemoveAll((x) => String.IsNullOrWhiteSpace(x) || x.Contains(" "));

            //In a single line, produce a dictionary that is keyed by each line of characters from the file. The
            //value associated with the key will be the ASCII sum of the characters in the line. You should be
            //able to do this with a single ToDictionary call with a few lambdas
            var dict = temp.Distinct().ToDictionary(key => key, val => val.Sum(i => i));

            //Build and populate a new dictionary that is keyed by the ASCII sum from the previous step.The
            //value will be a List of all the lines that sum to the key – this is not a single line of code!
            var myDict = dict.Values.Distinct().ToDictionary(c => c, k => dict.Keys.Where(sum => dict[sum] == k).ToList());

            //Use OrderBy and ToList on the dictionary from the previous step to produce an ordered
            //enumerable.Display to the console the lowest ASCII sum, and the ‘lowest’ contributing
            //string.
            var orderedDict = dict.OrderBy(x => x.Value).ToList();
            _lsbStats.Items.Add($"Lowest ASCII Sum: {orderedDict.First().Value}");
            _lsbStats.Items.Add($"Lowest String: {orderedDict.First().Key}");

            //Display the largest ASCII sum, and the ‘highest’ contributing string (characters in raw
            //and ascending order). The string constructor, Max, OrderBy, and ToArray may be of use
            //here!
            _lsbStats.Items.Add($"Highest ASCII Sum: {orderedDict.Last().Value}");
            _lsbStats.Items.Add($"Highest String: {myDict.OrderBy(x=>x.Key).Last().Value.Max()}/{new string(myDict.OrderBy(x=>x.Key).Last().Value.Max().OrderBy(c=>c).ToArray())}");

            //Display the count of the largest collection in your dictionary(the largest number of
            //strings that contributed to the same ASCII sum).
            _lsbStats.Items.Add($"Biggest Collection Count: {myDict.Values.Max(key=>key.Count)} - ASCII Sum: {myDict.OrderBy(x=> x.Value.Count).Last().Key}");

        }
    }
}
