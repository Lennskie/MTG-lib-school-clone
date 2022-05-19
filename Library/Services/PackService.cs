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

        public IEnumerable<UserPack> GetUserPacks()
        {
            return context.UserPacks.ToList();
        }

        public UserPack GetUserPackFromUserId(string userId)
        {
            return GetUserPacks().First(c => c.UserId == userId);
        }

        public void CreateUserPackForUser(string userId)
        {
            UserPack usersPacks = new UserPack();
            usersPacks.UserId = userId;
        
            Console.WriteLine("Creating userPack for user");

            context.Add(usersPacks);
            context.SaveChanges();
        }

        public void AddPackToUser(string userId, int amountOfPacksToAdd)
        {
            UserPack userPack = GetUserPackFromUserId(userId);

            userPack.Packs += amountOfPacksToAdd;

            context.Update(userPack);
            context.SaveChanges();
        }

        public void DecreasePackCountUser(string userId)
        {
            UserPack userPack = GetUserPackFromUserId(userId);

            int newAmountOfPacks = userPack.Packs - 1;
            if (newAmountOfPacks >= 0)
            {
                userPack.Packs -= 1;
            }
            // Silently "fail", so don't decrease with the request amount
            
            context.Update(userPack);
            context.SaveChanges();
        }

        public IEnumerable<Card> CreateRandomPack(string rarity)
        {
            IEnumerable<Card> cards = cardService.GetCards();
            
            // 15 Cards
            var r = new Random();
            // TODO: Check if the cards packs contain the right amount of cards for each option ...
            
            // 1 Basic Land
            IEnumerable<Card> basicCard = cards.Where(c => c.Type.Contains("Land")).OrderBy(c =>  r.Next(cards.Count() - 1)).Take(1);

            // 10 Commons
            IEnumerable<Card> commonCards = cards.Where(c => c.RarityCode == "C").OrderBy(c =>  r.Next(cards.Count() - 1)).Take(10);

            // 3 Uncommon
            IEnumerable<Card> uncommonCards = cards.Where(c => c.RarityCode == "U").OrderBy(c =>  r.Next(cards.Count() - 1)).Take(3);

            // 1 is rare or mythic
            IEnumerable<Card> rareOrMythicCard = cards.Where(c => c.RarityCode is "R" or "M").OrderBy(c => r.Next(cards.Count() - 1)).Take(1);

            IEnumerable<Card> cardPacks = basicCard.Concat(commonCards).Concat(uncommonCards).Concat(rareOrMythicCard).Select(c => c).ToList();

            return cardPacks;
        }

        private IEnumerable<Card> appendToList(IEnumerable<Card> toAppend)
        {
            IEnumerable<Card> toReturn = new List<Card>();

            return toReturn;
        }


        public IEnumerable<Card> retrievePacks()
        {
            return new List<Card>();
        }


        public IEnumerable<Card> retrievePackInfo(string packId)
        {
            return new List<Card>();
        }

    }
}

