using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericAl
{
    /// <summary>
    /// هسته محاسبات چیدن وزیر در صفحه شطرنج
    /// </summary>
    public class Vazir_Kernel
    {
        List<MainData> _data;
        List<ScaleAllData> _scale;
        List<CalculatedData> _calculatedDatas;

        int _Generation;
        int _JamiyatMax;
        int _BestFit;
        int _BoardSize;
        GA_Kernel Generic_Al;

        public Vazir_Kernel(List<MainData> data, List<ScaleAllData> scale, int JamiyatMax, int BestFit, int Generation, int BoardSize)
        {
            _data = data;
            _scale = scale;
            _JamiyatMax = JamiyatMax;
            _BestFit = BestFit;
            _Generation = Generation;
            _BoardSize = BoardSize;
            Generic_Al = new GA_Kernel(_data, _scale, _JamiyatMax, _BestFit, _Generation);
        }

        /// <summary>
        /// محاسبه اندازه یک کروموزوم
        /// این محاسبه در هر پروژه متفاوت است
        /// تمامی پارمتر های محاسباتی الگوریتم ژنتیک
        /// در این روتین محاسبه می شود
        /// </summary>
        /// <param name="Chromosome">کروموزوم</param>
        /// <returns>اندازه محاسبه شده</returns>
        public double CalculateFitnessChromosome(int[] Chromosome)
        {
            PrintBoard(Chromosome);
            int[] myval = (int[])Chromosome.Clone();
            int index = 0;
            int next = 0;
            double sum = 0;

            for (int x = 0; x < _BoardSize; x++)
            {
                for (int y = x + 1; y < _BoardSize; y++)
                {
                    //if (myval[sotoon] - myval[radif] == 0)
                }
            }
            return sum;
        }

        public List<CalculatedData> CalculateAllChromosome(List<CalculatedData> Population)
        {
            foreach (CalculatedData c in Population)
            {
                c.mutationRate = CalculateFitnessChromosome(c.Chromotion);
            }

            return Population;
        }

        public string CalculateAL()
        {
            List<CalculatedData> CreateFirstPopulation = new List<CalculatedData>();
            List<CalculatedData> LastGenerationPopulation = new List<CalculatedData>();
            CreateFirstPopulation = Generic_Al.InitializePopulation();
            //محاسبه مقدار  mutautionRate
            CreateFirstPopulation = CalculateAllChromosome(CreateFirstPopulation);
            //مرتب سازی از کوچکترین به بزرگترین
            CreateFirstPopulation = CreateFirstPopulation.OrderBy(v => v.mutationRate).ToList();


            CalculatedData BestChromosome = CreateFirstPopulation.First();
            LastGenerationPopulation = CreateFirstPopulation;
            Console.WriteLine("[" + 1.ToString() + "]- {" + Generic_Al.Create_Chromosome_String(BestChromosome.Chromotion) + "} = " + BestChromosome.mutationRate);
            int CounterLoop = 1;
            while (BestChromosome.mutationRate > _BestFit)
            {
                break;
                List<CalculatedData> GenerateNewPopulation = new List<CalculatedData>();

                for (int i = 0; i < LastGenerationPopulation.Count; i++)
                {
                    if (Generic_Al.isOdd(i))
                    {
                        List<int[]> co = new List<int[]>();
                        co = Generic_Al.CreateCrossOver(LastGenerationPopulation[i].Chromotion, LastGenerationPopulation[i - 1].Chromotion, 1);
                        if (co.Count > 0)
                        {
                            int ParentID = i;
                            foreach (int[] Chromosome in co)
                            {
                                CalculatedData calculatedData = new CalculatedData();
                                calculatedData.Chromotion = Chromosome;
                                calculatedData.strChromotion = Generic_Al.Create_Chromosome_String(Chromosome);
                                calculatedData.Parent = LastGenerationPopulation[ParentID].Chromotion;
                                calculatedData.strParent = Generic_Al.Create_Chromosome_String(LastGenerationPopulation[i].Chromotion);
                                calculatedData.mutationRate = CalculateFitnessChromosome(Chromosome);

                                GenerateNewPopulation.Add(calculatedData);
                                ParentID--;
                            }
                        }
                    }
                }

                GenerateNewPopulation = GenerateNewPopulation.OrderBy(v => v.mutationRate).ToList();
                LastGenerationPopulation = GenerateNewPopulation;
                BestChromosome = GenerateNewPopulation.First();
                Console.WriteLine("[" + CounterLoop.ToString() + "]- {" + Generic_Al.Create_Chromosome_String(BestChromosome.Chromotion) + "} = " + BestChromosome.mutationRate);
                CounterLoop++;

                if (CounterLoop == _Generation) break;
            }

            return "";
        }

        private void PrintBoard(int[] chromosome)
        {
            Console.WriteLine("javab = {" + String.Join(", ", chromosome) + "}");
            for (int i = 1; i <= _BoardSize; i++)
            {
                for (int j = 1; j <= _BoardSize; j++)
                {
                    Console.Write(chromosome[i-1] == j ? "Q " : "# ");
                }
                Console.WriteLine();
            }
        }
    }
}
