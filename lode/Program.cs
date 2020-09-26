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


            int aktualniIndex;

            DeklaracePole(out int sirka, out int delka);
            pole = new int[sirka, delka];
            bool tvorbaLodi = true;
            bool pohybLodi = true;

            while (tvorbaLodi)
            {
                VykresleniPole(pole);
                TvorbaLodi(pole, lode, ref tvorbaLodi);
                Console.Clear();
            }

            NacteniLodiNaPole(lode, lodeVPoli, souradniceLodi);
            aktualniIndex = 0;

            while (pohybLodi)
            {
                PohybLodi(lode, lodeVPoli, ref aktualniIndex, pole, ref pohybLodi, souradniceLodi);
                Console.Clear();
            }

            Console.ReadKey();
        }

        private static void NacteniLodiNaPole(List<int[,]> lode, List<int[,]> lodeVPoli, List<int[]> souradnice)
        {
            Console.Clear();
            Console.WriteLine("Zadejte index lodi, kterou chcete nacist:");
            int.TryParse(Console.ReadLine(), out int indexLodi);
            lodeVPoli.Add(lode[indexLodi - 1]);
            souradnice.Add(new int[] { 0, 0 });
            Console.Clear();
        }

        private static void DeklaracePole(out int sirka, out int delka)
        {
            Console.WriteLine("Zadejte rozmery pole (ve formatu napr. 10,8):");
            string[] rozmery = Console.ReadLine().Split(',');
            while (!(rozmery.Length == 2 && int.TryParse(rozmery[0], out sirka) && int.TryParse(rozmery[1], out delka)))
            {
                Console.WriteLine("Zadani bylo chybne, zkuste to znova:");
                rozmery = Console.ReadLine().Split(',');
            }
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
                        Console.Write("   |");
                    else if (pole[j, i] == 1)
                        Console.Write(" █ |");
                    else if (pole[j, i] == 2)
                    {
                        Console.Write(" ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("█");
                        Console.ResetColor();
                        Console.Write(" |");
                    }

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
                "Pro ukonceni zadavani a ulozeni lodi zadejte 'ok'.");
            input = Console.ReadLine();
            if (input == "ok")
            {
                lode.Add(UlozitLod(pole));
                tvorbaLodi = false;
                Console.WriteLine("Lod byla ulozena pod indexem {0}.", lode.Count);
                Console.WriteLine("Pokracujte stiskem libovolne klavesy");
                Console.ReadKey();
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
                                        lode.Add(UlozitLod(pole));
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
                                    lode.Add(UlozitLod(pole));
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
                            lode.Add(UlozitLod(pole));
                            tvorbaLodi = false;
                            Console.WriteLine("Lod byla ulozena pod indexem {0}.", lode.Count);
                            Console.WriteLine("Pokracujte stiskem libovolne klavesy");
                            Console.ReadKey();
                        }
                    }
                }
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

        private static int[,] UlozitLod(int[,] pole)
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

        private static void PohybLodi(List<int[,]> lode, List<int[,]> lodeVPoli, ref int aktualiIndex, int[,] pole, ref bool pohybLodi, List<int[]> souradniceLodi)
        {
            VykreslitLodeNaPole(lodeVPoli, souradniceLodi, aktualiIndex, pole);
            Console.WriteLine("Muzete pohybovat lodi pomoci sipek, otoceni lodi pomoci R \nPro nacteni nove lode na pole stisknete N. \nPro prepinani mezi aktualnimi lodmi v poli stisknete TAB \n(pro ukonceni stisknete ENTER)...");
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
                        NacteniLodiNaPole(lode, lodeVPoli, souradniceLodi);
                        break;
                    case ConsoleKey.Tab:
                        aktualiIndex++;
                        aktualiIndex %= lodeVPoli.Count;
                        break;
                    case ConsoleKey.R:
                        if (JdeRotace(pole, lodeVPoli[aktualiIndex], souradniceLodi[aktualiIndex][0], souradniceLodi[aktualiIndex][1]))
                        {
                            lodeVPoli[aktualiIndex] = RotaceLodi(lodeVPoli[aktualiIndex]);
                            //x += -posun * ((lod.GetLength(1) + lod.GetLength(0)) / 2 - 1); nesmysl
                            //y += posun * ((lod.GetLength(1) + lod.GetLength(0)) / 2 - 1); nesmysl
                            double posunuti = (lodeVPoli[aktualiIndex].GetLength(0) - lodeVPoli[aktualiIndex].GetLength(1)) / 2;
                            posunuti = Math.Floor(posunuti);
                            souradniceLodi[aktualiIndex][0] -= (int)posunuti;
                            souradniceLodi[aktualiIndex][1] += (int)posunuti;
                            lodeVPoli[aktualiIndex] = UlozitLod(lodeVPoli[aktualiIndex]); // orizne lod od pridanych casti
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private static void VykreslitLodeNaPole(List<int[,]> lodeVPoli, List<int[]> souradniceLodi, int aktualniIndex, int[,] pole, bool vykrestlit = true)
        {
            ResetPole(pole);
            int mult = 1;
            foreach (int[,] lod in lodeVPoli)
            {
                for (int i = 0; i < lod.GetLength(0); i++)
                {
                    for (int j = 0; j < lod.GetLength(1); j++)
                    {
                        if (lodeVPoli.IndexOf(lod) == aktualniIndex)
                            mult = 2;
                        if (lod[i, j] == 1)
                            pole[i + souradniceLodi[lodeVPoli.IndexOf(lod)][0], j + souradniceLodi[lodeVPoli.IndexOf(lod)][1]] = mult * lod[i, j];
                        mult = 1;
                    }
                }
            }
            if (vykrestlit)
                VykresleniPole(pole);
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
            int puvodniSoucet = SoucetPole(pole);
            int novySoucet;
            var novePole = new int[pole.GetLength(0), pole.GetLength(1)];
            List<int[]> souradniceLodiKopie = KopieListu(souradniceLodi);
            List<int[,]> lodeVPoliKopie = KopieListu(lodeVPoli);

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
                        lodeVPoliKopie[aktualiIndex] = RotaceLodi(lodeVPoliKopie[aktualiIndex]);
                        //x += -posun * ((lod.GetLength(1) + lod.GetLength(0)) / 2 - 1); nesmysl
                        //y += posun * ((lod.GetLength(1) + lod.GetLength(0)) / 2 - 1); nesmysl
                        double posunuti = (lodeVPoliKopie[aktualiIndex].GetLength(0) - lodeVPoliKopie[aktualiIndex].GetLength(1)) / 2;
                        posunuti = Math.Floor(posunuti);
                        souradniceLodiKopie[aktualiIndex][0] -= (int)posunuti;
                        souradniceLodiKopie[aktualiIndex][1] += (int)posunuti;
                        lodeVPoliKopie[aktualiIndex] = UlozitLod(lodeVPoliKopie[aktualiIndex]); // orizne lod od pridanych casti
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

        private static List<int[,]> KopieListu(List<int[,]> list)
        {
            var kopieListu = new List<int[,]>();
            for (int k = 0; k < list.Count; k++)
            {
                var kopiePole = new int[list[k].GetLength(0), list[k].GetLength(1)];
                for (int i = 0; i < list[k].GetLength(0); i++)
                {
                    for (int j = 0; j < list[k].GetLength(1); j++)
                    {
                        kopiePole[i, j] = list[k][i, j];
                    }
                }
                kopieListu.Add(kopiePole);
            }
            return kopieListu;
        }

        private static List<int[]> KopieListu(List<int[]> list)
        {
            var kopieListu = new List<int[]>();
            for (int k = 0; k < list.Count; k++)
            {
                var kopiePole = new int[list[k].Length];
                for (int i = 0; i < list[k].Length; i++)
                {
                    kopiePole[i] = list[k][i];
                }
                kopieListu.Add(kopiePole);
            }
            return kopieListu;
        }
    }
}
