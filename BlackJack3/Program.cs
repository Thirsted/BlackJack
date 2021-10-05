using System;
using static BlackJack3.Cards;

namespace BlackJack3
{
    class Program
    {
        public static int sop = 0; //ScoreOfPlayer
        public static int soa = 0; //ScoreOfAI
        public static int psum; //PlayerCardValueSum
        public static int asum; //AICardValueSum
        public static bool beginning = true; //NewGame?
        public static bool playerTurn = true; //IsPlayerTurn?
        public static bool playerStop = false; //PlayerWannaStopPickup?

        static void Main(string[] args)
        {
            //Cards cd = new();
            GameStart();
        }

        public static void GameStart() //게임 최초 실행
        {
            Program pm = new();
            Console.WriteLine("\n1. 게임 시작시 딜러와 플레이어는 카드를 두장씩 받습니다. 이때 딜러는 컴퓨터가 맡습니다.\n");
            Console.WriteLine("2. 매 페이즈 마다 플레이어-딜러 순으로 번갈아가며 카드를 뽑을지 말지 결정합니다.\n");
            Console.WriteLine("3. 최종적으로 둘중 21점에 가까운 사람이 승리하지만, 만약 21점을 넘길경우 패배로 간주합니다.\n");
            Console.WriteLine("4. 만약 플레이어가 뽑기 중단을 선언하면 플레이어는 그 이후로 카드를 뽑을 수 없습니다.\n");
            Console.WriteLine("5. 플레이어가 더 이상 뽑지 못할 때 동점일 경우 딜러의 승리로 간주합니다." +
                "\n\t단, 두명 다 21점일 경우 무승부로 처리합니다.\n");
            Console.WriteLine("-+-+-기타 규칙-+-+-\n\t- A는 1점과 11점중에 선택할 수 있습니다.\n\t" +
                "- J, Q, K은 모두 10점으로 처리합니다.\n\t" +
                "- 누군가 첫 두장으로 21점을 획득하면 두명 모두 다시 뽑습니다.\n");
            Console.Write("시작하려면 아무 값이나 입력하세요... ");
            Console.ReadLine();
            Console.WriteLine($"\n------- {sop + soa + 1}라운드 -------\n\n");
            NewGame();
        }

        public static void NewGame() //새 게임 시작
        {
            Cards cd = new();
            cd.CreatDeck(); //덱 생성
            //cd.ShowDeck(); //덱 초기화 여부 확인용
            psum = asum = 0;
            playerTurn = true;
            playerStop = false;
            beginning = true;
            cd.BeginningPickup(); //첫 두장 픽업
        }

        internal static void PlayerDecide()
        {
            bool rightAnswer = false; //Is right value?
            Cards cd = new();
            Console.WriteLine($"\n현재 점수:   You: {psum}\tAI: {asum}\n");
            Console.WriteLine("카드를 더 받으려면 'y' 또는 '네'를, 중단하려면 'n' 또는 '아니오'를 입력하세요.");
            Console.Write("한 번 뽑기를 중단하면 더이상 뽑을 수 없습니다: ");
            while (!rightAnswer)
            {
                string insert = Console.ReadLine();
                if (insert == "y" || insert == "Y" || insert == "네")
                {
                    rightAnswer = true;
                    Console.Write("\n카드를 한 장 더 받습니다." +
                        "\n\t계속하려면 아무 값이나 입력하세요... ");
                    Console.ReadLine();
                    cd.CheakAlreadyPickup();
                }
                else if ((insert == "n" || insert == "N" || insert == "아니오") && psum > asum)
                {
                    rightAnswer = true; playerTurn = false; playerStop = true;
                    Console.WriteLine("\n 당신은 더 이상 카드를 뽑지 않습니다.\n");
                    AIDecide();
                }
                else if ((insert == "n" || insert == "N" || insert == "아니오") && psum <= asum)
                {
                    rightAnswer = true;
                    Console.Write("\n당신의 점수가 AI보다 작거나 같으므로 당신은 반드시 한 장을 받아야합니다." +
                        "\n\t계속하려면 아무 값이나 입력하세요... ");
                    Console.ReadLine();
                    cd.CheakAlreadyPickup();
                }
                else
                {
                    Console.Write("올바른 값을 입력하세요: ");
                }
            }
        }

