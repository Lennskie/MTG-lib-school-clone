﻿using mtg_lib.Library.Models;


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
            List<Card> cardListR = GetRarityList(rarity_code);   
            List<Card> cardListM = GetManaList(converted_mana_cost);   
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

        private List<Card> GetRarityList(string rarity_code){
            IEnumerable<Card> cards = GetCards();
            List<Card> cardListPartial = new List<Card>();
            if(rarity_code == null){
                return cards.ToList();
            }else{
                return cards.Where(c=>c.RarityCode == rarity_code).ToList();
            }
        }

        private List<Card> GetManaList(string converted_mana_cost){
            IEnumerable<Card> cards = GetCards();
            List<Card> cardListPartial = new List<Card>();
            if(converted_mana_cost == null){
                return cards.ToList();
            }else{
                return cards.Where(c=>c.ConvertedManaCost == converted_mana_cost).ToList();
            }
        }

        private List<Card> GetPowerList(string power){
            IEnumerable<Card> cards = GetCards();
            List<Card> cardListPartial = new List<Card>();
            if(power == null){
                return cards.ToList();
            }else{
                return cards.Where(c=>c.Power == power).ToList();
            }
        }

        private List<Card> GetThoughnessList(string thoughness){
            IEnumerable<Card> cards = GetCards();
            List<Card> cardListPartial = new List<Card>();
            if(thoughness == null){
                return cards.ToList();
            }else{
                return cards.Where(c=>c.Toughness == thoughness).ToList();
            }
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