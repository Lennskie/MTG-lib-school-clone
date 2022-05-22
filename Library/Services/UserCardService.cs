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

        IEnumerable<UserCard> userCardsFiltered = userCards.Where(c => c.CardId == card.Id);
        
        if (userCardsFiltered.Any() && userCardsFiltered.First().Cards >= 1)
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


}