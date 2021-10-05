using System;
using static BlackJack3.Program;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackJack3
{
    public class Cards
    {
        public static int[,] card = new int[4, 14];
        public void CreatDeck() //덱 생성
        {
            for (int suits = 0; suits < 4; suits++)
            {
                for (int values = 0; values < 14; values++)
                {
                    card[suits, values] = values;
                }
            }
        }

        public void CheakAlreadyPickup() //임의의 수를 뽑고 중복인지 아닌지 판별
        {
            Random random = new();
            int suits = random.Next(0, 4);
            int values = random.Next(0, 14); //문양 및 값 랜덤픽업

            while (true)
            {
                if (card[suits, values] == 19 || values == 1)
                {
                    //Console.WriteLine("--------1이나 중복으로 뽑았음--------");
                    suits = random.Next(0, 4);
                    values = random.Next(0, 14);
                }
                else
                {
                    //Console.WriteLine("-----1이나 중복 아님-----");
                    card[suits, values] = 19;
                    //중복이 아니라면 해당 배열의 값을 19로 초기화
                    break;
                }
            }
            PickupCard(suits, values); //1 또는 중복이 아닌지 확인 후 값 제공
        }

        public void PickupCard(int suits, int values) //중복 확인 후 뽑은 카드 노출
        {
            string valued = "";
            switch (values)
            {
                case 0: valued = "A"; break;
                case 11: valued = "J"; values = 10; break;
                case 12: valued = "Q"; values = 10; break;
                case 13: valued = "K"; values = 10; break;
                default: valued = values.ToString(); break;
            }
            switch (suits)
            {
                case 0: Console.WriteLine($"♣{valued}"); break;
                case 1: Console.WriteLine($"♠{valued}"); break;
                case 2: Console.WriteLine($"♥{valued}"); break;
                case 3: Console.WriteLine($"◆{valued}"); break;
                    //문양 및 기호로 변환 후 출력
            }
            GiveValue(suits, values);
        }

        internal void BeginningPickup() //첫 두장 픽업
        {
            for (int times = 0; times < 4; times++)
            {
                CheakAlreadyPickup();
            }
            beginning = false;

            if (psum >= 21 || asum >= 21)
            {
                Console.Write("\n누군가의 첫 두장에서 21점 이상이 나와 게임을 다시 시작합니다" +
                    "\n\t아무 값이나 입력하여 진행합니다.. "); Console.ReadLine();
                Console.WriteLine($"\n------------------{sop + soa + 1}라운드 재개");
                NewGame(); //누군가 처음부터 21점 획득, 다시 뽑기 진행
            }
            else //두명 모두 첫 두장에서 21점이 나오지 않음, 게임 진행
            {
                Console.Write("\n아무 값이나 입력하여 본인의 턴을 준비합니다...");
                Console.ReadLine();
                PlayerDecide();
            }
        }

        private void GiveValue(int suits, int values) //누구의 턴인지에 따라 값 제공
        {
            if (playerTurn == true) //플레이어 턴일때
            {
                if (values == 0) //플레이어가 뽑은 카드가 ACE일떄
                {
                    ChooseACEValue(true);
                }
                else
                {
                    psum += values;
                }
                Console.WriteLine($"    ㄴ당신의 현재 점수는 {psum}점 입니다.");
                if (psum == 21 && beginning == false)
                {
                    Console.WriteLine("\n당신은 21점에 도달하였습니다.\n\t" +
                        "만약 AI가 이번턴에 21점에 도달하지 못할 경우 당신의 승리입니다.\n");
                }
            }
            else //AI턴일때
            {
                if (values == 0) //AI가 뽑은 카드가 ACE일때
                {
                    ChooseACEValue(false);
                }
                else
                {
                    asum += values;
                }
                Console.WriteLine($"    ㄴAI의 현재 점수는 {asum}점 입니다.");
            }
            if (beginning == true)
            {
                playerTurn = !playerTurn;
            }
            else if (beginning == false) //첫 두장은 승패 판단 안함
                Judgmented();
        }

        public void Judgmented() //현 점수에 따른 승패 판단, 턴에 따른 선택지 제공
        {
            if (playerTurn == true) //플레이어의 턴
            {
                if (psum > 21) //플레이어 턴일 때 21점 넘기면
                {
                    soa += 1; Console.WriteLine("21점을 초과하였습니다.");
                    Console.WriteLine("AI가 승리했습니다");
                    ShowScore();
                }
                else
                {
                    playerTurn = false;
                    Console.Write("아무 값이나 입력하여 AI에게 턴을 넘깁니다...");
                    Console.ReadLine();
                    AIDecide();
                }
            }
            else //AI의 턴
            {
                if (asum > 21) //AI턴일 때 21점 넘기면(플레이어는 21점 미만)
                {
                    sop += 1; Console.WriteLine("\n---AI가 21점을 초과하였습니다.");
                    Console.WriteLine("당신의 승리입니다.");
                    ShowScore();
                }
                else if (psum == 21 && asum == 21)
                {
                    sop += 1; soa += 1;
                    Console.WriteLine("\n---두 명 모두 21점이므로 무승부로 처리합니다.");
                    ShowScore();
                }
                else if (asum == 21)
                {
                    soa += 1; Console.WriteLine("\n---AI가 먼저 21점을 달성하였습니다.");
                    Console.WriteLine("AI의 승리입니다.");
                    ShowScore();
                }
                else if (playerStop == true && asum >= psum) //플레이어는 더 이상 카드를 뽑지 않으며 딜러의 점수가 크거나 같음
                {
                    soa += 1; Console.WriteLine("\n---당신은 더 이상 카드를 뽑을 수 없으며 " +
                        "AI의 점수보다 적거나 같습니다.");
                    Console.WriteLine("AI의 승리입니다.");
                    ShowScore();
                }
                else if (playerStop == true) //플레이어가 더이상 뽑지 않음, AI점수가 더 낮음
                {
                    AIDecide();
                    Console.Write("AI가 한 장을 뽑았습니다." +
                    "\n\t아무 값이나 입력하여 계속합니다.. "); Console.ReadLine();
                }
                else
                {
                    if (psum == 21) //플레이어가 21점에 도달 후, AI가 한 장을 뽑았음에도 21점을 달성하지 못함
                    {
                        sop += 1; Console.WriteLine("\n---혼자 21점에 도달하였습니다.");
                        Console.WriteLine("당신의 승리입니다.");
                        ShowScore();
                    }
                    else //AI가 뽑은 후 승패 결정이 안남, 다시 플레이어 차례
                    {
                        playerTurn = true;
                        Console.Write("아무 값이나 입력하여 본인의 턴을 준비합니다...");
                        Console.ReadLine();
                        PlayerDecide();
                    }
                }
            }
        }

        public void ShowDeck() //잔여 덱 실험용
        {
            foreach (var item in card)
            {
                Console.Write($"\t{item}");
            }
            Console.WriteLine("");
        }
    }
}
