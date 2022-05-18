using mtg_lib.Library.Models;


namespace mtg_lib.Library.Services
{
    public class CardService
    {
        private mtgdevContext context;


        public CardService()
        {
            context = new mtgdevContext();
        }

        public IEnumerable<Card> GetCards()
        {
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

        public Card? GetCardFromString(string cardName)
        {
            IEnumerable<Card> cards = GetCards();

            return cards.Where(c => c.Name == cardName).First();
        }

        public List<string> RetrieveMtgIdsFromString(Card? card)
        {

            List<string> variationsList = new List<string>();

            var variationsString = card?.Variations;

            if (variationsString != null)
            {
                char[] charsToTrim = {'[', ']', '"'};
                
                var splitVariations = card?.Variations?.Split(',');

                foreach (var mtgid in splitVariations)
                {
                    var newId = mtgid.Trim(charsToTrim);

                    variationsList.Add(newId);
                }
            }

            return variationsList;
        }


        public string? GetImageFromVariations(Card card)
        {
            List<string> variations = RetrieveMtgIdsFromString(card);

            if (variations.Count != 0)
            {
                string IdVariationCard = variations.First();

                Card cardVariation = GetCards().Where(c => c.MtgId == IdVariationCard).First();

                if (cardVariation.OriginalImageUrl != null)
                {
                    //Console.WriteLine("Returning new image Url");
                    return cardVariation.OriginalImageUrl;
                }
                
            }
            
            //Console.WriteLine("No new image url to return!");
            return null;
        }

        public IEnumerable<Card> GetCardsByFilters(string rarity_code, string converted_mana_cost, string power, string thoughness)
        {
            IEnumerable<Card> cards = GetCards();
            List<Card> cardList = new List<Card>();

            foreach(var card in cards){
                if(card.RarityCode == rarity_code && card.ConvertedManaCost == converted_mana_cost && card.Power == power && card.Toughness == thoughness){
                    cardList.Add(card);
                }
            }
            return cardList;
        }
        
    }
}