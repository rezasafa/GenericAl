using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericAl
{
    /// <summary>
    /// این کلاس اسمی برای یک ژن از یک کروموزوم در دیتابیس یا حافظه می سازد
    /// مثلا در الگوریتم فروشنده دوره گرد اسم شهر ها نگه می دارد
    /// {1,2,3,4,5,6} 
    /// هر آرایه برایبر اسا با یک کروموزوم
    /// هر عضو از آرایه یک ژن است
    /// </summary>
    public class MainData
    {
        //آیدی ژن است که این خود یک ژن است
        [Key]
        public int Id { get; set; }
        //نام یک ژن است
        public string Name { get; set; }

        public ICollection<ScaleAllData> ScaleAllDatas { get; set; } = new List<ScaleAllData>();

    }
    /// <summary>
    /// این کلاس شناسه مقدار بین دوتا زن را نگهداری می کند
    /// در اینجا شناسه ژن ها نگهداری می کنیم
    /// به عوان مثال در الگوریتم ژنتیک فصله شهر 1 تا شهر دورا نگهداری می کند
    /// </summary>
    public class ScaleAllData
    {
        [Key]
        public int Id { get; set; }
        // شناسه یک ژن
        public int MainDataId { get; set; }
        public MainData MainData { get; set; } = null;
        // شناسه ژن بعدی
        public int MainDataIdNext { get; set; }
        // مقدار دوتازن
        public int ScaleValue { get; set; }

    }

    /// <summary>
    /// نتیجه محاسبه یک کروموزوم را نگهداری می کند
    /// و این کروموزوم از کدام کروموزوم دیگر تولید شده است
    /// </summary>
    public class CalculatedData
    {
        //کروموزوم اصلی کهاز آن کروموزوم این کلاس ایجاد شده است
        public int[] Parent { get; set; }
        // نمایش متنی یک کروموزوم اصلی
        public string strParent { get; set; }
        //یک کروموزوم
        public int[] Chromotion { get; set; }
        // نمایش متنی یک کروموزوم ایجاد شده
        public string strChromotion { get; set; }
        //نتیجه محاسبه یک کروموزوم
        public double mutationRate { get; set; }
    }


}
