using System;
using System.Collections.Generic;

namespace ConsoleBlackJack {

    /* TODO: double functionality
     *       split functionality
     *          several hands
     *       insurance functionality
     *       topscore based on number of decks used    
     * 
     * 
     * */


    internal class Program {
        public const int RoundLimitForSave = 10;

        static void Main(string[] args) {
            // Load existing game stats
            List<GameStats> allStats = GameStats.LoadGameStats();
            string nameBuffer = "";


            while (true)
            {
                Console.WriteLine("Welcome to the Console BlackJack game!");
                Console.WriteLine("Please choose an option:");
                Console.WriteLine("1: View Top Score");
                Console.WriteLine("2: Start Game");
                Console.WriteLine("3: Exit");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        GameStats.DisplayTopScore(allStats);
                        break;
                    case "2":
                        nameBuffer = StartGame(allStats, nameBuffer);
                        break;
                    case "3":
                        Console.WriteLine("Thank you for playing! Goodbye.");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private static string StartGame(List<GameStats> allStats, string n) {
            // Game "table" setup
            Player dealer = new Player("Dealer");
            Player human = new Player(5000); // fund limit
            string playerName = n;
            Deck deck;
            int deckNumber = 0, bet = 0;
            string roundResult = "";

            // Game statistics Player
            int rounds = 0, totWinnings = 0, timesWon = 0, timesLost = 0, timesPushed = 0;



            // test if playerName is already set or not
            if (playerName.Equals("") || playerName.Equals("GameTester69"))
            {
                Console.Write("What is your NAME? ");
                playerName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(playerName))
                {
                    playerName = "GameTester69";
                }
            }
            human.SetName(playerName);

            Console.Write($"Greetings {playerName}!\nHow many DECKS do you want to use? ");
            while (deckNumber == 0)
            {
                if (!int.TryParse(Console.ReadLine(), out deckNumber))
                {   //case if input is not a number
                    deckNumber = 1;
                    Console.WriteLine("House advantage:\t0.17 %");
                }
                else
                {
                    Console.WriteLine($"Great, {deckNumber} Decks it is!");
                    switch(deckNumber)
                    {
                        case 1: Console.WriteLine("House advantage:\t0.17 %"); break;
                        case 2: Console.WriteLine("House advantage:\t0.46 %"); break;
                        case 3: Console.WriteLine("House advantage:\t0.57 %"); break;
                        case 4: Console.WriteLine("House advantage:\t0.60 %"); break;
                        case 5: Console.WriteLine("House advantage:\t0.62 %"); break;
                        case 6: Console.WriteLine("House advantage:\t0.64 %"); break;
                        case 7: Console.WriteLine("House advantage:\t0.65 %"); break;
                        case 8: Console.WriteLine("House advantage:\t0.66 %"); break;
                    }
                }
            }
            deck = new Deck(deckNumber);

            // Game loop
            while (true)
            {
                if (human.GetMoney() == 0)
                {
                    break; // no money, no game
                }


                Console.WriteLine("-----New Round-----");
                Console.WriteLine($"Funds: {human.GetMoney()}");
                Console.Write("How much would you like to BET? ");

                // Validating bet input
                while (true)
                {
                    if (!int.TryParse(Console.ReadLine(), out bet))
                    {
                        Console.WriteLine("Please enter a valid number.");
                    }
                    else if (bet <= 0)
                    {
                        Console.WriteLine("Bet must be greater than zero. Please enter a valid bet.");
                    }
                    else if (bet > human.GetMoney())
                    {
                        Console.WriteLine($"You cannot bet more than you have. Your current balance is {human.GetMoney()}.");
                    }
                    else
                    {
                        // Valid bet, exit the loop
                        break;
                    }
                }


                human.SetBet(bet);

                // Start the round
                roundResult = new Game(human, dealer, deck, bet).Start();
                rounds++;

                // round results
                if (roundResult.Contains("win"))
                {
                    int winnings = (roundResult.Contains("Blackjack"))
                        ? (int)Math.Round(human.GetBet() * 1.5)
                        : human.GetBet();

                    human.EditMoney(winnings);          // Add winnings
                    totWinnings += winnings;            // Update total winnings
                    timesWon++;                         // Update times won
                    PrintGreen("Congratulations! You won ");
                    Console.WriteLine($"{ winnings}");
                }
                else if (roundResult.Contains("lose"))
                {
                    timesLost++;                        // Update times lost
                    human.EditMoney(-human.GetBet());   // Deduct the bet
                    PrintRed("You lost ", human.GetBet());
                }
                else if (roundResult.Contains("push"))
                {
                    timesPushed++;                      // Update times pushed
                    PrintYellow("It's a push!");
                }

                // Reset for the next round
                bet = 0;
                roundResult = "";
                human.ClearHand();
                dealer.ClearHand();
            }

            double winRatio = rounds > 0 ? ((double)timesWon / rounds) * 100 : 0;  // Calculate win ratio

            Console.WriteLine($"\n\n\nGame Over {playerName}! You have run out of money.");
            Console.WriteLine($"Total winnings: {totWinnings}");
            Console.WriteLine($"Rounds played:  {rounds}");
            Console.WriteLine($"Times won:      {timesWon}");
            Console.WriteLine($"Times lost:     {timesLost}");
            Console.WriteLine($"Times pushed:   {timesPushed}");
            Console.WriteLine($"Win ratio %:    {winRatio:F2}"); // Format win ratio to 2 decimal places
            Console.WriteLine($"Decks shuffled: {deck.GetReshuffleCount()}\n\n");

            // Create a new GameStats instance to save
            var gameStats = new GameStats
            {
                Name = playerName,
                TotalWinnings = totWinnings,
                RoundsPlayed = rounds,
                TimesWon = timesWon,
                TimesLost = timesLost,
                TimesPushed = timesPushed,
                WinRatio = winRatio,
                DecksShuffled = deck.GetReshuffleCount(),
                Timestamp = DateTime.Now // Add timestamp
            };

            // Save game statistics (if played more than RoundLimitForSave)
            if (rounds > RoundLimitForSave)
            {
                allStats.Add(gameStats);
                GameStats.SaveGameStats(gameStats);
                Console.WriteLine("Game Saved\n\n");
            }
            else
            {
                Console.WriteLine("Game not saved. You must play more than 10 rounds to save your game.\n\n");
            }
            return playerName;
        }


        private static void PrintRed(string message, int b) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message + b);
            Console.ResetColor();
        }

        private static void PrintGreen(string message) {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(message);
            Console.ResetColor();
        }

        private static void PrintYellow(string message) {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
