namespace MultiThread_02AsyncAwait
{
    internal class Program
    {
        static void Main(string[] args)//C# 버전 높은 버전은 진입점을 찾아줌
        {
            //await MakeBaristaToWork(); //호출
            Task<string> task = MakeBaristaToWork();
            task.Wait();

            Console.WriteLine($"고객님 오래 기다리셨습니다. \n 주문하신 " + task.Result + " 나왔습니다 \n");//결과 호출

            #region 첫 쓰레드 개선
            /*
             
            //테스크를 팩토리 패턴으로 만들어 쓴다.
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 10; i++) 
            {
                int tmpID = i;
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    HireBarista($"Baristat{tmpID}")//값의 주소를 참조하기 때문에 값이 변조되므로, 지역변수를 선언해서 사용
                    .GoToCafe("Luke's coffee")
                    .MakeCoffe().ToString();
                    Console.WriteLine(" ");
                }));
                tasks[tmpID].Wait();//tmpID 할때까지 기다려
                //비동기 루틴 

            }//위 작업이 완료될 때까지 기다리는 문법
            Task.WaitAll(tasks.ToArray());

             Task 만드는 구문
             */

            List<Task<Beverage>> baristaTasks = new List<Task<Beverage>>();
            Barista barista = HireBarista("Super Barista");
            for (int i = 0; i < 10; i++)
            {
                baristaTasks.Add(barista.MakeCoffe());
            }

            Task.WaitAll(baristaTasks.ToArray());
            for (int i = 0; i < baristaTasks.Count; i++)
            {
                Console.WriteLine($"{i}번째 고객님 오래 기다리셨습니다. \n 주문하신 " + baristaTasks[i].Result + " 나왔습니다 \n");
            }
         
        }

        //Task 만드는 구문 대신에 Async를 만들어 사용
        static async Task<string> MakeBaristaToWork()
        {
            //간략
            Beverage result = await HireBarista($"Baristat")
                                    .GoToCafe("Luke's coffee")
                                    .MakeCoffe();
            return result.ToString();
        }
        #endregion


        static Barista HireBarista(string nickname)//고용하는 함수
        {
            Barista barista = new Barista(nickname);
            return barista;
        }
    }
    public enum Beverage
    {
        Aspresso,
        Tea,
        Americano,
    }

    public class Barista
    {
        public Barista(string name)//생성자
        {
            this.name = name;
        }
        public string name { get; private set; }
        private Random random = new Random();

        //체이닝 함수 만들기 반환형을 현재 class
        public Barista GoToCafe(string cafeName)//카페 가기
        {
            Console.WriteLine($"바리스타 {name}(이)가 {cafeName}으로 출근중입니다");
            return this;//내 자신을 반환하기
        }
        public async Task<Beverage> MakeCoffe() //커피 내리기: 비동기 명시하기 위해 async
        {
            Console.WriteLine($"바리스타 {name}(이)가 커피(을)를 추출을 시작합니다...");
            await Task.Delay(1000); //3초동안 잠듦 //Thread.Sleep대신 await Task Delay로 쓰게 되면
            //01에서 Main에 작성했던
            //wait  Task<T> name = new Task<T>(() => 
            //{
            //      HireBarista($"Baristat{tmpID}")//값의 주소를 참조하기 때문에 값이 변조되므로, 지역변수를 선언해서 사용
            //      .GoToCafe("Luke's coffee")
            //      .MakeCoffe().ToString();
            //      Console.WriteLine(" ");
            //}
            //name.Start();
            //name.Wait();
            //까지 생략
            Console.WriteLine($"추출 중...");
            /*
             
            Task task1 = new Task(() =>
            {
                Thread.Sleep(1000);

            });
            task1.Start();
            task1.Wait();
            */
            await Task.Delay(2000); //3초동안 잠듦
            Console.WriteLine($"바리스타 {name}(이)가 커피(을)를 추출을 완료했습니다...");
            return (Beverage)random.Next(0, Enum.GetValues(typeof(Beverage)).Length);//0~Beverage범위까지 Random 뽑은 것을 이넘Beverage캐스팅해서 음료 반환
        }
    }
}
