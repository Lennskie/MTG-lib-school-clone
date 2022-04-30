using mtg_lib.Library.Models;


namespace mtg_lib.Library.Services
{
    public class CardService
    {
        private mtgdevContext context;
        
        
        public CardService() {
            context = new mtgdevContext();
        }
        
        public IEnumerable<Card> GetCards() {
            return context.Cards.ToList();
        }

        
        public IEnumerable<Card> GetSetAmountOfCards(int amount)
        {
            if (amount != 0)
            {
                return GetCards().Take(amount);
            }
            return GetCards().Take(100);
        }

        public Card? GetCardFromId(string cardId)
        {
            IEnumerable<Card> cards = GetCards();
            
            return cards.SingleOrDefault(c => c.MtgId == cardId);
        }


    }    
}