        internal static void ChooseACEValue(bool isPlayerTurn) //ACE값 선택
        {
            if (isPlayerTurn == false) //AI의 ACE값 선택
            {
                if (asum + 11 <= 21)
                {
                    asum += 11; Console.WriteLine("AI: 11점 선택");
                }
                else
                {
                    asum += 1; Console.WriteLine("AI: 1점 선택");
                }
            }
            else //플레이어의 ACE값 선택
            {
                if (psum + 11 > 21) //11점을 받으면 21점이 넘어갈 때
                {
                    psum += 1; Console.WriteLine("11점을 선택할 수 없기에 자동으로 1점을 추가합니다.");
                }
                else //11점을 받아도 상관이 없을 때
                {
                    bool isInt = false; //IsInt?
                    int av = 0; //AceValue
                    Console.Write("1점과 11점을 선택할 수 있습니다. 원하는 값을 입력하세요: ");
                    while(!isInt)
                    {
                        string insert = Console.ReadLine();
                        isInt = Int32.TryParse(insert, out av);
                        if (!isInt)
                        {
                            Console.Write("1 또는 11을 입력해야 합니다: ");
                        }
                    }
                    switch (av)
                    {
                        case 1: psum += 1; Console.WriteLine("1점을 선택하였습니다."); break;
                        case 11: psum += 11; Console.WriteLine("11점을 선택하였습니다."); break;
                    }
                }
            }
        }

        public static void ShowScore() //각 라운드 종료시
        {
            Cards cd = new();
            Console.WriteLine($"\n-+-+-+-+== You : {sop} VS AI: {soa} ==+-+-+-+-");
            Console.Write("한 번 더 하시려면 '네' 또는 'y'를 입력하세요: ");
            string onceMore = Console.ReadLine();
            Console.WriteLine("");
            if (onceMore == "네" || onceMore == "y" || onceMore == "Y")
            {
                Console.WriteLine($"\n------- {sop + soa + 1}라운드 -------\n\n");
                NewGame();
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine("  -게임이 종료되었습니다.");
                Console.WriteLine($"  -총 게임수 : {sop + soa}\n");
                if (sop < soa)
                {
                    Console.WriteLine($"\t---최종 결과는 {sop} : {soa}, AI의 승리입니다---");
                }
                else if (sop > soa)
                {
                    Console.WriteLine($"\t---최종 결과는 {sop} : {soa}, 당신의 승리입니다---");
                }
                else
                    Console.WriteLine($"\t---최종 결과는 {sop} : {soa}, 무승부입니다---");
                Environment.Exit(0);
            }
        }

        public static void AIDecide() //AI가 점수에 따라 뽑을지 말지 결정
        {
            Cards cd = new();
            if (psum > asum && psum <= 21) //점수가 21점 이하이며, 플레이어보다 낮다면 뽑기 진행
            {
                cd.CheakAlreadyPickup();
                Console.Write("AI가 한 장을 뽑았습니다." +
                    "\n\t아무 값이나 입력하여 계속합니다.. "); Console.ReadLine();
            }
            else if (psum <= asum)
            {
                playerTurn = true;
                Console.WriteLine("\n-+-AI가 뽑지 않고 턴을 넘겼습니다. " +
                    "당신은 AI보다 점수가 낮거나 같기 때문에 반드시 한 장을 뽑아야 합니다.");
                Console.WriteLine("\t아무 값이나 입력하여 계속합니다...");
                Console.ReadLine();
                cd.CheakAlreadyPickup();
            }
        }
    }
}
