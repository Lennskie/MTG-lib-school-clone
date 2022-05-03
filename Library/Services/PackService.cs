using mtg_lib.Library.Models;


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
            // TODO: Check if the cards packs contain the right amount of cards for each option ...
            
            // 1 Basic Land
            IEnumerable<Card> basicCard = cards.Where(c => c.Type.Contains("Land")).OrderBy(c =>  r.Next(cards.Count() - 1)).Take(1);

            // 10 Commons
            IEnumerable<Card> commonCards = cards.Where(c => c.RarityCode == "C").OrderBy(c =>  r.Next(cards.Count() - 1)).Take(10);

            // 3 Uncommon
            IEnumerable<Card> uncommonCards = cards.Where(c => c.RarityCode == "U").OrderBy(c =>  r.Next(cards.Count() - 1)).Take(3);

            // 1 is rare or mythic
            IEnumerable<Card> rareOrMythicCard = cards.Where(c => c.RarityCode is "R" or "M").OrderBy(c => r.Next(cards.Count() - 1)).Take(1);

            IEnumerable<Card> cardPacks = basicCard.Concat(commonCards).Concat(uncommonCards).Concat(rareOrMythicCard).Select(c => c).ToList();

            return cardPacks;
        }

        private IEnumerable<Card> appendToList(IEnumerable<Card> toAppend)
        {
            IEnumerable<Card> toReturn = new List<Card>();

            return toReturn;
        }


        public IEnumerable<Card> retrievePacks()
        {
            return new List<Card>();
        }


        public IEnumerable<Card> retrievePackInfo(string packId)
        {
            return new List<Card>();
        }

    }
}

