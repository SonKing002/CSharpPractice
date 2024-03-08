namespace MultiThread_EX
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool isAnswer = false;
            int count = 3;
            while (isAnswer == false)
            {
                Console.WriteLine("\"신원 조회합니다 성함이 어떻게 되십니까? \"");

                string yourName = Console.ReadLine();
                isAnswer = (string.IsNullOrEmpty(yourName) == false);
                if (isAnswer)
                {
                    Console.WriteLine($" \" 고생하십니다 \" ");
                    break;
                }
                else
                {
                    Console.WriteLine("\"신원이 조회되지 않습니다 다시 말씀해주세요\"");
                    count--;
                    Console.Clear();

                    if (count == 0)
                    {
                        Console.WriteLine("\" 신원불명합니다. 연행하겠습니다 \"");
                        return;
                    }
                }
            }

            Task<string> task = RefineIngredient();
            task.Wait();
            Console.WriteLine($"\"어떠냐! {task.Result}한 느낌 있지!?\"");

            isAnswer = false;
            count = 3;
            while (isAnswer == false)
            { 
                string answer = Console.ReadLine();
                isAnswer = (string.IsNullOrEmpty(answer) == false);
                if (isAnswer)
                {
                    Console.Clear();
                    Console.WriteLine($" \" 그래!! 이어서 진행하도록 하지!! \" ");
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\"응 그래서, 어떤가?!\"");
                    count--;

                    if (count == 0)
                    {
                        Console.WriteLine("\" 내키지가 않네 저리가게! \"");
                        return;
                    }
                }
            }

        task = MakeWeapon();
            task.Wait();
            Console.WriteLine($"\"가저가거라! {task.Result} 완성했다\"");



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
        }//Main

        static WeaponCrefter HireCrefter(string nickName)//대장장이 고용
        {
            WeaponCrefter weaponCrefter = new WeaponCrefter(nickName);
            return weaponCrefter;
        }

        static async Task<string> RefineIngredient()
        {
            WeaponGrade result = await HireCrefter("난쟁이")
                .InthePlace("엘프숲")
                .CreateWeaponGrade();
            Console.WriteLine("정제되었습니다...");
            return result.ToString();
        }
        static async Task<string> MakeWeapon()
        {
            WeaponType result = await HireCrefter("난쟁이")
                .CreateWeapon();
            Console.WriteLine("물건을 가져옵니다.");
            return result.ToString();
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

    public class WeaponCrefter
    {
        public WeaponCrefter(string name)
        {
            this.name = name;
        }

        public string name { get; private set; }
        private Random randomGrade = new Random();
        private Random randomTyoe = new Random();

        public WeaponCrefter InthePlace(string shopName)
        {
            Console.WriteLine($"{shopName}가게 안 쪽에서 소리가 들립니다");
            Task.Delay(1000);
            Console.WriteLine($"\"손님이군!\"");
            return this;
        }

        public async Task<WeaponGrade> CreateWeaponGrade()// 명시 Async
        {
            await Task.Delay(1000);
            Console.WriteLine("망치를 내리칩니다...");
            await Task.Delay(500);
            Console.WriteLine("...깡!");
            await Task.Delay(500);
            Console.WriteLine("...깡!");
            await Task.Delay(1000);
            Console.WriteLine("담금질을 합니다...");
            await Task.Delay(500);
            Console.WriteLine("취이...");
            await Task.Delay(500);
            Console.WriteLine("취이...");
            await Task.Delay(500);
            Console.WriteLine("\"음! 어디 한번!\"");
            return (WeaponGrade)randomGrade.Next(0, Enum.GetValues(typeof(WeaponGrade)).Length);
        }

        public async Task<WeaponType> CreateWeapon()
        {
            await Task.Delay(1000);
            Console.WriteLine("무기를 제작합니다");
            await Task.Delay(1000);
            Console.WriteLine("망치를 내리칩니다...");
            await Task.Delay(500);
            Console.WriteLine("...깡!");
            await Task.Delay(500);
            Console.WriteLine("...깡!");
            await Task.Delay(1000);
            Console.WriteLine("담금질을 합니다...");
            await Task.Delay(500);
            Console.WriteLine("취이...");
            await Task.Delay(500);
            Console.WriteLine("취이...");
            await Task.Delay(500);
            Console.WriteLine($"{name} 대장장이가 무기를 완성합니다.");
            return (WeaponType)randomTyoe.Next(0, Enum.GetValues(typeof(WeaponType)).Length);
        }


        public void Ex()
        {

            //List IEnumerable 사용, 순회 가능한
            List<int> list = new List<int>();
            using (IEnumerator<int> e = list.GetEnumerator())
            {
                while (e.MoveNext())//다음으로
                {
                    Console.WriteLine(e.Current);//현재Current
                }
                e.Reset();//초기화

            }// Dispose자동 호출

            //자주 사용하니까 위를 아래처럼 간략
            foreach (int i in list)
            { 
            
            }
        }

        
        public async Task Nothing()
        {

        }

        public async Task<string> Nothung2()
        {
            return "fds";
        }
    }


    
}
