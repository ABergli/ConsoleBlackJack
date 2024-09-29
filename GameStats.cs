using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ConsoleBlackJack {
    internal class GameStats {
        public string Name { get; set; }
        public int TotalWinnings { get; set; }
        public int RoundsPlayed { get; set; }
        public int TimesWon { get; set; }
        public int TimesLost { get; set; }
        public int TimesPushed { get; set; }
        public double WinRatio { get; set; }
        public int DecksShuffled { get; set; }
        public DateTime Timestamp { get; set; }

        private const string FileName = "BlackJackGameStats.json";

        // Save game stats to a file (append mode)
        public static void SaveGameStats(GameStats stats) {
            List<GameStats> statsList = new List<GameStats>();

            // Check if the file exists and is not empty
            if (File.Exists(FileName))
            {
                string existingJson = File.ReadAllText(FileName);

                if (!string.IsNullOrEmpty(existingJson))
                {
                    try
                    {
                        // Attempt to deserialize the existing data into a list
                        statsList = JsonSerializer.Deserialize<List<GameStats>>(existingJson);
                    }
                    catch (JsonException)
                    {
                        // If the file isn't in a valid list format, we'll treat it as empty
                        Console.WriteLine("Warning: The JSON file is not in the expected format. Starting with an empty list.");
                    }
                }
            }

            // Add the new game statistics to the list
            statsList.Add(stats);

            // Convert the updated list to JSON format with indentation
            string updatedJson = JsonSerializer.Serialize(statsList, new JsonSerializerOptions { WriteIndented = true });

            // Write the updated JSON back to the file
            File.WriteAllText(FileName, updatedJson);
        }

        // Load saved game stats from the file
        public static List<GameStats> LoadGameStats() {
            if (!File.Exists(FileName))
            {
                Console.WriteLine("No saved game statistics found.");
                return new List<GameStats>();
            }

            string json = File.ReadAllText(FileName);
            try
            {
                return JsonSerializer.Deserialize<List<GameStats>>(json) ?? new List<GameStats>();
            }
            catch (JsonException)
            {
                Console.WriteLine("Error: Failed to load game statistics due to incorrect file format.");
                return new List<GameStats>();
            }
        }

        // Method to display statistics in the console
        public static void DisplayGameStats(GameStats stats) {
            Console.WriteLine($"\nPlayer:         {stats.Name}");
            Console.WriteLine($"Total winnings: {stats.TotalWinnings}");
            Console.WriteLine($"Rounds played:  {stats.RoundsPlayed}");
            Console.WriteLine($"Times won:      {stats.TimesWon}");
            Console.WriteLine($"Times lost:     {stats.TimesLost}");
            Console.WriteLine($"Times pushed:   {stats.TimesPushed}");
            Console.WriteLine($"Win ratio:      {stats.WinRatio:F2}%");
            Console.WriteLine($"Decks shuffled: {stats.DecksShuffled}");
            Console.WriteLine($"Game finished:  {stats.Timestamp}\n");
        }

        // Method to display all saved statistics
        public static void DisplayAllStats(List<GameStats> statsList) {
            if (statsList.Count == 0)
            {
                Console.WriteLine("No statistics available.");
                return;
            }

            foreach (var stats in statsList)
            {
                DisplayGameStats(stats);
            }
        }

        // Method to display the top score based on winnings per game
        public static void DisplayTopScore(List<GameStats> statsList) {
            if (statsList.Count == 0)
            {
                Console.WriteLine("No statistics available.");
                return;
            }

            // Find the top score (max winnings per game)
            GameStats topScore = null;
            double highestRatio = 0.0;

            foreach (var stats in statsList)
            {
                if (stats.RoundsPlayed > 0) // Avoid division by zero
                {
                    double currentRatio = (double)stats.TotalWinnings / stats.RoundsPlayed;

                    if (topScore == null || currentRatio > highestRatio)
                    {
                        topScore = stats;
                        highestRatio = currentRatio;
                    }
                }
            }

            if (topScore != null)
            {
                Console.WriteLine("\nTop Score (Total Winnings / Rounds Played):");
                DisplayGameStats(topScore);
            }
            else
            {
                Console.WriteLine("No valid statistics available to determine the top score.");
            }
        }
    }
}
