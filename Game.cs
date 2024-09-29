using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBlackJack {
    internal class Game {
        Player human;
        Player dealer;
        Deck deck;
        int bet, splitCount = 0;
        bool bust, split, doubleOK, multiHand, blackjack, roundWin, push = false;
        string input = "";
        string gameResult = "";
        public Game(Player p, Player d, Deck dk, int b) {
            human = p;
            dealer = d;
            deck = dk;
            bet = b;
            // ref
            Console.Clear();
            Console.WriteLine("\x1b[3J");
            //
            Console.WriteLine($"New Round");
            Console.WriteLine($"Player: {human.GetName()}");
            Console.WriteLine($"Funds: {human.GetMoney()}\tbet: {human.GetBet()}");
            Console.WriteLine($"Cards in deck: {deck}");
        }

        public string Start() {

            // initial deals
            human.AddCard(1, deck.DrawCard());
            dealer.AddCard(1, deck.DrawCard());
            human.AddCard(1, deck.DrawCard());
            dealer.AddCard(1, deck.DrawCard());

            //TODO test if dealer has ace/ blackjack = insurance?


            if (dealer.GetCard(1, 0).Contains("Ace"))
            {
                PrintRed($"\nDealer Hand: \n\t{dealer.GetCard(1, 0)}\n\t* hidden card *\n");
                // TODO insurance
            }
            else
            {   // Display the card normally if it's not an Ace
                Console.WriteLine($"\nDealer Hand: \n\t{dealer.GetCard(1, 0)}\n\t* hidden card *");
            }

            if (human.GetCard(1, 0).Contains("Ace") || human.GetCard(1, 1).Contains("Ace"))
            {   // player has ace, less risk to hit
                PrintYellow($"\nYour Hand: ( {human.GetPlayValue(1)} )\n\t{human.GetCard(1, 0)}\n\t{human.GetCard(1, 1)}\n");
                
                if (human.GetPlayValue(1) == 21)
                {
                    blackjack = true;
                    gameResult = "win player has Blackjack!";
                    Console.WriteLine("BlackJack!");
                    input = "s";
                }
            }
            else
            {
                Console.WriteLine($"\nYour Hand: ( {human.GetPlayValue(1)} )\n\t{human.GetCard(1, 0)}\n\t{human.GetCard(1, 1)}");
            }





            while (input != "s")
            {
                doubleOK = human.GetMoney() >= human.GetBet()*2 && 
                    human.GetPlayValue(1) <= 11 && human.GetPlayValue(1) >= 9 && human.GetHandSize(1) == 2;
                split = human.CanSplit(1);

                Console.WriteLine("\nWhat would you like to do?");
                PrintYellow($"Hit (h), Stand (s)");
                if (doubleOK)
                    PrintCyan(", Double (d)");
                if (split)
                    PrintCyan(", Split (x)?");
                Console.Write("\n>>> ");



                // Get the key press
                var key = Console.ReadKey(true);  // true to hide the key pressed in the console
                input = key.Key.ToString().ToLower();  // Convert to lowercase string for comparison



                input = Console.ReadLine();
                if (key.Key == ConsoleKey.H || key.Key == ConsoleKey.UpArrow)
                {// Hit
                    human.AddCard(1, deck.DrawCard());
                    Console.WriteLine($"\tPulled: {human.GetLastCard()} ");
                    Console.WriteLine($"Hand value: ( {human.GetPlayValue(1)} )");

                    if (human.GetPlayValue(1) > 21)
                    {
                        bust = true;
                        gameResult = "lose player bust";
                        Console.Write($"Bust! \nYour cards: \n");
                        human.PrintHand(1);
                        break;
                    }

                }
                else if (key.Key == ConsoleKey.S || key.Key == ConsoleKey.DownArrow)
                {// Stand
                    break;

                }
                else if ((key.Key == ConsoleKey.D || key.Key == ConsoleKey.RightArrow) && doubleOK)
                {// Double
                    //Console.Write($"\t\tOld bet: {human.GetBet()} , new: ");
                    human.SetBet(human.GetBet() *2);
                    PrintYellow($"new bet amount: {human.GetBet()}\n");

                    human.AddCard(1, deck.DrawCard());
                    Console.WriteLine($"\tPulled: {human.GetLastCard()} ");
                    Console.WriteLine($"Hand value: ( {human.GetPlayValue(1)} )");

                    if (human.GetPlayValue(1) > 21)
                    {
                        bust = true;
                        gameResult = "lose player bust";
                        Console.Write($"Bust! \nYour cards: \n");
                        human.PrintHand(1);
                    }
                    input = "s";
                    break;


                }
                else if (key.Key == ConsoleKey.X || key.Key == ConsoleKey.LeftArrow)
                {// Split
                    splitCount++;
                }
                else
                {
                    Console.WriteLine("Invalid input");
                }
            }

            //Console.WriteLine($"bj: {blackjack}\nbust: {bust}\n");

            if (bust || blackjack)
            {
                // skip the dealers turn
            }
            else
            {
                // dealers turn
                Console.WriteLine($"\nDealers turn...");
                while (dealer.GetPlayValue(1) < 17)
                {
                    dealer.AddCard(1, deck.DrawCard());
                    Console.WriteLine($"Dealer Hand: ( {dealer.GetPlayValue(1)} )");
                    if (dealer.GetPlayValue(1) > 21)
                    {
                        Console.WriteLine("Dealer Busts!");
                        gameResult = "win dealer busts";
                        // do return bet*2
                        break;
                    }
                }
                //Console.WriteLine($"Dealer has: stopped at ( {dealer.GetPlayValue(1)} ) ");
                dealer.PrintHand(1);
                /*for (int i = 0; i < dealer.GetHandSize(1); i++)
                {
                    Console.WriteLine($"\t{dealer.GetCard(1, i)}");
                }*/


                if (dealer.GetPlayValue(1) > 21)
                {
                    //dealer bust
                    gameResult = "win dealer bust";
                    roundWin = true;
                }
                else
                {
                    if (dealer.GetPlayValue(1) > human.GetPlayValue(1))
                    {
                        // dealer wins
                        gameResult = "lose dealer highest value";
                        Console.WriteLine("Dealer Wins");
                    }
                    else if (dealer.GetPlayValue(1) == human.GetPlayValue(1))
                    {
                        // tie
                        push = true;
                        gameResult = "push equal values";

                    }
                    else
                    {
                        // player wins
                        Console.WriteLine("You Win");
                        roundWin = true;
                        gameResult = "win player has highest value";
                    }
                }
            }
            return gameResult;
        }

        private static void PrintRed(string message) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(message);
            Console.ResetColor();
        }

        private static void PrintGreen(string message) {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(message);
            Console.ResetColor();
        }

        private static void PrintYellow(string message) {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(message);
            Console.ResetColor();
        }
        private static void PrintCyan(string message) {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(message);
            Console.ResetColor();
        }

    }
}

/* Refs
 * https://stackoverflow.com/questions/75471607/console-clear-doesnt-clean-up-the-whole-console
 *          Console.Clear();
 *          Console.WriteLine("\x1b[3J");
 * 
 */