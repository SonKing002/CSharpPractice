using System;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Threading;//쓰레딩으 사용하기 위해 네임스페이스 등록

namespace MultiThread_01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            //기본 == 메인 쓰레드
            //생성하기
            Thread baristaThread1 = new Thread(() =>
                                                    {
                                                        HireBarista("Carl")
                                                            .GoToCafe("Luke's coffee")
                                                            .MakeCoffe();
                                                        //체이닝 없이 사용한다면
                                                        //Barista barista = HireBarista("Carl");
                                                        //barista.GoToCafe("Luke's coffee");
                                                        //barista.MakeCoffee();
                                                    },
                                                    1024 * 1024 / 2);
            //작업 쓰레드 생성 : Carl 고용. Luke's coffee출근
            //작업 시작하기
            baristaThread1.Name = "Barista Thread 1";// 디버깅을 위해서 이름 지정 가능
            baristaThread1.IsBackground = true;//배경으로 돌릴 쓰레드 (false 메인이 끝나든 말든 내꺼 하는 것)//true 메인 종료시 다 종료
            

            Thread.Sleep(1000);
            Console.WriteLine("주문하신 커피가 나왔습니다");

            
            ThreadPool.SetMinThreads(1, 0);//주작업 쓰레드 WorkerThreads CPU Bound Thread == 연산이 많음, CompletionPortThread IO Bound Thread == 
                                           //CPU 성능에 맞게 처리하기 떄문에, 코어당 한개 씩 코어수 만큼 할당하여 사용한다.
                                           //CompletionPortThread IO Bound Thread  작업 완료될때까지 기다렸다가, 끝내는 ex. 통신 : 요청 기다림

            //할당된 쓰레드가 기다리는 역할 : 다른 작업 못함 == 조그만한 것을 뗴어내서 대신 기다림
            //현 쓰레드에서 연산이 많은 것들을 동시에 처리하고, 체크할 수 있도록 하기 위함
            //ex 코어 4 : 4개, 돌리면서 대기명령도 동시에 돌림,//호출할때마다 바뀌면, 오버헤드 일어난다.
            ThreadPool.SetMaxThreads(4, 8);
            //그래서 Pool을 만들어준다.

            //고정적으로 필요한 경우에 new Thread로 생성하고, 이외 변동되는 것들은
            //테스크 클래스를 통해서 : ThreadPool 에서 Thread 를 빌려다가 작업을 할당하는 클래스
            Task task1 = new Task(() =>
            {
                HireBarista("Carl")
                .GoToCafe("Luke's coffee")
                .MakeCoffe();
            });
            //쓰래드 풀에서 가져다 쓴다.
            baristaThread1.Start();
            baristaThread1.Join();//호출한 쓰레드(바리스타 명령이 끝나는 것)을 기다린 다음,

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


            //싱글쓰레드 과정은 작업자 하나가 처리했으니 논리적으로 순서가 맞았다.
            //Coroutine은 yield return하면 순서 기다림. 싱글쓰레드라서 다른 작업 없이도, 멀티처럼 순서를 지정해주지 않아도 된다.

            //하지만,
            //내부의 알고리즘되어있는데, 스케쥴러가 순서를 결정하기 떄문에
            //확률은 있지만, 0~9까지 내가 먼저 할당시킨 순서를 지켜주지 않는다. 

            //작업에 대한 결과를 통지 받고 싶을 때, 방법 
            //서빙 해야하니까, 커피 내린 것을 받아볼 수 있어야 한다.
            //Generic Type : 작업의 결과물을 반환받고 싶다면 Task<string>
            Task<string> taskWithResult = new Task<string>(() =>
            {
                //같은 내용을 실행할 건데
                
                return//문자열 반환
                HireBarista("$smart_Barista")
                .GoToCafe("Luke's coffee")
                .MakeCoffe().ToString();//랜덤 음료를 내준다.
            });
            taskWithResult.Start();
            taskWithResult.Wait();
            Console.WriteLine(taskWithResult.Result);
            //foreach문 IEnumerable 에서 Enumerator -> GetEnumerator() current

            /*
            Stopwatch stopwatch = stopwatch.StartNew();
            while (stopwatch.ElapsedMilliseconds < 1000.0f)
            { 
            
            }
            */

            #region 설명
            /*
                public Thread(ThreadStart start) {
            if (start == null) {
                throw new ArgumentNullException("start");
            }
            Contract.EndContractBlock();
            SetStartHelper((Delegate)start,0);  //0 will setup Thread with default stackSize
            }
 
            */
            //ThreadStart 대리자
            //public delegate void ThreadStart();
            //stackOverflow : 스택이 넘쳐나는 것. get 접근자에 get 프로퍼티를 접근해서 나온 에러

            //메인 스레드에 : 기본 스택 1mb 할당된다.
            //Public Thread(TreadStart ,int Size) 작업의 크기를 알고 있다면 Capacity를 설정해서 size 재할당 작업없이 메모리를 아낄 수 있다

            //1024 * 1024  == 1mb 
            //1024 = 1kb
            #endregion

            #region 디버깅 하는 방법
            //F9  중단점 찍고 실행하면 -> 
            //F10 프로시져 단위 다음 실행
            //F11 한 줄 단위 다음 실행
            //누르고 확인
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
            public Beverage MakeCoffe() //커피 내리기
            {
                Console.WriteLine($"바리스타 {name}(이)가 커피(을)를 추출을 시작합니다...");
                Thread.Sleep(1000); //3초동안 잠듦
                Console.WriteLine($"추출 중...");
                Thread.Sleep(2000); //3초동안 잠듦
                Console.WriteLine($"바리스타 {name}(이)가 커피(을)를 추출을 완료했습니다...");
                return (Beverage)random.Next(0,Enum.GetValues(typeof(Beverage)).Length);//0~Beverage범위까지 Random 뽑은 것을 이넘Beverage캐스팅해서 음료 반환
            }
        }
    }
}
