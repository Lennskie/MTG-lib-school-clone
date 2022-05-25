using mtg_lib.Library.Models;

namespace mtg_lib.Library.Services;

public class UserCardService
{
    
    private mtgdevContext context;
    private CardService _cardService = new CardService();

    public UserCardService()
    {
        context = new mtgdevContext();
    }

    public IEnumerable<UserCard> GetUserCards()
    {
        return context.UserCards.ToList();
    }

    public IEnumerable<UserCard> GetUserCardsForUser(string userId)
    {
        return GetUserCards().Where(c => c.UserId == userId).ToList();
    }

    public bool CheckPrecenceCardForUser(string userId, string mtgId)
    {
        Card? card = _cardService.GetCardFromId(mtgId);
        IEnumerable<UserCard> userCards =  GetUserCardsForUser(userId);

        IEnumerable<UserCard> userCards2 = userCards.Where(c => card != null && c.CardId == card.Id);

        if (userCards2.Any() && userCards2.First().Cards > 0)
        {
            return true;
        }

        return false;
    }

    public void AddCardsToUserCards(string userId, IEnumerable<string?> cardMtgIds)
    {
        IEnumerable<UserCard> userCards =  GetUserCardsForUser(userId);
        

        foreach (var cardMtgId in cardMtgIds)
        {
            if (cardMtgId != null)
            {
                Card? card = _cardService.GetCardFromId(cardMtgId);

                bool userCardPresence = userCards.Select(c => c.CardId).Contains(card.Id);

                if (userCardPresence)
                {
                    UserCard userCard = userCards.First(c => c.CardId == card.Id); 
                
                    userCard.Cards += 1;

                    context.UserCards.Update(userCard);
                    context.SaveChanges();
                }
                else
                {
                    UserCard newUserCard = new UserCard();

                    newUserCard.UserId = userId;
                    newUserCard.CardId = card.Id;
                    newUserCard.Cards = 1;

                    context.UserCards.Add(newUserCard);
                    context.SaveChanges();
                }
            }
        }
    }

    public List<Card> RetrieveCardsInUserCollection(string userId){
        List<UserCard> userCards = GetUserCardsForUser(userId).ToList();
        List<Card> cardsInUserCollection = new List<Card>();
        foreach (var card in userCards){
            cardsInUserCollection.Add(_cardService.GetCardFromUserTableId(card.CardId.ToString()));
        }
        return cardsInUserCollection;
    }

    public List<Card> GetCardFromString(string cardName, string userId)
        {
            List<Card> cards = RetrieveCardsInUserCollection(userId);
            List<Card> matches = new List<Card>();

            foreach(var card in cards){
                if(card.Name.Contains(cardName)) matches.Add(card);
            }

            return matches;
        }

        public IEnumerable<Card> GetCardsByFilters(string rarityCode, string convertedManaCost, string power, string thoughness, string userId)
        {
            List<Card> cardListR = GetRarityList(rarityCode, userId);   
            List<Card> cardListM = GetManaList(convertedManaCost, userId);   
            List<Card> cardListP = GetPowerList(power, userId);
            List<Card> cardListT = GetThoughnesList(thoughness, userId);

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

         private List<Card> GetRarityList(string rarityCode, string userId){
            IEnumerable<Card> cards = RetrieveCardsInUserCollection(userId);
            
            if(rarityCode == null){
                return cards.ToList();
            }

            return cards.Where(c=>c.RarityCode == rarityCode).ToList();
         }

        private List<Card> GetManaList(string convertedManaCost, string userId){
            IEnumerable<Card> cards = RetrieveCardsInUserCollection(userId);
            
            if(convertedManaCost == null){
                return cards.ToList();
            }

            return cards.Where(c=>c.ConvertedManaCost == convertedManaCost).ToList();
        }

        private List<Card> GetPowerList(string power, string userId){
            IEnumerable<Card> cards = RetrieveCardsInUserCollection(userId);

            if(power == null){
                return cards.ToList();
            }

            return cards.Where(c=>c.Power == power).ToList();
        }

        private List<Card> GetThoughnesList(string thoughness, string userId){
            IEnumerable<Card> cards = RetrieveCardsInUserCollection(userId);
            
            if(thoughness == null){
                return cards.ToList();
            }
            
            return cards.Where(c=>c.Toughness == thoughness).ToList();
        }

}