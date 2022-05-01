﻿using mtg_lib.Library.Models;


namespace mtg_lib.Library.Services
{
    public class PackService
    {
        private mtgdevContext context;
        private CardService cardService = new CardService();


        public PackService()
        {
            context = new mtgdevContext();
        }

        public IEnumerable<Card> CreateRandomPack(string rarity)
        {
            IEnumerable<Card> cards = cardService.GetCards();
            
            // 15 Cards
            var r = new Random();
            
            // 1 Basic Land
            IEnumerable<Card> basicCard = cards.Where(c => c.Type.Contains("Land")).OrderBy(c =>  r.Next(cards.Count() - 1)).Take(1);
            Console.WriteLine("Count: " + basicCard.Count());
            
            // 10 Commons
            IEnumerable<Card> commonCards = cards.Where(c => c.RarityCode == "C").OrderBy(c =>  r.Next(cards.Count() - 1)).Take(10);
            Console.WriteLine("Count: " + commonCards.Count());
            
            // 3 Uncommon
            IEnumerable<Card> uncommonCards = cards.Where(c => c.RarityCode == "U").OrderBy(c =>  r.Next(cards.Count() - 1)).Take(3);
            Console.WriteLine("Count: " + uncommonCards.Count());
            
            // 1 is rare or mythic
            IEnumerable<Card> rareOrMythicCard = cards.Where(c => c.RarityCode is "R" or "M").OrderBy(c => r.Next(cards.Count() - 1)).Take(1);
            Console.WriteLine("Count: " + rareOrMythicCard.Count());

            
            IEnumerable<Card> cardPacks = cards.OrderBy(c => r.Next(cards.Count() - 1)).Take(15);
            
            /*IEnumerable<Card> cardPacks = basicCard;
            cardPacks.Concat(commonCards);
            cardPacks.Concat(uncommonCards);
            cardPacks.Concat(rareOrMythicCard);*/

            return cardPacks;
        }

        private IEnumerable<Card> appendToList(IEnumerable<Card> toAppend)
        {
            IEnumerable<Card> toReturn = new List<Card>();

            return toReturn;
        }



    }
}

