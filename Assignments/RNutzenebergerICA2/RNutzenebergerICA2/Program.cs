using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GDIDrawer;
using RNUtility;
namespace RNutzenebergerICA2
{
    class Program
    {
        static void Main(string[] args)
        {
            //////////////////////////////////////////////////////
            //Testing Genrange with no parameters
            //////////////////////////////////////////////////////
            Console.WriteLine("--------------------------------------------------------\nTesting GenRange(No Parameters)....\n--------------------------------------------------------");
            foreach (int i in Utility.GenRange())
            {
                //spit out the returned value
                Console.Write($"[{i}] ");
            }
            Console.WriteLine("\n\n");

            //////////////////////////////////////////////////////
            //Testing Genrange with Upper bounds
            //////////////////////////////////////////////////////
            Console.WriteLine("--------------------------------------------------------\nTesting GenRange(Upper Bound)....\n--------------------------------------------------------");
            foreach (int i in Utility.GenRange(12))
            {
                Console.Write($"[{i}] ");
            }
            Console.WriteLine("\n\n");

            //////////////////////////////////////////////////////
            //Testing Genrange with upper and lower bounds
            //////////////////////////////////////////////////////
            Console.WriteLine("--------------------------------------------------------\nTesting GenRange(Upper and Lower Bound)....\n--------------------------------------------------------");
            foreach (int i in Utility.GenRange(27,140))
            {
                Console.Write($"[{i}] ");
            }
            Console.WriteLine("\n\n");

            //////////////////////////////////////////////////////
            //Testing Genrange to throw and exception
            //////////////////////////////////////////////////////
            Console.WriteLine("--------------------------------------------------------\nTesting GenRange(Throw Exception)....\n--------------------------------------------------------");
            try
            {
                foreach (int i in Utility.GenRange(200, 130))
                {
                    Console.Write($"[{i}] ");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            Console.WriteLine("\n\n");

            //////////////////////////////////////////////////////
            //Tetsing GenPass by getting ten 5-character strings
            //////////////////////////////////////////////////////
            Console.WriteLine("--------------------------------------------------------\nTesting GenPass....\n--------------------------------------------------------");
            foreach (string s in Utility.GenPass().Take(10))
            {
                Console.Write($"[{s}] ");
            }
            Console.WriteLine("\n\n");

            //////////////////////////////////////////////////////
            //testing shuffle with a list of ints
            //////////////////////////////////////////////////////
            Console.WriteLine("--------------------------------------------------------\nTesting Shuffle....\n--------------------------------------------------------");
            List<int> junk = new List<int>();
            for(int i = 0; i < 10; ++i)
            {
                junk.Add(i);
                Console.Write($"[{i}] ");
            }
            Console.WriteLine();

            
            foreach (int i in Utility.Shuffle(junk))
            {
                Console.Write($"[{i}] ");
            }
            Console.WriteLine("\n\n");

            //////////////////////////////////////////////////////
            //testing shuffle with a list of strings
            //////////////////////////////////////////////////////
            string[] junk2 = new[] { "Dogs", "Cats", "Birds", "Whales", "Zebras", "Elephants" };
            foreach (string s in junk2)
            {
                Console.Write($"[{s}] ");
            }
            Console.WriteLine();
            foreach (string s in Utility.Shuffle(junk2))
            {
                Console.Write($"[{s}] ");
            }
            Console.WriteLine("\n\n");


            //////////////////////////////////////////////////////
            //testing SuperSort with a random list of ints
            //between 0 and 150
            //////////////////////////////////////////////////////
            Console.WriteLine("--------------------------------------------------------\nTesting SuperSort....\n--------------------------------------------------------");
            Random rnd = new Random();
            List<int> junk3 = new List<int>();
            for(int i = 0; i < 15; ++i)
            {
                junk3.Add(rnd.Next(0, 151));
                Console.Write($"[{junk3[i]}] ");
            }
            Console.WriteLine();
            foreach(int i in Utility.SuperSort(junk3))
            {
                Console.Write($"[{i}] ");
            }
            Console.WriteLine("\n\n");

            //////////////////////////////////////////////////////
            //testing SuperSort with a Queue of Balls
            //sorting the ball by radius
            //////////////////////////////////////////////////////
            Queue<Ball> junk4 = new Queue<Ball>();
            for (int i = 0; i < 15; ++i)
            {
                junk4.Enqueue(new Ball(new Point(0, rnd.Next(10, 300)), RandColor.GetColor()));
            }
            foreach(Ball b in junk4)
            {
                Console.Write($"[{b._ballRad}] ");
            }
            Console.WriteLine();
            foreach (Ball b in Utility.SuperSort(junk4))
            {
                Console.Write($"[{b._ballRad}] ");
            }
            Console.WriteLine("\n\n");

            //////////////////////////////////////////////////////
            //testing SuperSort a string and sort the chars
            //////////////////////////////////////////////////////
            string stuff = "SortMe!";
            Console.Write($"{stuff}\n");
            foreach (char c in Utility.SuperSort(stuff))
            {
                Console.Write($"{c}");
            }
            Console.ReadKey();
        }

       
    }
    
}
