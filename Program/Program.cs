using System.IO; //Filebekéréshez.
using System.Collections; //Arraylist-hez.

namespace NKDG9I_Menu //Egy menüválasztós program, ami tudja többször meghívni az alprogramjait.
                      //(Alprogramok: Akasztófa. Bővíthető!)
                      //Mindenhol ellenőrzött bekérés van.
{
    internal class Program
    {
        static void Main(string[] args) //Kiadja az utasítást a menü elindítására. Itt indul a program.
        {
            Console.WriteLine("Welcome in my homework\n");
            Menu();
            Console.ReadKey();
        }
        static void Menu()
        {
            bool end = false;
            int choice = 0;
            while (!end)
            {
                bool good = false;
                Console.WriteLine("\nPlease choose from the programs by tiping its number: (Hangman: 1, End: 0)");
                while (!good)
                {
                    good = int.TryParse(Console.ReadLine(), out choice);
                }
                Console.Clear();
                switch (choice)
                {
                    case 0:
                        end = true;
                        Console.WriteLine("End");
                        break;
                    case 1:
                        Hangman();
                        break;
                }
            }
        }//Kiválasztja a felhasználó melyik programot szeretné futtatni.
         //Ide térnek vissza az alprogramok. Addig fut amíg a felhasználó nem ad 0-át.
         //Egyenlőre csak az akasztófa programot implementáltam bele, de meg van a lehetőség arra, hogy több mindent összepakolhassunk.
         //Elkezdtem egy nyelvtanuló programot is. Azt kivettem belőle mert nem végeztem volna.
        static void Hangman()
        {
            string s = " "; //Változók és kezdeti értékeik.
            char c;
            int life = 8; //Azt láttam a legizgalmasabbnak, ha pontosan 8 élet van. Próbálkoztam azzal, hogy a kiválasztott szó hosszának fele, de az túl nehéz lett volna, vagy túl könnyű, mert az angolban csak 29 kaerakter van.
            bool end = false;
            ArrayList wrong_chars = new ArrayList(); //Itt tárolja a program a rossz tippeket. Figyelve, hogy ne legyen ismétlődés.

            Text_Reader(out s);
            
            bool[] found = new bool[s.Length]; //A már megtalált karakterek helye a szóban.
            while (!end)
            {
                Console.WriteLine("\n\nLife: " + life);
                Try_In(out c);
                Console.Clear();
                Decider(life, s, c, out life, found, wrong_chars);
                New_Writer(found, s, wrong_chars);
                End_Decider(life, out end, found, s);
            }
            Win_Decider(life);
        }//Az akasztófa alprogram függvénye.
        static void Text_Reader(out string s)
        {
            s = "";
            Random r = new Random();
            try
            {
                FileStream fs = new FileStream("words.txt", FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                double i = 0;
                double edge = 0;
                for (int j = 0; j < 10; j++)
                {
                    edge += r.Next(0, 1000);
                }
                while (i < edge)
                {

                    s = sr.ReadLine();
                    i++;
                }
                Writer(s);
                sr.Close();
                fs.Close();
            }
            catch (IOException)
            {
                Console.WriteLine("Error");
            }
        }//Beolvas egy random angol szót egy txt-file ból és ezt kell majd kitalálni. Ellenőrzötten olvas be.
        static void Try_In(out char c)
        {
            c = ' ';
            bool good = false;
            while (!good)
            {
                Console.WriteLine("\nGive a char: \n");
                good = char.TryParse(Console.ReadLine(), out c);
            }
        }//Az akasztófa bemenetét kezeli.
        static void Writer(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                Console.Write("_ ");
            }
        }//Az akasztófa első üzenetét írja ki a konzolra.
        static void Decider(int old_life, string s, char c, out int life, bool[] found, ArrayList wrong_chars)
        {
            life = old_life;
            bool die = true;
            for (int i = 0; i < s.Length; i++)
            {
                if (c == s[i])
                {
                    found[i] = true;
                    die = false;
                }
            }

            if (die)
            {
                life--;
                bool good = true;
                for (int i = 0; i < wrong_chars.Count; i++)
                {
                    if(c == Convert.ToChar(wrong_chars[i]))
                    {
                        good = false;
                    }
                }
                if (good)
                {
                    wrong_chars.Add(c);
                }
            }
        }//Az akasztófa programban
         //eldönti, hogy van-e a szóban a megadott karakter és ha igen, akkor megadja hol.
         //Ha nincs találat, akkor levon egy életet.
        static void New_Writer(bool[] found, string s, ArrayList wrong_chars)
        {
            for (int i = 0; i < found.Length; i++)
            {
                if (found[i])
                {
                    Console.Write("{0} ", s[i]);
                }
                else
                {
                    Console.Write("_ ");
                }
            }
            Console.WriteLine("\n\nThe wrong chars are: ");
            foreach (var i in wrong_chars)
            {
                Console.Write("{0}, ", i);
            }
        }//Az akasztófa már kitalált értékeit kiírja a konzolra.
        static void End_Decider(int life, out bool end, bool[] found, string s)
        {
            end = true;

            for (int i = 0; i < s.Length; i++)
            {
                if (!found[i])
                {
                    end = false;
                }
            }
            if (life == 0)
            {
                end = true;
                Console.WriteLine("You died!\n\nThe correct answer would be: " + s);
                Console.ReadKey();
            }
        }//Eldönti, hogy vége van-e az akasztófa programnak.
        static void Win_Decider(int life)
        {
            if (life > 0)
            {
                Console.WriteLine("\nYou won!!");
                Console.ReadKey();
            }
            Console.Clear();
        }//Eldönti, hogy nyertünk-e az akasztófa alprogramban.
         //Ha igen kiírja a konzolra.
    }
} 