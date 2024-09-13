using GenericAl;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;


Console.WriteLine("Salam, Tedad Shahrharooo begoo  = ");
string Count = Console.ReadLine();

int MainDataCount = int.Parse(Count);
List<MainData> mainDatas = new List<MainData>();
for (int i = 1; i <= MainDataCount; i++)
{
    MainData md = new MainData();
    md.Id = i;
    md.Name = "Shahre_" + i.ToString();
    mainDatas.Add(md);

    Console.WriteLine(md.Name);

}


List<ScaleAllData> ListAllScale = new List<ScaleAllData>();

foreach (MainData md in mainDatas)
{
    var WithOutMD = mainDatas.Where(m => m.Id != md.Id).ToList();
    foreach (var m in WithOutMD)
    {

        var serach_Repeat1 = ListAllScale.Where(w => w.MainDataId == md.Id && w.MainDataIdNext == m.Id).ToList();
        var serach_Repeat2 = ListAllScale.Where(w => w.MainDataId == m.Id && w.MainDataIdNext == md.Id).ToList();

        if (serach_Repeat1.Count == 0 && serach_Repeat2.Count == 0)
        {
            Random rnd = new Random();
            int rInt = rnd.Next(10, 1000);
            ScaleAllData scaleAll = new ScaleAllData();
            scaleAll.MainDataId = md.Id;
            scaleAll.MainDataIdNext = m.Id;
            scaleAll.ScaleValue = rInt;

            ListAllScale.Add(scaleAll);
        }
    }
}
int CounterLoop = 0;
foreach (ScaleAllData item in ListAllScale)
{
    CounterLoop++;
    string ValName1 = mainDatas.Where(w => w.Id == item.MainDataId).FirstOrDefault().Name;
    string ValName2 = mainDatas.Where(w => w.Id == item.MainDataIdNext).FirstOrDefault().Name;
    Console.WriteLine(CounterLoop.ToString() + " - " + ValName1 + " ----- " + ValName2 + " = " + item.ScaleValue);
}

var al = new Kernel(mainDatas, ListAllScale, 100, 2000,50);
string a = al.CalculateAL();

Console.ReadKey();

