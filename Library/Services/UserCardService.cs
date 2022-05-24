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

        IEnumerable<UserCard> userCards2 = userCards.Where(c => c.CardId == card.Id);

        if (userCards2.Any() && userCards2.First().Cards > 0)
        {
            return true;
        }

        return false;
    }

    public void AddCardsToUserCards(string userId, IEnumerable<string> cardMtgIds)
    {
        IEnumerable<UserCard> userCards =  GetUserCardsForUser(userId);
        

        foreach (var cardMtgId in cardMtgIds)
        {
            Card? card = _cardService.GetCardFromId(cardMtgId);
            //Console.WriteLine("MtgID: " + cardMtgId + " Card Id: " + card.Id);

            bool userCardPresence = userCards.Select(c => c.CardId).Contains(card.Id);

            if (userCardPresence)
            {
                UserCard userCard = userCards.First(c => c.CardId == card.Id); 
                
                userCard.Cards += 1;

                context.UserCards.Update(userCard);
                context.SaveChanges();
                    
                //Console.WriteLine("Updating existing userCard");
            }
            else
            {
                UserCard newUserCard = new UserCard();

                newUserCard.UserId = userId;
                newUserCard.CardId = card.Id;
                newUserCard.Cards = 1;

                context.UserCards.Add(newUserCard);
                context.SaveChanges();
                
                //Console.WriteLine("Adding new UserCard");
            }
            
        }
    }

    public List<Card> retrieveCardsInUserCollection(string userId){
        List<UserCard> userCards = GetUserCardsForUser(userId).ToList();
        List<Card> convertList = new List<Card>();
        foreach (var card in userCards){
            convertList.Add(_cardService.GetCardFromUserTableId(card.CardId.ToString()));
        }
        return convertList;
    }

    public List<Card> GetCardFromString(string cardName, string userId)
        {
            List<Card> cards = retrieveCardsInUserCollection(userId);
            List<Card> matches = new List<Card>();

            foreach(var card in cards){
                if(card.Name.Contains(cardName)) matches.Add(card);
            }

            return matches;
        }


}