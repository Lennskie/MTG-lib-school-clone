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
            List<Card> cardListR = new List<Card>();   
            List<Card> cardListM = new List<Card>();   
            List<Card> cardListP = new List<Card>();         
            List<Card> cardListT = new List<Card>();

            if(rarity_code != "null"){
                foreach(var card in cards){
                if( card.RarityCode == rarity_code && !cardListR.Contains(card)){
                        cardListR.Add(card);
                    }
                }
            }else if(converted_mana_cost != "null"){
                foreach(var card in cards){
                if( card.ConvertedManaCost == converted_mana_cost && !cardListM.Contains(card)){
                        cardListM.Add(card);
                    }
                }
            }else if(power != "null"){
                foreach(var card in cards){
                if( card.Power == power && !cardListP.Contains(card)){
                        cardListP.Add(card);
                    }
                }
            }else if(thoughness != "null"){
                foreach(var card in cards ){
                if( card.Toughness == thoughness && !cardListT.Contains(card)){
                        cardListT.Add(card);
                    }
                }
            }
            var endList = cardListR.ToHashSet();
            endList.SymmetricExceptWith(cardListM); 
            endList.SymmetricExceptWith(cardListP);
            endList.SymmetricExceptWith(cardListT);
            // TODO: fix this because filtering one thing works but multiple doesn't really
            return endList.ToList();  
        }

        public List<String> getRarity(){
            IEnumerable<Card> cards = GetCards();
            List<string> rarityList = new List<string>();
            foreach(var card in cards){
                if(!rarityList.Contains(card.RarityCode) && card.RarityCode != null){
                    rarityList.Add(card.RarityCode);
                }
            }
            return rarityList.OrderBy(r=>r).ToList();
        }

        public List<int> getPower(){
            IEnumerable<Card> cards = GetCards();
            List<int> powerList = new List<int>();
            foreach(var card in cards){
                var value = 0;
                int.TryParse(card.Power, out value);
                if(!powerList.Contains(value) && card.Power != null){
                    powerList.Add(value);
                }
            }
            return powerList.OrderBy(p=>p).ToList();
        }

        public List<int> getThoughness(){
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

        public List<int> getManaCosts(){
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