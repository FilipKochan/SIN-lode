﻿using System;
using System.Collections.Generic;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] pole;
            int sirka, delka;
            List<int[,]> lode = new List<int[,]>();
            int x = 0, y = 0;

            DeklaracePole(out sirka, out delka);
            pole = new int[sirka, delka];
            bool tvorbaLodi = true;
            bool pohybLodi = true;

            while (tvorbaLodi)
            {
                VykresleniPole(pole);
                TvorbaLodi(pole, lode, ref tvorbaLodi);
                Console.Clear();
            }
            Console.WriteLine("Zadejte index lodi, kterou chcete nacist:");
            int.TryParse(Console.ReadLine(), out int indexLodi);
            Console.Clear();

            while (pohybLodi)
            {
                PohybLodi(lode, lode[indexLodi - 1], pole, ref pohybLodi, ref x, ref y);
                Console.Clear();
            }

            Console.ReadKey();
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
                    else
                        Console.Write(" █ |");

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
                UlozitLod(lode, pole);
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
                                        UlozitLod(lode, pole);
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
                                    UlozitLod(lode, pole);
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
                            UlozitLod(lode, pole);
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

        private static void UlozitLod(List<int[,]> lode, int[,] pole)
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

            Console.WriteLine($"poc0: {pocatecniIndex0}, kon0: {koncovyIndex0}\npoc1: {pocatecniIndex1}, kon1: {koncovyIndex1}");

            //vytvoreni noveho arraye lodi
            int[,] poleLodi = new int[koncovyIndex0 - pocatecniIndex0 + 1, koncovyIndex1 - pocatecniIndex1 + 1];
            for (int j = pocatecniIndex0; j <= koncovyIndex0; j++)
            {
                for (int k = pocatecniIndex1; k <= koncovyIndex1; k++)
                {
                    poleLodi[j - pocatecniIndex0, k - pocatecniIndex1] = pole[j, k];
                }
            }

            lode.Add(poleLodi);
        }

        private static void PohybLodi(List<int[,]> lode, int[,] lod, int[,] pole, ref bool pohybLodi, ref int x, ref int y)
        {
            VykreslitLodNaPole(lod, pole, x, y);
            Console.WriteLine("Muzete pohybovat lodi pomoci sipek, otoceni lodi pomoci R (pro ukonceni stisknete ENTER)...");
            ConsoleKeyInfo zmacknutaKlavesa = Console.ReadKey();
            switch (zmacknutaKlavesa.Key)
            {
                case ConsoleKey.Enter:
                    pohybLodi = false;
                    break;
                case ConsoleKey.LeftArrow:
                    if (x > 0)
                        x--;
                    break;
                case ConsoleKey.RightArrow:
                    if (x + lod.GetLength(0) < pole.GetLength(0))
                        x++;
                    break;
                case ConsoleKey.UpArrow:
                    if (y > 0)
                        y--;
                    break;
                case ConsoleKey.DownArrow:
                    if (y + lod.GetLength(1) < pole.GetLength(1))
                        y++;
                    break;
                case ConsoleKey.R:
                    if (JdeRotace(pole, lod, x, y))
                    {
                        lode[lode.IndexOf(lod)] = RotaceLodi(lod);
                        //x += -posun * ((lod.GetLength(1) + lod.GetLength(0)) / 2 - 1);
                        //y += posun * ((lod.GetLength(1) + lod.GetLength(0)) / 2 - 1);
                        double posunuti = (lod.GetLength(1) - lod.GetLength(0)) / 2;
                        posunuti = Math.Floor(posunuti);
                        x -= (int)posunuti;
                        y += (int)posunuti;
                    }
                    break;
                default:
                    break;
            }
        }

        private static void VykreslitLodNaPole(int[,] lod, int[,] pole, int x, int y)
        {
            ResetPole(pole);
            for (int i = 0; i < lod.GetLength(0); i++)
            {
                for (int j = 0; j < lod.GetLength(1); j++)
                {
                    pole[i + x, j + y] = lod[i, j];
                }
            }
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
            //pokud je jen jeden rozmer lichy
            if (lod.GetLength(0) % 2 + lod.GetLength(1) % 2 == 1)
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
            double posunuti = (lod.GetLength(0) - lod.GetLength(1)) / 2;
            posunuti = Math.Floor(posunuti);
            x -= (int)posunuti;
            y += (int)posunuti;

            if (x >= 0 && x + lod.GetLength(0) < pole.GetLength(0))
            {
                if (y >= 0 && y + lod.GetLength(1) < pole.GetLength(1))
                {
                    return true;
                }
            }

            return false;
        }
    }
}