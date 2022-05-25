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

        public Card? GetCardFromUserTableId(string cardId)
        {
            IEnumerable<Card> cards = GetCards();

            return cards.SingleOrDefault(c => c.Id.ToString() == cardId);
        }

        public List<Card> GetCardFromString(string cardName)
        {
            IEnumerable<Card> cards = GetCards();
            List<Card> matches = new List<Card>();

            foreach(var card in cards){
                if(card.Name.Contains(cardName)) matches.Add(card);
            }

            return matches;
        }

        public List<string> RetrieveMtgIdsFromString(Card? card)
        {

            List<string> variationsList = new List<string>();

            var variationsString = card?.Variations;

            if (variationsString != null)
            {
                char[] charsToTrim = {'[', ']', '"'};
                
                var splitVariations = card?.Variations?.Split(',');

                if (splitVariations != null)
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
                string idVariationCard = variations.First();

                Card cardVariation = GetCards().Where(c => c.MtgId == idVariationCard).First();

                if (cardVariation.OriginalImageUrl != null)
                {
                    return cardVariation.OriginalImageUrl;
                }
                
            }
            return null;
        }

        public IEnumerable<Card> GetCardsByFilters(string rarityCode, string convertedManaCost, string power, string thoughness)
        {
            List<Card> cardListR = GetRarityList(rarityCode);   
            List<Card> cardListM = GetManaList(convertedManaCost);   
            List<Card> cardListP = GetPowerList(power);
            List<Card> cardListT = GetThoughnessList(thoughness);

            var disjunction = new HashSet<Card>(cardListR);
            if(cardListM.Count != 65597){
                disjunction.SymmetricExceptWith(cardListM);
            }
            if(cardListP.Count != 65597){
                disjunction.SymmetricExceptWith(cardListP);
            }
            if(cardListT.Count != 65597){   
                disjunction.SymmetricExceptWith(cardListT);
            }
            return disjunction;  
        }

        private List<Card> GetRarityList(string rarityCode){
            IEnumerable<Card> cards = GetCards();

            if(rarityCode == null){
                return cards.ToList();
            }
            return cards.Where(c=>c.RarityCode == rarityCode).ToList();
        }

        private List<Card> GetManaList(string convertedManaCost){
            IEnumerable<Card> cards = GetCards();
            
            if(convertedManaCost == null){
                return cards.ToList();
            }

            return cards.Where(c=>c.ConvertedManaCost == convertedManaCost).ToList();
        }

        private List<Card> GetPowerList(string power){
            IEnumerable<Card> cards = GetCards();

            if(power == null){
                return cards.ToList();
            }

            return cards.Where(c=>c.Power == power).ToList();
        }

        private List<Card> GetThoughnessList(string thoughness){
            IEnumerable<Card> cards = GetCards();
            if(thoughness == null){
                return cards.ToList();
            }

            return cards.Where(c=>c.Toughness == thoughness).ToList();
        }

        public List<String> GetRarity(){
            IEnumerable<Card> cards = GetCards();
            List<string> rarityList = new List<string>();
            foreach(var card in cards){
                if(!rarityList.Contains(card.RarityCode) && card.RarityCode != null){
                    rarityList.Add(card.RarityCode);
                }
            }
            return rarityList.OrderBy(r=>r).ToList();
        }

        public List<int> GetPower(){
            IEnumerable<Card> cards = GetCards();
            List<int> powerList = new List<int>();
            foreach(var card in cards){
                var value = 0;
                int.TryParse(card.Power, out value);
                if(!powerList.Contains(value) && card.Power != null){
                    powerList.Add(value);
                }
            }
            powerList.Sort();
            return powerList;
        }

        public List<int> GetThoughness(){
            IEnumerable<Card> cards = GetCards();
            List<int> thoughnessList = new List<int>();
            foreach(var card in cards){
                var value = 0;
                int.TryParse(card.Toughness, out value);
                if(!thoughnessList.Contains(value) && card.Toughness != null){
                    thoughnessList.Add(value);
                }
            }
            thoughnessList.Sort();
            return thoughnessList;
        }

        public List<int> GetManaCosts(){
            IEnumerable<Card> cards = GetCards();
            List<int> manaList = new List<int>();
            foreach(var card in cards){
                var value = 0;
                int.TryParse(card.ConvertedManaCost, out value);
                if(!manaList.Contains(value) && card.ConvertedManaCost != null){

                    manaList.Add(value);
                }
            }
            manaList.Sort();
            return manaList;
        }
    }
}