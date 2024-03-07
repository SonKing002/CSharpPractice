using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace MultiThread_EX
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            /*
            switch (grade)
            {
                case WeaponGrade.Normal:
                    Console.WriteLine($"흔해 보이는");
                    break;
                case WeaponGrade.Rare:
                    Console.WriteLine($"땀을 흘리며 작업을 계속합니다");
                    break;
                case WeaponGrade.SuperRare:
                    break;
                case WeaponGrade.LegendUnique:
                    break;
                default:
                    break;
            }
            */
        }

        static async Task<string> RefineIngredient()
        {
            WeaponGrade result = await HireCrefter("난쟁이")
                .WhichCampGoToWork("엘프숲")
                .CreateWeaponGrade();
            Console.WriteLine("정제되었습니다...");
            return result.ToString();
        }

        static WeaponCrefter HireCrefter(string nickName)//대장장이 고용
        {
            WeaponCrefter weaponCrefter = new WeaponCrefter(nickName);
            return weaponCrefter;
        }
    }

    public enum WeaponGrade
    { 
        Normal,
        Rare,
        SuperRare,
        LegendUnique,
    }
    public enum WeaponType
    { 
        Sword,
        Bow,
        Spear,
        Wand,
        Gun,
    }

    public class WeaponCrefter()
    {
        public WeaponCrefter(string name)
        {
            this.name = name;
        }


        public string name { get; private set; }
        private Random randomGrade = new Random();
        private Random randomTyoe = new Random();


        public WeaponCrefter WhichCampGoToWork(string shopName)
        {
            Console.WriteLine($"{shopName}의 대장장이가 모습을 보입니다");
            return this;
        }

        public async Task<WeaponGrade> CreateWeaponGrade()// 명시 Async
        {
            Console.WriteLine("망치를 내리칩니다...");
            await Task.Delay(1000);
            Console.WriteLine("...깡!");
            await Task.Delay(500);
            Console.WriteLine("...깡!");
            await Task.Delay(500);
            return (WeaponGrade)randomGrade.Next(0, Enum.GetValues(typeof(WeaponGrade)).Length);
        }

        public async Task<WeaponType> CreateWeapon()
        {
            Console.WriteLine("깊은 숨을 들이쉬며...");
            await Task.Delay(1000);
            Console.WriteLine($"{name} 대장장이가 물건을 가져옵니다.");
            return (WeaponType)randomTyoe.Next(0, Enum.GetValues(typeof(WeaponType)).Length);
        }
    }
}
