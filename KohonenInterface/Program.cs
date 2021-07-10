using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{

    public struct KohonenInfo
    {
        public int n, k, m, qMax, numPr;
        public double v, vIzm;
        public double[,] prim;
    }

    class Kohonen
    {
        private int n, k, m, q = 1, qMax;
        private double v, vIzm;

        private double[,] koef;
        private double[,] prim;

        private int[] sequence_prim;

        public string clustStr = "", rasstStr = "", koefStr = "", istStr = "";


        public void load_info(KohonenInfo ki)
        {
            k = ki.k;
            n = ki.n;
            m = ki.m;
            qMax = ki.qMax;
            v = ki.v;
            vIzm = ki.vIzm;
            prim = ki.prim;
            sequence_prim = new int [n];
            for (int i = 0; i < n; i++)
                sequence_prim[i] = i;
            koef = new double[k, m];
        }


        //перемешивание примеров для случайного порядка подачи примеров
        private void Shuffle()
        {
            if (sequence_prim.Length < 1) return;
            var random = new Random();
            for (var i = 0; i < sequence_prim.Length; i++)
            {
                var key = sequence_prim[i];
                var rnd = random.Next(i, sequence_prim.Length);
                sequence_prim[i] = sequence_prim[rnd];
                sequence_prim[rnd] = key;
            }
        }

        //вычисление минимального или максимального значения всех примеров на одном входе
        private double MinMaxColumn(int requme, int num_column)
        {
            double num = prim[0, num_column];
            for (int pr = 0; pr < n; pr++)
            {
                if (requme == 0) //поиск минимума
                    if (prim[pr, num_column] < num)
                        num = prim[pr, num_column];
                if (requme == 1) //поиск максимума
                    if (prim[pr, num_column] > num)
                        num = prim[pr, num_column];
            }
            return num;
        }

        //нормализация примеров впределах диапазона [0, 1]
        private void NormalizePrim()
        {
            for (int input = 0; input < m; input++)
            {
                //проверка, входит ли вход во всех примерах в диапазон [0, 1]
                if (MinMaxColumn(0, input) <= 0 || MinMaxColumn(1, input) >= 1)
                {
                    double min = MinMaxColumn(0, input), max = MinMaxColumn(1, input);
                    for (int pr = 0; pr < n; pr++)
                    {
                        prim[pr, input] = (prim[pr, input] - min) / (max - min);
                    }
                }
            }
        }

        //инициализация весовых коэффицентов случайными числами
        private void InitRandom()
        {
            var random = new Random();
            //вычисление ограничений
            double min = 0.5 - Math.Pow(m, -0.5), max = 0.5 + Math.Pow(m, -0.5);
            //заполнение
            for (int clust = 0; clust < k; clust++)
                for (int input = 0; input < m; input++)
                {
                    koef[clust, input] = min + random.NextDouble() * (max - min);
                }
        }

        //уменьшение скорости
        private void ChangeSpeed()
        {
            v = v * vIzm;
        }

        //подача примера
        private void Calculation(int primer)
        {
            double[] distance = new double[k];
            double sum = 0;
            for (int clust = 0; clust < k; clust++)
            {
                for (int input = 0; input < m; input++)
                {
                    sum += (prim[primer, input] - koef[clust, input]) * (prim[primer, input] - koef[clust, input]);
                }
                distance[clust] = Math.Sqrt(sum);
                sum = 0;
            }

            for (int clust = 0; clust < k; clust++)
            {
                istStr += "Расстояние до кластера " + (clust + 1) + ": " + Math.Round(distance[clust], 2) + Environment.NewLine;
            }

            //поиск нейрона-победителя
            int min = 0;
            for (int clust = 0; clust < k; clust++)
            {
                if (distance[clust] < distance[min])
                    min = clust;
            }
            istStr += "Нейрон-победитель: " + (min + 1) + Environment.NewLine;

            //изменение весов
            for (int input = 0; input < m; input++)
            {
                koef[min, input] = koef[min, input] + v * (prim[primer, input] - koef[min, input]);
            }
            istStr += "Новые веса: " + Environment.NewLine + PrintKoef();

        }

        //вывод в консоль весовых коэфицентов
        private string PrintKoef()
        {
            string curStr = "";

            curStr += "№\t";
            for (int input = 0; input < m; input++)
                curStr += "x" + (input + 1) + "\t";
            curStr += Environment.NewLine;

            for (int clust = 0; clust < k; clust++)
            {
                curStr += (clust + 1) + "\t";
                for (int input = 0; input < m; input++)
                {
                    curStr += Math.Round(koef[clust, input], 2) + "\t";
                }
                curStr += Environment.NewLine;
            }
            curStr += Environment.NewLine;

            return curStr;
        }

        //вывод в консоль примеров
        private string PrintPrim()
        {
            string curStr = "";

            curStr += "№\t";
            for (int input = 0; input < m; input++)
                curStr += "x" + (input + 1) + "\t";
            curStr += Environment.NewLine;

            for (int clust = 0; clust < n; clust++)
            {
                curStr += (clust + 1) + "\t";
                for (int input = 0; input < m; input++)
                {
                    curStr += Math.Round(prim[clust, input], 2) + "\t";
                }
                curStr += Environment.NewLine;
            }
            curStr += Environment.NewLine;

            return curStr;
        }

        //кластеризация
        private void Clustering()
        {
            double[,] distance = new double[n, k];
            double sum = 0;
            for (int clust = 0; clust < k; clust++)
            {
                for (int pr = 0; pr < n; pr++)
                {
                    for (int input = 0; input < m; input++)
                    {
                        sum += (prim[pr, input] - koef[clust, input]) * (prim[pr, input] - koef[clust, input]);
                    }
                    distance[pr, clust] = Math.Sqrt(sum);
                    sum = 0;
                }
            }

            rasstStr += "№\t";
            for (int clust = 0; clust < k; clust++)
                rasstStr += "R" + (clust + 1) + "\t";
            rasstStr += Environment.NewLine;
            for (int pr = 0; pr < n; pr++)
            {
                rasstStr += (pr + 1) + "\t";
                for (int clust = 0; clust < k; clust++)
                {
                    rasstStr += Math.Round(distance[pr, clust], 2) + "\t";
                }
                rasstStr += Environment.NewLine;
            }
            rasstStr += Environment.NewLine;

            //определение какой пример к какому кластеру относится
            for (int clust = 0; clust < k; clust++)
            {
                clustStr += "К " + (clust + 1) + "-ому кластеру относятся:";
                for (int pr = 0; pr < n; pr++)
                {
                    int min = 0;
                    for (int clust2 = 0; clust2 < k; clust2++)
                    {
                        if (distance[pr,clust2] < distance[pr,min])
                            min = clust2;
                    }
                    if (min == clust)
                        clustStr += " " + (pr + 1) + " ";
                }
                clustStr += Environment.NewLine;
            }

        }

        //обучение сети
        public void Teaching()
        {
            //Нормализация примеров
            NormalizePrim();
            //инициализация весов случайными числами
            InitRandom();

            for (int era = q; era <= qMax; era++)
            {
                //перемешивание примеров
                Shuffle();
                istStr += "/////////////////////////////////////" + Environment.NewLine;
                istStr += "Номер эпохи: " + era + Environment.NewLine;
                istStr += "Скорость обучения: " + v + Environment.NewLine;
                for (int pr = 0; pr < n; pr++)
                {
                    istStr += Environment.NewLine;
                    istStr += "Номер подаваемого примера: " + (sequence_prim[pr] + 1) + Environment.NewLine;
                    //подача примера
                    Calculation(sequence_prim[pr]);
                }
                //изменение скорости обучения
                ChangeSpeed();
            }
            koefStr = PrintKoef();
            //кластеризация
            Clustering();
        }
    }


    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
