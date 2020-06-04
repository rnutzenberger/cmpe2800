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

namespace RNutzenbergerICA4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            btnOpen.Click += btnOpen_Click;
            Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<string> sourceStrings = new List<string>(
                new string[] { "Caballo", "Gato", "Perro", "Conejo", "Tortuga", "Cangrejo" });

            //select the strings where their ASCII sum is less than 600
            _lsbStats.Items.Add("------------Part A------------");
            foreach (string s in from n in sourceStrings where n.Sum(i => i) < 600 select n)
            {
                _lsbStats.Items.Add(s);
            }
            _lsbStats.Items.Add("");

            //using anonymous types, display the string and sum, unordered
            var animals = from n in sourceStrings
                         where n.Sum(i => i) < 600 
                         select new
                         {
                            Str = n,
                            Sum = n.Sum(i => i)
                         };
            //foreach through and display the contents
            foreach(var v in animals)
            {
                _lsbStats.Items.Add($"{{Str = {v.Str}, Sum = {v.Sum} }}");
            }

            _lsbStats.Items.Add("");
            //same as last, but now order by Sum descending
            var animals2 = from n in sourceStrings
                          where n.Sum(i => i) < 600
                          orderby n.Sum(i=>i) descending
                          select new
                          {
                              Str = n,
                              Sum = n.Sum(i => i)
                          };
            //foreach through and display the contents
            foreach (var v in animals2)
            {
                _lsbStats.Items.Add($"{{Str = {v.Str}, Sum = {v.Sum} }}");
            }

        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            _lsbStats.Items.Clear();
            LoadFile();
        }

        public void LoadFile()
        {

            //read out the text file into a string
            string filebits = File.ReadAllText(@"..\..\..\junk.txt");

            //Use a LINQ expressiom to split the string line by line and remove anywhere there is no line or has whitespace
            var temp = from f in filebits.Split(new[] { "\r\n" }, StringSplitOptions.None).ToList()
                       where f.Length > 0 && !String.IsNullOrWhiteSpace(f)
                       select f;

            //Use a LINQ expression to now group eahc line by the sum of the line, ordered by that sum
            //and create anonymous variables to access it easier
            var ordered = from t in temp
                          group t by t.Sum(i => i) into q
                          orderby q.Key 
                          select new
                          {
                              Sum = q.Key,
                              String = q.ToList()
                          };

            //Display to the console the lowest ASCII sum, and the ‘lowest’ contributing
            //string.
            _lsbStats.Items.Add($"Lowest ASCII Sum : {ordered.First().Sum}");
            _lsbStats.Items.Add($"Lowest String : {ordered.First().String.First()}");

            //Display the largest ASCII sum, and the ‘highest’ contributing string (characters in raw
            //and ascending order).
            _lsbStats.Items.Add($"Highest ASCII Sum : {ordered.Last().Sum}");
            _lsbStats.Items.Add($"Highest String : {ordered.OrderBy(x=>x.Sum).Last().String.Max()}/{new string(ordered.OrderBy(x => x.Sum).Last().String.Max().OrderBy(x=>x).ToArray())}");

            //Display the count of the largest collection in your dictionary(the largest number of
            //strings that contributed to the same ASCII sum).
            _lsbStats.Items.Add($"Biggest Collection Count: {ordered.Max(x=>x.String.Count)} - ASCII Sum: {ordered.OrderBy(x=>x.String.Count).Last().Sum}");

        }
    }
}
