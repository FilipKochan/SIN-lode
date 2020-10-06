using System;
using System.Collections.Generic;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] pole;
            List<int[,]> lode = new List<int[,]>();
            List<int[,]> lodeVPoli = new List<int[,]>();
            List<int[]> souradniceLodi = new List<int[]>();

            lode.Add(new int[,] { { 0, 1 }, { 0, 1 }, { 1, 1 } });
            lode.Add(new int[,] { { 0, 1, 1 }, { 1, 1, 0 }, { 1, 0, 0 } });
            lode.Add(new int[,] { { 1, 1, 1 }, { 1, 1, 0 } });

            int aktualniIndex;

            pole = DeklaracePole();
            bool tvorbaLodi = true;
            bool pohybLodi = true;

            while (tvorbaLodi)
            {
                VykresleniPole(pole);
                TvorbaLodi(pole, lode, ref tvorbaLodi);
                Console.Clear();
            }

            ResetPole(pole);
            NacteniLodiNaPole(lode, lodeVPoli, souradniceLodi, pole);
            aktualniIndex = 0;

            while (pohybLodi)
            {
                PohybLodi(lode, lodeVPoli, ref aktualniIndex, pole, ref pohybLodi, souradniceLodi);
                Console.Clear();
            }
        }

        private static void NacteniLodiNaPole(List<int[,]> lode, List<int[,]> lodeVPoli, List<int[]> souradnice, int[,] pole)
        {
            Console.Clear();
            Console.WriteLine("Zadejte index lodi, kterou chcete nacist:");
            VykresleniListuLodi(lode);
            int indexLodi;
            while (!int.TryParse(Console.ReadLine(), out indexLodi) && (indexLodi < 1 || indexLodi > lode.Count))
            {
                Console.WriteLine("Zadan, nespravny index, zkuste to prosim znova.");
            };
            //Console.WriteLine("lode.count: {0}", lode.Count);
            //Console.WriteLine("indexlodi: {0}", indexLodi);
            int[] novesouradnice = ZiskatSouradniceLode(lode[indexLodi - 1], pole);
            //Console.WriteLine($"n1: {novesouradnice[0]}, n2: {novesouradnice[1]}");

            if (novesouradnice[0] == -1)
            {
                Console.WriteLine("Pro danou lod neni v poli dost mista. (Zkuste lode premistit a misto uvolnit.)");
                Console.WriteLine("Pokracujte stiskem libovolne klavesy . . .");
                Console.ReadKey();
            }
            else
            {
                //Console.WriteLine($"n1: {novesouradnice[0]}, n2: {novesouradnice[1]}");
                lodeVPoli.Add(lode[indexLodi - 1]);
                souradnice.Add(novesouradnice);
            }
            //Console.WriteLine("lodevpoli.count: {0}", lodeVPoli.Count);
            //Console.WriteLine("\n----------LODE V POLI-----------");
            //VykresleniListuLodi(lodeVPoli, souradnice);
            Console.Clear();
        }

        private static int[,] DeklaracePole()
        {
            int sirka, delka;
            Console.WriteLine("Zadejte rozmery pole (ve formatu napr. 10,8):");
            string vstup = Console.ReadLine();
            //bez zadani rozmeru se automaticky vytvori pole 20x10, abych to nemusel porad psat dokola
            if (string.IsNullOrEmpty(vstup))
            {
                sirka = 20;
                delka = 10;
            }
            else
            {
                string[] rozmery = vstup.Split(',');
                while (!(rozmery.Length == 2 && int.TryParse(rozmery[0], out sirka) && int.TryParse(rozmery[1], out delka)))
                {
                    Console.WriteLine("Zadani bylo chybne, zkuste to znova:");
                    rozmery = Console.ReadLine().Split(',');
                }
            }
            Console.Clear();

            return new int[sirka, delka];
        }

        private static void VykresleniPole(int[,] pole)
        {
            for (int i = 0; i < pole.GetLength(1); i++)
            {
                Console.WriteLine(" " + new String('-', 4 * pole.GetLength(0) + 1));
                Console.Write(" |");
                for (int j = 0; j < pole.GetLength(0); j++)
                {
                    if (pole[j, i] == 0)
                    {
                        Console.Write("   |");
                        //Console.Write(" 0 |");
                    }
                    else if (pole[j, i] == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(" █");
                        Console.ResetColor();
                        Console.Write(" |");
                    }
                    else if (pole[j, i] == 2)
                    {
                        Console.Write(" ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("█");
                        //Console.Write("2");
                        Console.ResetColor();
                        Console.Write(" |");
                    }

                    //Console.Write(" {0} |", pole[j, i]);
                }
                Console.WriteLine();
            }
            Console.WriteLine(" " + new String('-', 4 * pole.GetLength(0) + 1));
        }

        private static void TvorbaLodi(int[,] pole, List<int[,]> lode, ref bool tvorbaLodi)
        {
            bool spravneZadani = false;
            int[] pozice;
            string input;
            string[] stringpozice;
            Console.WriteLine("Vyberte misto pro cast lodi (zadejte souradnice ve formatu napr. 5,1) a potvrdte klavesou ENTER:\n" +
                "Pro ulozeni aktualni lode a tvorbu nove lode pouze stisknete ENTER.\n" +
                "Pro ukonceni zadavani a ulozeni lodi zadejte 'ok'.");
            input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                UlozitLod(pole, lode);
            }
            else if (input == "ok")
            {
                UlozitLod(pole, lode);
                tvorbaLodi = false;
            }
            else
            {
                while (!spravneZadani)
                {
                    stringpozice = input.Split(',');
                    if (stringpozice.Length == 2 && int.TryParse(stringpozice[0], out int x) && int.TryParse(stringpozice[1], out int y))
                    {
                        pozice = new int[] { x, y };
                        if (pozice[0] <= pole.GetLength(0) && pozice[0] > 0 && pozice[1] <= pole.GetLength(1) && pozice[1] > 0)
                        {
                            if (pole[pozice[0] - 1, pozice[1] - 1] != 1)
                            {
                                if (JePolePrazdne(pole))
                                {
                                    spravneZadani = true;
                                    pole[pozice[0] - 1, pozice[1] - 1] = 1;
                                }
                                else if (JeUmisteniDobre(pole, pozice))
                                {
                                    spravneZadani = true;
                                    pole[pozice[0] - 1, pozice[1] - 1] = 1;
                                }
                                else
                                {
                                    Console.WriteLine("Tato cast nelze pro lod zvolit. Zkuste zadat jinou pozici:");
                                    input = Console.ReadLine();
                                    if (input == "ok")
                                    {
                                        lode.Add(VytahnoutLodZPole(pole));
                                        tvorbaLodi = false;
                                        Console.WriteLine("Lod byla ulozena pod indexem {0}.", lode.Count);
                                        Console.WriteLine("Pokracujte stiskem libovolne klavesy");
                                        Console.ReadKey();
                                    }
                                }

                            }
                            else
                            {
                                Console.WriteLine("Zadana pozice je jiz obsazena, zkuste prosim jinou.");
                                input = Console.ReadLine();
                                if (input == "ok")
                                {
                                    lode.Add(VytahnoutLodZPole(pole));
                                    tvorbaLodi = false;
                                    Console.WriteLine("Lod byla ulozena pod indexem {0}.", lode.Count);
                                    Console.WriteLine("Pokracujte stiskem libovolne klavesy");
                                    Console.ReadKey();
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Zadani bylo chybne, zkuste to prosim znova:");
                        input = Console.ReadLine();
                        if (input == "ok")
                        {
                            lode.Add(VytahnoutLodZPole(pole));
                            tvorbaLodi = false;
                            Console.WriteLine("Lod byla ulozena pod indexem {0}.", lode.Count);
                            Console.WriteLine("Pokracujte stiskem libovolne klavesy");
                            Console.ReadKey();
                        }
                    }
                }
            }
        }

        private static void UlozitLod(int[,] pole, List<int[,]> lode)
        {
            int[,] novaLod = VytahnoutLodZPole(pole);
            //pridavam jen lod co neni "prazdna"
            if (novaLod.GetLength(0) > 0)
            {
                lode.Add(novaLod);
                ResetPole(pole);
                Console.WriteLine("Lod byla ulozena pod indexem {0}.", lode.Count);
                Console.WriteLine("Pokracujte stiskem libovolne klavesy . . .");
                Console.ReadKey();
            }
        }

        private static bool JePolePrazdne(int[,] pole)
        {
            for (int i = 0; i < pole.GetLength(0); i++)
            {
                for (int j = 0; j < pole.GetLength(1); j++)
                {
                    if (pole[i, j] != 0)
                        return false;
                }
            }
            return true;
        }

        private static bool JeUmisteniDobre(int[,] pole, int[] pozice)
        {
            int x = pozice[0] - 1;
            int y = pozice[1] - 1;
            if (y - 1 >= 0 && pole[x, y - 1] == 1)
                return true;
            if (y + 1 <= pole.GetLength(1) && pole[x, y + 1] == 1)
                return true;
            if (x - 1 >= 0 && pole[x - 1, y] == 1)
                return true;
            if (x + 1 <= pole.GetLength(0) && pole[x + 1, y] == 1)
                return true;

            return false;
        }

        private static int[,] VytahnoutLodZPole(int[,] pole)
        {
            int pocatecniIndex0 = -1, koncovyIndex0 = pole.GetLength(0);
            int pocatecniIndex1 = -1, koncovyIndex1 = pole.GetLength(1);

            int i = 0;
            //x-zacatek a x-konec
            while (i < pole.GetLength(0))
            {
                for (int j = 0; j < pole.GetLength(1); j++)
                {
                    if (pole[i, j] == 1)
                    {
                        pocatecniIndex0 = i;
                        break;
                    }
                }
                if (pocatecniIndex0 != -1)
                    break;
                i++;
            }
            i = pole.GetLength(0) - 1;
            while (i >= 0)
            {
                for (int j = 0; j < pole.GetLength(1); j++)
                {
                    if (pole[i, j] == 1)
                    {
                        koncovyIndex0 = i;
                        break;
                    }
                }
                if (koncovyIndex0 != pole.GetLength(0))
                    break;
                i--;
            }

            //y-zacatek a y-konec
            i = 0;
            while (i < pole.GetLength(1))
            {
                for (int j = 0; j < pole.GetLength(0); j++)
                {
                    if (pole[j, i] == 1)
                    {
                        pocatecniIndex1 = i;
                        break;
                    }
                }
                if (pocatecniIndex1 != -1)
                    break;
                i++;
            }
            i = pole.GetLength(1) - 1;
            while (i >= 0)
            {
                for (int j = 0; j < pole.GetLength(0); j++)
                {
                    if (pole[j, i] == 1)
                    {
                        koncovyIndex1 = i;
                        break;
                    }
                }
                if (koncovyIndex1 != pole.GetLength(1))
                    break;
                i--;
            }

            //Console.WriteLine($"poc0: {pocatecniIndex0}, kon0: {koncovyIndex0}\npoc1: {pocatecniIndex1}, kon1: {koncovyIndex1}");

            //vytvoreni noveho arraye lodi
            if (pocatecniIndex0 > -1)
            {
                int[,] poleLodi = new int[koncovyIndex0 - pocatecniIndex0 + 1, koncovyIndex1 - pocatecniIndex1 + 1];
                for (int j = pocatecniIndex0; j <= koncovyIndex0; j++)
                {
                    for (int k = pocatecniIndex1; k <= koncovyIndex1; k++)
                    {
                        poleLodi[j - pocatecniIndex0, k - pocatecniIndex1] = pole[j, k];
                    }
                }
                return poleLodi;
            }
            else
                return new int[0, 0];
        }

        private static void PohybLodi(List<int[,]> lode, List<int[,]> lodeVPoli, ref int aktualiIndex, int[,] pole, ref bool pohybLodi, List<int[]> souradniceLodi)
        {
            VykreslitLodeNaPole(lodeVPoli, souradniceLodi, aktualiIndex, pole);
            Console.WriteLine("Muzete pohybovat lodi pomoci sipek, otoceni lodi pomoci R.\nPro nacteni nove lode na pole stisknete N.\nPro prepinani mezi aktualnimi lodmi v poli stisknete TAB.\n(pro ukonceni stisknete ENTER)...");
            ConsoleKeyInfo zmacknutaKlavesa = Console.ReadKey();

            if (JdeUdelatPohyb(pole, new List<int[,]>(lodeVPoli), new List<int[]>(souradniceLodi), aktualiIndex, zmacknutaKlavesa))
            //if (true)
            {
                switch (zmacknutaKlavesa.Key)
                {
                    case ConsoleKey.Enter:
                        pohybLodi = false;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (souradniceLodi[aktualiIndex][0] > 0)
                            souradniceLodi[aktualiIndex][0]--;
                        break;
                    case ConsoleKey.RightArrow:
                        if (souradniceLodi[aktualiIndex][0] + lodeVPoli[aktualiIndex].GetLength(0) < pole.GetLength(0))
                            souradniceLodi[aktualiIndex][0]++;
                        break;
                    case ConsoleKey.UpArrow:
                        if (souradniceLodi[aktualiIndex][1] > 0)
                            souradniceLodi[aktualiIndex][1]--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (souradniceLodi[aktualiIndex][1] + lodeVPoli[aktualiIndex].GetLength(1) < pole.GetLength(1))
                            souradniceLodi[aktualiIndex][1]++;
                        break;
                    case ConsoleKey.N:
                        NacteniLodiNaPole(lode, lodeVPoli, souradniceLodi, pole);
                        break;
                    case ConsoleKey.Tab:
                        aktualiIndex++;
                        aktualiIndex %= lodeVPoli.Count;
                        break;
                    case ConsoleKey.R:
                        if (JdeRotace(pole, lodeVPoli[aktualiIndex], souradniceLodi[aktualiIndex][0], souradniceLodi[aktualiIndex][1]))
                        {
                            UdelatRotaci(lodeVPoli, aktualiIndex, souradniceLodi);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private static void UdelatRotaci(List<int[,]> lodeVPoli, int aktualiIndex, List<int[]> souradniceLodi)
        {
            lodeVPoli[aktualiIndex] = RotaceLodi(lodeVPoli[aktualiIndex]);
            double posunuti = (lodeVPoli[aktualiIndex].GetLength(0) - lodeVPoli[aktualiIndex].GetLength(1)) / 2;
            posunuti = Math.Floor(posunuti);
            souradniceLodi[aktualiIndex][0] -= (int)posunuti;
            souradniceLodi[aktualiIndex][1] += (int)posunuti;
            // orizne lod od casti pridanych pro rotaci a upravi souradnice tak aby se lod po oriznuti neposunula
            int[] noveSoradnice = NoveSouradnicePoOriznuti(lodeVPoli[aktualiIndex]);
            lodeVPoli[aktualiIndex] = VytahnoutLodZPole(lodeVPoli[aktualiIndex]);
            souradniceLodi[aktualiIndex][0] += noveSoradnice[0];
            souradniceLodi[aktualiIndex][1] += noveSoradnice[1];
        }
        private static int[] NoveSouradnicePoOriznuti(int[,] lod)
        {
            bool jePrvniRadekPrazdny = true, jePrvniSloupecPrazdny = true;
            for (int i = 0; i < lod.GetLength(0); i++)
            {
                if (lod[i, 0] == 1)
                {
                    jePrvniSloupecPrazdny = false;
                    break;
                }
            }
            for (int i = 0; i < lod.GetLength(1); i++)
            {
                if (lod[0, i] == 1)
                {
                    jePrvniRadekPrazdny = false;
                    break;
                }
            }

            if (jePrvniSloupecPrazdny)
                return new int[] { 0, 1 };
            else if (jePrvniRadekPrazdny)
                return new int[] { 1, 0 };
            else
                return new int[] { 0, 0 };
        }

        private static void VykreslitLodeNaPole(List<int[,]> lodeVPoli, List<int[]> souradniceLodi, int aktualniIndex, int[,] pole, bool vykrestlit = true)
        {
            ResetPole(pole);
            //Console.WriteLine("lodevpole.count: {0}", lodeVPoli.Count);
            for (int k = 0; k < lodeVPoli.Count; k++)
            {
                int val = k == aktualniIndex ? 2 : 1;
                for (int i = 0; i < lodeVPoli[k].GetLength(0); i++)
                {
                    for (int j = 0; j < lodeVPoli[k].GetLength(1); j++)
                    {
                        if (k == aktualniIndex)
                            val = 2;
                        if (lodeVPoli[k][i, j] == 1)
                            pole[i + souradniceLodi[k][0], j + souradniceLodi[k][1]] = val;
                    }
                }
            }
            if (vykrestlit)
                VykresleniPole(pole);
        }

        private static void VykreslitLodNaPole(int[,] lod, int[] souradniceLodi, int[,] pole)
        {
            for (int i = 0; i < lod.GetLength(0); i++)
            {
                for (int j = 0; j < lod.GetLength(1); j++)
                {
                    if (lod[i, j] == 1)
                        pole[i + souradniceLodi[0], j + souradniceLodi[1]] = 1;
                }
            }
        }

        private static void ResetPole(int[,] pole)
        {
            for (int i = 0; i < pole.GetLength(0); i++)
            {
                for (int j = 0; j < pole.GetLength(1); j++)
                {
                    pole[i, j] = 0;
                }
            }
        }

        private static int[,] RotaceLodi(int[,] lod)
        {
            int novyIndex0 = lod.GetLength(0), novyIndex1 = lod.GetLength(1);

            if ((lod.GetLength(0) % 2 + lod.GetLength(1) % 2) == 1)
            {
                if (lod.GetLength(0) % 2 == 1)
                    novyIndex0 = lod.GetLength(0) + 1;
                if (lod.GetLength(1) % 2 == 1)
                    novyIndex1 = lod.GetLength(1) + 1;
            }

            int[,] upravenaPuvodniLod = new int[novyIndex0, novyIndex1];
            for (int i = 0; i < upravenaPuvodniLod.GetLength(0); i++)
            {
                for (int j = 0; j < upravenaPuvodniLod.GetLength(1); j++)
                {
                    if (i < lod.GetLength(0) && j < lod.GetLength(1))
                        upravenaPuvodniLod[i, j] = lod[i, j];
                    else
                        upravenaPuvodniLod[i, j] = 0;
                }
            }


            int[,] otocenaLod = new int[upravenaPuvodniLod.GetLength(1), upravenaPuvodniLod.GetLength(0)];
            for (int i = 0; i < lod.GetLength(0); i++)
            {
                for (int j = 0; j < lod.GetLength(1); j++)
                {
                    otocenaLod[j, i] = lod[lod.GetLength(0) - 1 - i, j];
                }
            }
            return otocenaLod;
        }

        private static bool JdeRotace(int[,] pole, int[,] lod, int x, int y)
        {
            //kontroluje pouze, jestli lod po rotaci zustane v poli
            int[,] otocenaLod = RotaceLodi(lod);
            double posunuti = (otocenaLod.GetLength(0) - otocenaLod.GetLength(1)) / 2;
            posunuti = Math.Floor(posunuti);
            x -= (int)posunuti;
            y += (int)posunuti;
            /*Console.WriteLine("otocene x: {0}, y: {1}", x, y);
            Console.ReadKey();*/
            if (x >= 0 && x + otocenaLod.GetLength(0) <= pole.GetLength(0))
            {
                if (y >= 0 && y + otocenaLod.GetLength(1) <= pole.GetLength(1))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool JdeUdelatPohyb(int[,] pole, List<int[,]> lodeVPoli, List<int[]> souradniceLodi, int aktualiIndex, ConsoleKeyInfo zmacknutaKlavesa)
        {
            //kontroluje prekryti lodi
            int puvodniSoucet = SoucetPole(pole);
            int novySoucet;
            int[,] novePole = new int[pole.GetLength(0), pole.GetLength(1)];
            List<int[]> souradniceLodiKopie = KopiePole(souradniceLodi);
            List<int[,]> lodeVPoliKopie = KopiePole(lodeVPoli);

            switch (zmacknutaKlavesa.Key)
            {
                case ConsoleKey.LeftArrow:
                    if (souradniceLodiKopie[aktualiIndex][0] > 0)
                        souradniceLodiKopie[aktualiIndex][0]--;
                    break;
                case ConsoleKey.RightArrow:
                    if (souradniceLodiKopie[aktualiIndex][0] + lodeVPoliKopie[aktualiIndex].GetLength(0) < pole.GetLength(0))
                        souradniceLodiKopie[aktualiIndex][0]++;
                    break;
                case ConsoleKey.UpArrow:
                    if (souradniceLodiKopie[aktualiIndex][1] > 0)
                        souradniceLodiKopie[aktualiIndex][1]--;
                    break;
                case ConsoleKey.DownArrow:
                    if (souradniceLodiKopie[aktualiIndex][1] + lodeVPoliKopie[aktualiIndex].GetLength(1) < pole.GetLength(1))
                        souradniceLodiKopie[aktualiIndex][1]++;
                    break;
                case ConsoleKey.R:
                    if (JdeRotace(pole, lodeVPoliKopie[aktualiIndex], souradniceLodiKopie[aktualiIndex][0], souradniceLodiKopie[aktualiIndex][1]))
                    {
                        UdelatRotaci(lodeVPoliKopie, aktualiIndex, souradniceLodiKopie);
                    }
                    break;
                default:
                    break;
            }

            VykreslitLodeNaPole(lodeVPoliKopie, souradniceLodiKopie, aktualiIndex, novePole, false);
            novySoucet = SoucetPole(novePole);
            if (puvodniSoucet == novySoucet)
                return true;
            else
                return false;
        }

        private static int SoucetPole(int[,] pole)
        {
            int soucet = 0;
            for (int i = 0; i < pole.GetLength(0); i++)
            {
                for (int j = 0; j < pole.GetLength(1); j++)
                {
                    soucet += pole[i, j];
                }
            }
            return soucet;
        }

        private static List<int[,]> KopiePole(List<int[,]> pole)
        {
            List<int[,]> kopie = new List<int[,]>();

            foreach (int[,] arr in pole)
            {
                int[,] kopiePole = new int[arr.GetLength(0), arr.GetLength(1)];
                for (int i = 0; i < arr.GetLength(0); i++)
                {
                    for (int j = 0; j < arr.GetLength(1); j++)
                    {
                        kopiePole[i, j] = arr[i, j];
                    }
                }
                kopie.Add(kopiePole);
            }
            return kopie;
        }

        private static List<int[]> KopiePole(List<int[]> pole)
        {
            List<int[]> kopie = new List<int[]>();

            foreach (int[] arr in pole)
            {
                int[] kopiePole = new int[arr.Length];
                for (int i = 0; i < arr.Length; i++)
                {
                    kopiePole[i] = arr[i];
                }
                kopie.Add(kopiePole);
            }
            return kopie;
        }

        private static int[,] KopiePole(int[,] pole)
        {
            int[,] kopiePole = new int[pole.GetLength(0), pole.GetLength(1)];

            for (int i = 0; i < pole.GetLength(0); i++)
            {
                for (int j = 0; j < pole.GetLength(1); j++)
                {
                    kopiePole[i, j] = pole[i, j];
                }
            }

            return kopiePole;
        }

        private static int[] ZiskatSouradniceLode(int[,] lodKPridani, int[,] pole)
        {
            VykresleniPole(pole);
            int sirkaLode = lodKPridani.GetLength(0);
            int delkaLode = lodKPridani.GetLength(1);

            int x = 0, y = 0;
            bool hledaniSouradnic = true;
            int spravnySoucet = SoucetPole(pole) + SoucetPole(lodKPridani);
            int soucetPoleSLodi;
            while (x + sirkaLode <= pole.GetLength(0) && hledaniSouradnic)
            {
                while (y + delkaLode <= pole.GetLength(1) && hledaniSouradnic)
                {
                    int[,] zkusebniPole = KopiePole(pole);//pole musim po kazdem pokusu vytvorit znovu
                    for (int i = 0; i < sirkaLode; i++)
                    {
                        for (int j = 0; j < delkaLode; j++)
                        {
                            //menim pouze pozice na kterych lod realne je (ne nuly – prazdna mista)
                            if (lodKPridani[i, j] == 1)
                                zkusebniPole[i + x, j + y] = 1;
                        }
                    }
                    soucetPoleSLodi = SoucetPole(zkusebniPole);
                    if (soucetPoleSLodi == spravnySoucet)
                    {
                        hledaniSouradnic = false;
                        //Console.WriteLine("Nalezeno!: {0}, {1}.", x, y);
                        //Console.ReadKey();
                    }
                    else
                    {
                        //Console.WriteLine("nenalezeno: {0}, {1}.", x, y);
                        //Console.WriteLine("spravnysoucet: {0}, soucetslodi: {1}", spravnySoucet, soucetPoleSLodi);
                        //Console.ReadKey();
                        y++;
                    }
                }
                if (hledaniSouradnic)
                {
                    x++;
                    y = 0;
                }
            }


            if (hledaniSouradnic)
            {
                Console.WriteLine("NENALEZENY ZADNE SOURADNICE");
                return new int[] { -1, -1 };
            }
            else
            {
                Console.WriteLine("Nalezeny souradnice {0} a {1}.", x, y);
                return new int[] { x, y };
            }
        }

        private static void VykresleniListuLodi(List<int[,]> lode)
        {
            foreach (int[,] lod in lode)
            {
                Console.WriteLine("\nLod {0}:", lode.IndexOf(lod) + 1);
                VykresleniPole(lod);
            }
        }
    }
}
