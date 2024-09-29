using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBlackJack {
    public class Player {
        string name { get; set; }
        private int money { get; set; }
        private int bet { get; set; }
        List<Card> hand1 = new();
        List<Card> hand2 = new();
        List<Card> hand3 = new();
        List<Card> hand4 = new();

        public Player(int m) {
            money = m;
        }
        public Player(string n) {
            name = n;
        }



        public void SetName(string n) {
            name = n;
        }
        public string GetName() {
            return name;
        }
        public void SetBet(int b) {
            bet = b;
        }
        public int GetBet() {
            return bet;
        }

        public int GetMoney() {
            return money;
        }
        public void EditMoney(int amount) {
            money += amount;
        }


        public void ClearHand() {
            hand1.Clear();
            hand2.Clear();
            hand3.Clear();
            hand4.Clear();
        }



        public int GetHandSize(int i) {
            switch (i)
            {
                case 1:
                    return hand1.Count;
                case 2:
                    return hand2.Count;
                case 3:
                    return hand3.Count;
                case 4:
                    return hand4.Count;
                default:
                    return 0;
            }
        }

        public void AddCard(int handIndex, Card c) {
            switch (handIndex)
            {
                case 1:
                    hand1.Add(c);
                    break;
                case 2:
                    hand2.Add(c);
                    break;
                case 3:
                    hand3.Add(c);
                    break;
                case 4:
                    hand4.Add(c);
                    break;
                default:
                    Console.WriteLine("Invalid hand index");
                    break;
            }
            //Console.WriteLine($"{c} at hand{handIndex}");
        }

        public String GetLastCard() {
            return hand1[hand1.Count - 1].ToString();
        }


        public string GetCard(int handIndex, int cardIndex) {
            switch (handIndex)
            {
                case 1:
                    return hand1[cardIndex].ToString();
                case 2:
                    return hand2[cardIndex].ToString();
                case 3:
                    return hand3[cardIndex].ToString();
                case 4:
                    return hand4[cardIndex].ToString();
                default: return "Invalid hand index";
            }
        }

        public bool CanSplit(int handIndex) {
            if (handIndex == 1)
            {
                if (hand1[0].GetValue() == hand1[1].GetValue())
                {
                    return true;
                    /*Console.WriteLine("Would you like to split?");
                    string input = Console.ReadLine();
                    if (input == "yes")
                    {
                        AddCard(2, hand1[1]);
                        hand1.RemoveAt(1);
                        return true;
                    }*/
                }
            }
            return false;
        }

        public int GetPlayValue(int handIndex) {
            List<Card> hand = handIndex switch
            {
                1 => hand1,
                2 => hand2,
                3 => hand3,
                4 => hand4,
                _ => null // In case of invalid handIndex
            };

            if (hand == null || hand.Count == 0)
            {
                return 0; // If the hand is invalid or empty, return 0
            }

            int totValue = hand.Sum(card => card.GetValue());
            int aceCount = hand.Count(card => card.IsAce());

            while (totValue > 21 && aceCount > 0)
            {
                totValue -= 10;
                aceCount--;
            }
            return totValue;
        }

        public void PrintHand(int hand) {
            Console.WriteLine($"Cards at hand: ( {GetPlayValue(hand)} )");
            for (int i = 0; i < hand1.Count; i++)
            {
                Console.WriteLine($"\t{hand1[i].ToString()}");
            }
        }



        public override string ToString() {
            return $"Player: {name} \nFunds: {money} - Bet( {bet} )";
        }
    }
}
