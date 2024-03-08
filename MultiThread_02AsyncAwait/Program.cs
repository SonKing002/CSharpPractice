using System.Security.Cryptography.X509Certificates;

namespace MultiThread_02AsyncAwait
{
    internal class Program
    {
        public static int BeverageCount;
        public static object lockCount = new object();
        static void Main(string[] args)//C# 버전 높은 버전은 진입점을 찾아줌
        {

            List<Task<string>> tasks = new List<Task<string>>();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(MakeBaristaToWork());
            }
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine(BeverageCount);
            
            return;

            //await MakeBaristaToWork(); //호출
            Task<string> task = MakeBaristaToWork();
            task.Wait();

            Console.WriteLine($"고객님 오래 기다리셨습니다. \n 주문하신 " + task.Result + " 나왔습니다 \n");//결과 호출

            #region 쓰레드 개선
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

            Task.WaitAll(baristaTasks.ToArray());//대기

            Console.WriteLine(BeverageCount);
            for (int i = 0; i < baristaTasks.Count; i++)
            {
                Console.WriteLine($"{i}번째 고객님 오래 기다리셨습니다. \n 주문하신 " + baristaTasks[i].Result + " 나왔습니다 \n");
            }
         
        }
        #endregion
        //Task 만드는 구문 대신에 Async를 만들어 사용
        static async Task<string> MakeBaristaToWork() //async함수를 호출하는 순간 Stack영역이 만들어짐 
        {
            //간략
            Beverage result = await HireBarista($"Baristat")
                                    .GoToCafe("Luke's coffee")
                                    .MakeCoffe();//얘는 락이 안걸려있다. 여기서 Count++하면, race Condition

            #region Lock Monitor
            //Monitor.Enter(object obj); obj Internal Call 가상머신에서 다루기 떄문에 어떻게 다뤄지는지 알 수 없으므로

            /*
            Monitor.Enter(lockCount);
            //이 영역을 임계영역이라고 부름 : Critical Section
            Monitor.Exit(lockCount);
            */

            lock (lockCount)//if문처럼 사용
            {
                for (int i = 0; i < 10000; i++)//한명당 10000개
                {
                    BeverageCount++;//동시접근해서 race Condition으로 누적되지 않은 현상이 발생 -> Lock걸기
                }
            }//어플리케이션 내에서 사용하는 문법
            return result.ToString();
            #endregion
        }

        /*
         * 
         * HireBarista(" "); -> return ; 반환 : 값을 넘겨준다, stack 영역 반환 == 시스템( 이 프로그램을 실행할 때 필요한==체제 )에게 제어권을 반환하겠다. //os 기본운영체계
         * MakeBaritaTo Work();
         */

        #region 수학기초
        // 데이터 영역 = 함수에서 연산에 필요한 데이터들으로 지역변수를 말함 nickName, barista

        //함수 뜻?
        //f(x) =  ax + b ;
        // f     : 함수이름
        // ( x ) : 매개 변수, 매개체를 통해 전달되는 인자 // 스택에 쌓이는 지역변수
        //ax + b : 코드블록 내용
        // =     : 반환

        //개발하기 위해서는 더 나아가서 수학을 알아야함 백터의 내적? 
        // 시야각 계산.
        //움직이는 축과 키보드의 축은 다름.

        //스칼라, 벡터 >

        //변위, 거리
        //A-B 거리
        //0,0,0 동쪽 x+  2,0,0 만큼 떨어져있따. 변위

        //속도, 속력
        //speed 50 속력
        //동쪽으로 speed 50 속도

        //Transform.position 원점 기준 얼마나 떨어졌는지. == 벡터개념

        //벡터 간의 덧셈뺄셈 연산 : 방향성 막대기 크기를 이어준다.이때 처음지점 ~ 마지막 지점을 이어준 막대기
        //1. Transform.position -> 동쪽으로 2m가는 벡터 
        //동북쪽으로 + ->  3m가는 벡터
        //우상향 
        //2. Transform.position -> 동쪽으로 2m가는 벡터 
        //동북쪽으로 - 간다 -> 3m가는 벡터
        //우하향 

        //벡터의 곱셈 >
        // 1. 한축 기준으로 곱 내적 Dot Product  중간 점 기호

        //  (A축 성분 기준으로 B에서의 A으로 직선으로 내린 후) : 닿는 위치까지의 막대기를 곱해준다. 한축만 고려했기 때문에 == 결과값이 스칼라 //방향은 필요없으므로 절대값을 넣어준다.
        // Cos 공식 = 밑면 / 빗면  이므로  
        // 내적 곱 = ( 밑면 = ｜빗면｜ * cos각 )  * ｜기준 A｜  

        // 사이 각도를 구해보자
        // ( 내적곱 ) / (｜빗면｜*｜기준A｜) = cos각
        // 각도 = cos ∧-1 * (  내적곱 / (｜빗면｜*｜기준A｜)  )  // cos∧-1 = Acos

        // 2. 모든 축 기준 곱  외적 Cross  x 곱셈기호

        // 단위벡터 : 방향만 가르키는 벡터 크기는 1으로 고정
        // 곱셈 == 법선벡터의 방향을 의미

        // "어떠한 평면에 대한 수직인 벡터 "( 방향을 곱하는 것 ) x축 i , y축 j , z축 k 
        // i * j 해당 면의 법선벡터는 k
        // i * k 해당 면의 법선벡터는 j

        // 앞면 뒷면 렌더가 다르다. BackfaceCulling을 하지 않으면 뒷면을 그리지 않는다. 벡터 간의 곱셈을 하는것이다.
        //외적 곱, 앞뒤 판단 
        // (x1,y1,z1) (x2,y2,z2)
        //  (x1 * x2 * i^2) + (y1 * y2 * j^2) + (z1 * z2 * z^2)   == 같은 방향 == 0
        //  (x1 * y2 * i * j) + (x1 * z2 * i * k) +
        //  (y1 * x2 * i * j) + (y1 * z2 * j * k) +
        //  (z1 * x2 * i * k) + (z1 * y2 * j * k) +
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
