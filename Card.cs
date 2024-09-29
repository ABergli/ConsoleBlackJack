using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBlackJack {
    public class Card {
        string suit;
        int value;

        public Card(string s, int v) {
            suit = s;
            value = v;
        }

        public string GetSuit() {
            return suit;
        }

        public int GetValue() {
            if (this.IsAce())
            {
                return 11; // Initially treat Ace as 11
            }
            else if (this.value >= 10)
            {
                return 10; // Face cards (Jack, Queen, King) and 10
            }
            else
            {
                return this.value; // Number cards (2-9)
            }
        }

        public bool IsAce() {
            return value == 1;
        }


        public override string ToString() {
            string faceValue = value switch
            {
                1 => "Ace",
                11 => "Jack",
                12 => "Queen",
                13 => "King",
                _ => value.ToString()
            };
            return $"{faceValue} of {suit}";
        }
    }
}