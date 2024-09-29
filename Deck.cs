using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBlackJack {
    public class Deck {
        int reshullfleCount = 0;
        List<Card> cards = new();
        enum Suit { Spades, Hearts, Clubs, Diamonds };
        enum FaceValue {
            Ace = 1 | 11, Two, Three, Four, Five, Six, Seven,
            Eight, Nine, Ten = 10, Jack = 10, Queen = 10, King = 10
        };
        Random rng = new();
        int preferredDeckCount;

        public Deck(int deckCount = 1) {
            preferredDeckCount = deckCount;
            fillDeck();
            Shuffle();
            // printDeck();
            //Console.WriteLine();
            //Console.WriteLine($"total num cards in deck: {cards.Count}");
            //Console.WriteLine($"next cared = {drawCard()}");
            //printDeck();
        }


        void fillDeck() {
            for (int i = 0; i < preferredDeckCount; i++)
            {
                foreach (var suit in Enum.GetValues(typeof(Suit)))
                {
                    for (int value = 1; value <= 13; value++)
                    {
                        if (suit.ToString() != null)
                        {
                            cards.Add(new Card(suit.ToString()!, value));
                        }
                    }
                }
            }
            reshullfleCount++;
            Shuffle();
        }



        public void Shuffle() {
            cards = cards.OrderBy(a => rng.Next()).ToList();
        }

        public Card DrawCard() {
            if (cards.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n...Deck is empty, shuffling...\n");
                Console.ResetColor();
                fillDeck();
            }
            Card next = cards[0];
            cards.RemoveAt(0);
            //return next.ToString();
            return next;
        }

        public int GetReshuffleCount() {
            return reshullfleCount;
        }

        void PrintDeck() {
            foreach (Card card in cards)
            {
                Console.WriteLine(card.ToString());
            }
        }

        public override string ToString() {
            return $"{cards.Count} cards left";
        }
    }
}
