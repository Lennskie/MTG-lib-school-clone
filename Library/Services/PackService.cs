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

            IEnumerable<Card> cardPacks = cards.OrderBy(c => r.Next(cards.Count() - 1)).Take(15);


            return cardPacks;
        }



    }
}

