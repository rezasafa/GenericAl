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
    public class Kernel
    {
        List<MainData> _data;
        List<ScaleAllData> _scale;
        List<CalculatedData> _calculatedDatas;

        int _JamiyatMax;
        int _BestFit;
        List<int[]> Jamiyat;
        public Kernel(List<MainData> data, List<ScaleAllData> scale,int JamiyatMax,int BestFit)
        {
            _data = data;
            _scale = scale;
            _JamiyatMax = JamiyatMax;
            _BestFit = BestFit;
        }


        public List<int[]> InitializePopulation()
        {

            for (int i = 1; i<= _JamiyatMax; i++)
            {

            }
            return Jamiyat;
        }

        public string CalculateAL()
        {

            int[] myval = ConvertDataListToChromosome();
            string result = "";

            result += "\n" + "parent = " + Create_Chromosome_String(myval);
            myval = ConvertDataListToChromosome();
            result += "\n" + "swap = " + Create_Chromosome_String(CreateNewSwap(myval));
            result += "\n" + "swap2 = " + Create_Chromosome_String(CreateNewSwap(myval));
            myval = ConvertDataListToChromosome();
            result += "\n" + "invertion = " + Create_Chromosome_String(CreateNewInvertion(myval));
            result += "\n" + "insertion = " + Create_Chromosome_String(CreateNewInsertion(myval));

            return result;
        }

        /// <summary>
        /// یک کروموزوم کراس اور شده را 
        /// بررسی میکند که آیا ژن های آن تکراری 
        /// ویا نسبت به والدش کم و کسری نداشته باشد 
        /// </summary>
        /// <param name="Parent">والد</param>
        /// <param name="child">کروموزوم کراس اور شده</param>
        /// <returns>کروموزوم سالم</returns>
        public int[] CorrectCrossOverChromosome(int[] Parent, int[] child)
        {
            bool findRepeat = true;
            int[] array = (int[])child.Clone();
            int[] result = (int[])child.Clone();
            
            while (findRepeat)
            {
                var dict = new Dictionary<int, int>();
                List<int> RepeatValue = new List<int>();
                List<int> NotExistValue = new List<int>();

                //آنهایی که در فرزند وجود ندارد را پیدا  میکند
                IEnumerable<int> aOnlyNumbers = Parent.Except(child);
                Console.WriteLine("Numbers in first array but not second array:");
                foreach (var n in aOnlyNumbers)
                {
                    NotExistValue.Add(n);
                    //Console.WriteLine(n);
                }

                //تکراری ها را پیدا میکند
                foreach (var value in array)
                {
                    // When the key is not found, "count" will be initialized to 0
                    dict.TryGetValue(value, out int count);
                    dict[value] = count + 1;
                }
                //تکراری ها را به لیست اضافه میکند
                foreach (var pair in dict)
                {
                    //Console.WriteLine("Value {0} occurred {1} times.", pair.Key, pair.Value);

                    if (pair.Value > 1)
                    {
                        findRepeat = true;
                        RepeatValue.Add(pair.Key);
                    }
                }

                if (RepeatValue.Count > 0 && NotExistValue.Count > 0)
                {
                    int loopIndex = 0;
                    foreach (int item in RepeatValue)
                    {
                        for (int i = Parent.Length - 1; i > -1; i--)
                        {
                            if (result[i] == item)
                            {
                                result[i] = NotExistValue[loopIndex];
                                loopIndex++;
                                break;
                            };

                        }
                    }
                }
                else
                {
                    findRepeat = false;
                }

                array = result;

            }

            return result;
        }
        /// <summary>
        /// عمل کراس اور یا ادغام دو تا کروموزم انجام می دهد
        /// </summary>
        /// <param name="Parent1">پدر</param>
        /// <param name="Parent2">مادر</param>
        /// <param name="minRandom">حداقل شروع عدد تصادفی</param>
        /// <returns>دو کروموزوم بر میگرداند</returns>
        public List<int[]> CreateCrossOver(int[] Parent1, int[] Parent2, int minRandom)
        {

            Random rnd = new Random();
            int CrossPoint = rnd.Next(minRandom, Parent1.Length/2);

            int[] copyParent1 = (int[])Parent1.Clone();
            int[] copyParent2 = (int[])Parent2.Clone();
            int[] child1 = new int[copyParent1.Length];
            int[] child2 = new int[copyParent2.Length];

            for (int i = 0; i < CrossPoint; i++)
            {
                child1[i] = copyParent1[i];
                child2[i] = copyParent2[i];
            }

            for (int i = CrossPoint; i < copyParent1.Length; i++)
            {
                child1[i] = copyParent2[i];
                child2[i] = copyParent1[i];
            }

            child1 = CorrectCrossOverChromosome(copyParent1, child1);
            child2 = CorrectCrossOverChromosome(copyParent2, child2);

            List<int[]> mychild = new List<int[]>();
            mychild.Add(child1);
            mychild.Add(child2);
            return mychild;

        }
        /// <summary>
        /// عمل سوآپ روی یک کرموزوم انجام میدهد
        /// نقطه اول را با نقطه دوم جابجا میکند
        /// نقاط به صورت تصادفی بدست می آید
        /// </summary>
        /// <param name="Chromosome">یک کروموزوم</param>
        /// <returns>یک کروموزوم برمیگرداند</returns>
        public int[] CreateNewSwap(int[] Chromosome)
        {
            int[] Result = (int[])Chromosome.Clone();
            Random rnd = new Random();
            int a = rnd.Next(GetMinIndex(), GetMaxIndex());
            int b = rnd.Next(GetMinIndex(), GetMaxIndex());

            int val_a = Result[a];
            int val_b = Result[b];

            Result[a] = val_b;
            Result[b] = val_a;

            return Result;
        }
        /// <summary>
        /// عمل اینورشن انجام می دهد
        /// حدفاصل نقطه شرو و نقطه پایان را معکوس میکند
        /// نقاط شروع و پایان تصادفی بدست می اید
        /// </summary>
        /// <param name="Chromosome">کروموزوم</param>
        /// <returns>یک کروموزم برمیگرداند</returns>
        public int[] CreateNewInvertion(int[] Chromosome)
        {
            int[] Result = (int[])Chromosome.Clone(); ;
            Random rnd = new Random();
            int a = rnd.Next(GetMinIndex(), GetMaxIndex());
            int b = rnd.Next(GetMinIndex(), GetMaxIndex());
            int max = 0;
            int min = 0;
            if (a > b)
            {
                max = a;
                min = b;
            }
            else
            {
                max = b;
                min = a;
            }

            int Ekhtelaf = max - min;
            while (isEven(Ekhtelaf) || (Ekhtelaf < 3))
            {
                a = rnd.Next(GetMinIndex(), GetMaxIndex());
                b = rnd.Next(GetMinIndex(), GetMaxIndex());
                if (a > b)
                {
                    max = a;
                    min = b;
                }
                else
                {
                    max = b;
                    min = a;
                }
                Ekhtelaf = max - min;
            }

            int j = max;
            for (int i = min; i <= max; i++)
            {
                Result[i] = Chromosome[j];
                Result[j] = Chromosome[i];
                j -= 1;
            }

            return Result;
        }
        /// <summary>
        /// عمل اینزرشن را انجام می دهد
        /// نقطه شروع را به بعد از نقطه پایان انتقال می دهد
        /// همان کات کردن 
        /// نقطه شروع و پایان تصادفی بدست می آید
        /// </summary>
        /// <param name="Chromosome">کروموزوم</param>
        /// <returns>یک کروموزوم</returns>
        public int[] CreateNewInsertion(int[] Chromosome)
        {
            int[] Result = (int[])Chromosome.Clone();
            Random rnd = new Random();
            int a = rnd.Next(GetMinIndex(), GetMaxIndex());
            int b = rnd.Next(GetMinIndex(), GetMaxIndex());
            int max = 0;
            int min = 0;
            if (a > b)
            {
                max = a;
                min = b;
            }
            else
            {
                max = b;
                min = a;
            }

            int Ekhtelaf = max - min;
            while (Ekhtelaf < 1)
            {
                a = rnd.Next(GetMinIndex(), GetMaxIndex());
                b = rnd.Next(GetMinIndex(), GetMaxIndex());
                if (a > b)
                {
                    max = a;
                    min = b;
                }
                else
                {
                    max = b;
                    min = a;
                }
                Ekhtelaf = max - min;
            }

            int start = Result[a];
            Result = DeleteItemInArray(Result, start);
            Result = InsertItemInArray(Result, b, start);

            return Result;
        }

        /// <summary>
        /// کوچکترین مقدار کروموزوم را بدست می آورد
        /// </summary>
        /// <returns>عدد</returns>
        private int GetMinIndex()
        {
            var min = _data.OrderBy(m => m.Id).First();
            return min.Id;
        }
        /// <summary>
        /// بزرگترین مقدار کروموزوم را بدست می آورد
        /// </summary>
        /// <returns>عدد</returns>
        private int GetMaxIndex()
        {
            var min = _data.OrderBy(m => m.Id).Last();
            return min.Id;
        }
        
        /// <summary>
        /// تبدیل یک لیست به کروموزوم
        /// </summary>
        /// <returns>کرموزوم</returns>
        public int[] ConvertDataListToChromosome()
        {
            _data = _data.OrderBy(w => w.Id).ToList();
            int[] list = new int[_data.Count];
            int index = 0;
            foreach (MainData MD in _data)
            {
                list[index] = MD.Id;
                index += 1;
            }

            return list;
        }
        /// <summary>
        /// تشخیص میدهد که عدد زوج است
        /// </summary>
        /// <param name="number">عد ورودی</param>
        /// <returns>درست یعنی زوج</returns>
        private static bool isEven(int number)
        {
            return (number % 2 == 0);
        }
        /// <summary>
        /// تشخیص میدهد که عدد فرد است
        /// </summary>
        /// <param name="number">عد ورودی</param>
        /// <returns>درست یعنی فرد</returns>
        private static bool isOdd(int number)
        {
            return (number % 2 != 0);
        }
        /// <summary>
        /// تبدین یک کروموزوم به متن
        /// </summary>
        /// <param name="Chromosome">کروموزوم</param>
        /// <returns>متن</returns>
        public string Create_Chromosome_String(int[] Chromosome)
        {
            return string.Join(", ", Chromosome);
        }
        /// <summary>
        /// اضافه کردن یک ژن به کروموزوم
        /// </summary>
        /// <param name="Chromosome">کروموزوم</param>
        /// <param name="Index">نقطه که باید ژن اضافه شود</param>
        /// <param name="Values">ژن</param>
        /// <returns>کروموزوم</returns>
        private int[] InsertItemInArray(int[] Chromosome, int Index, int Values)
        {
            int[] myArray = (int[])Chromosome.Clone();
            int indexToInsert = Index;
            int elementToInsert = Values;

            Array.Resize(ref myArray, myArray.Length + 1); // increase array length by 1
            Array.Copy(myArray, indexToInsert, myArray, indexToInsert + 1, myArray.Length - indexToInsert - 1); // shift elements right from the index
            myArray[indexToInsert] = elementToInsert; // insert element at the index

            return myArray;
        }
        /// <summary>
        /// حذف یک ژن از کروموزوم
        /// </summary>
        /// <param name="Chromosome">کروموزوم</param>
        /// <param name="Values">ژن</param>
        /// <returns>کروموزوم</returns>
        private int[] DeleteItemInArray(int[] Chromosome, int Values)
        {
            int[] numbers = (int[])Chromosome.Clone();
            int numToRemove = Values;
            numbers = numbers.Where(val => val != numToRemove).ToArray();

            return numbers;
        }
    }

    public class MainData
    {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Vazn { get; set; }

        public ICollection<ScaleAllData> ScaleAllDatas { get; set; } = new List<ScaleAllData>();

    }

    public class ScaleAllData
    {
        [Key]
        public int Id { get; set; }

        public int MainDataId { get; set; }
        public MainData MainData { get; set; } = null;

        public int MainDataIdNext { get; set; }

        public int ScaleValue { get; set; }

    }

    public class CalculatedData
    {
        public int[] Parent { get; set; }
        public string strParent { get; set; }

        public int[] Chromotion { get; set; }
        public string strChromotion { get; set; }

        public double mutationRate { get; set; }
    }


}
