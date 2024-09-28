using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace GenericAl
{
    /// <summary>
    /// هسته محاسبات فروشنده دوره گرد
    /// </summary>
    public class ForoshandehDorehGard_Kernel
    {
        List<MainData> _data;
        List<ScaleAllData> _scale;
        List<CalculatedData> _calculatedDatas;

        int _Generation;
        int _JamiyatMax;
        int _BestFit;

        GA_Kernel Generic_Al;

        public ForoshandehDorehGard_Kernel(List<MainData> data, List<ScaleAllData> scale, int JamiyatMax, int BestFit,int Generation)
        {
            _data = data;
            _scale = scale;
            _JamiyatMax = JamiyatMax;
            _BestFit = BestFit;
            _Generation = Generation;

            Generic_Al = new GA_Kernel(_data,_scale,_JamiyatMax,_BestFit,_Generation);
        }
        
        /// <summary>
        /// محاسبه اندازه یک کروموزوم
        /// این محاسبه در هر پروژه متفاوت است
        /// تمامی پارمتر های محاسباتی الگوریتم ژنتیک
        /// در این روتین محاسبه می شود
        /// </summary>
        /// <param name="Chromosome">کروموزوم</param>
        /// <param name="scales">لیستی از اندازه ها</param>
        /// <returns>اندازه محاسبه شده</returns>
        public double CalculateFitnessChromosome(int[] Chromosome, List<ScaleAllData> scales)
        {
            int[] myval = (int[])Chromosome.Clone();
            int index = 0;
            int next = 0;
            double sum = 0;
            while (index < Chromosome.Length)
            {

                next = index + 1;
                if (next < Chromosome.Length)
                {
                    var find = scales.Where(v =>
                        ((v.MainDataId == myval[index]) || (v.MainDataIdNext == myval[index]))
                        &&
                        ((v.MainDataId == myval[next]) || (v.MainDataIdNext == myval[next]))
                    );
                    if (find != null)
                        if (find.Count() > 0)
                            sum += find.First().ScaleValue;

                }



                index++;
            }
            return sum;
        }

        public List<CalculatedData> CalculateAllChromosome(List<CalculatedData> Population)
        {
            foreach(CalculatedData c in Population)
            {
                c.mutationRate = CalculateFitnessChromosome(c.Chromotion,_scale);
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
                                calculatedData.mutationRate = CalculateFitnessChromosome(Chromosome,_scale);

                                GenerateNewPopulation.Add(calculatedData);
                                ParentID--;
                            }
                        }
                    }
                }

                GenerateNewPopulation = GenerateNewPopulation.OrderBy(v => v.mutationRate).ToList();
                LastGenerationPopulation = GenerateNewPopulation;
                BestChromosome = GenerateNewPopulation.First();
                Console.WriteLine("[" + CounterLoop.ToString() + "]- {" + Generic_Al. Create_Chromosome_String(BestChromosome.Chromotion) + "} = " + BestChromosome.mutationRate);
                CounterLoop++;

                if (CounterLoop == _Generation) break;
            }

            return "";
        }  

    }
}

