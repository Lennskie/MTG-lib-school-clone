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
            List<Card> cardListT = GetThoughnessList(thoughness);
            List<Card> cardListR = GetRarityList(rarity_code);   
            List<Card> cardListM = GetManaList(converted_mana_cost);   
            List<Card> cardListP = GetPowerList(power);

            var disjunction = new HashSet<Card>();
            disjunction.SymmetricExceptWith(cardListR);
            disjunction.SymmetricExceptWith(cardListM);
            disjunction.SymmetricExceptWith(cardListP);
            disjunction.SymmetricExceptWith(cardListT);
            return disjunction;  
        }

        private List<Card> GetThoughnessList(string thoughness){
            IEnumerable<Card> cards = GetCards();
            List<Card> cardListPartial = new List<Card>();
                foreach(var card in cards ){
                if(card.Toughness == thoughness && !cardListPartial.Contains(card)){
                        cardListPartial.Add(card);
                    }
            }
            return cardListPartial;
        }

        private List<Card> GetPowerList(string power){
            IEnumerable<Card> cards = GetCards();
            List<Card> cardListPartial = new List<Card>();
                foreach(var card in cards ){
                if(card.Power == power && !cardListPartial.Contains(card)){
                        cardListPartial.Add(card);
                    }
                }
            return cardListPartial;
        }

        private List<Card> GetRarityList(string rarity_code){
            IEnumerable<Card> cards = GetCards();
            List<Card> cardListPartial = new List<Card>();
            foreach(var card in cards ){
            if(card.RarityCode == rarity_code && !cardListPartial.Contains(card)){
                    cardListPartial.Add(card);
                }
            }
            return cardListPartial;
        }

        private List<Card> GetManaList(string converted_mana_cost){
            IEnumerable<Card> cards = GetCards();
            List<Card> cardListPartial = new List<Card>();
                foreach(var card in cards ){
                if(card.ConvertedManaCost == converted_mana_cost && !cardListPartial.Contains(card)){
                        cardListPartial.Add(card);
                    }
            }
            return cardListPartial;
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