
using mtg_lib.Library.Models;

namespace mtg_lib.Library.Services;

public class UserCoinService
{
    
    private mtgdevContext context;

    private readonly int _defaultCoins = 100;
    private readonly int _dailyCoins = 50;


    public UserCoinService()
    {
        context = new mtgdevContext();
    }
    
    public IEnumerable<UserCoin> GetUserCoins()
    {
        return context.UserCoins.ToList();
    }

    public void CreateUserCoinForUser(string userId)
    {
        UserCoin userCoin = new UserCoin();
        userCoin.UserId = userId;
        
        Console.WriteLine("Creating userCoin for user");

        context.Add(userCoin);
        context.SaveChanges();
    }
    
    private UserCoin GetUserCoinFromId(string userId)
    {
        IEnumerable<UserCoin> userCoins = GetUserCoins();

        return userCoins.First(uc => uc.UserId == userId);
    }


    public int GetUserCoinBalance(string userId)
    {
        IEnumerable<UserCoin> userCoins = GetUserCoins();

        return userCoins.Where(uc => uc.UserId == userId).Select(c => c.Coins).First();
    }
    
    
    public bool CheckIfReceivedDefaultCoins(string userId)
    {
        UserCoin userCoin = GetUserCoinFromId(userId);

        return userCoin.CoinsClaimed;
    }

    public void ReceiveDefaultCoins(string userId)
    {
        UserCoin userCoin = GetUserCoinFromId(userId);

        if (!userCoin.CoinsClaimed)
        {
            userCoin.Coins += _defaultCoins;
            userCoin.CoinsClaimed = true;
        }

        context.SaveChanges();
    }


    public bool CheckDailyCoinsClaimed(string userId)
    {
        UserCoin userCoin = GetUserCoinFromId(userId);

        DateTime? userCoinDataTime = userCoin.ClaimedTimeStamp;
        
        DateTime? currentDateTime = DateTime.Now;

        var difference = currentDateTime - userCoinDataTime;

        if (difference != null && difference.Value.Hours > 24)
        {
            return false;
        }

        return true;
    }

    public void AddDailyCoins(string userId)
    {
        UserCoin userCoin = GetUserCoinFromId(userId);

        if (CheckDailyCoinsClaimed(userId))
        {
            userCoin.Coins += _dailyCoins;
            userCoin.ClaimedTimeStamp = DateTime.Now;
        }

        context.SaveChanges();
    }


    public void DecreaseUserCoinBalance(string userId, int amount)
    {
        UserCoin userCoin = GetUserCoinFromId(userId);

        int coinBalance = userCoin.Coins;

        userCoin.Coins = coinBalance - amount;

        context.Update(userCoin);
        context.SaveChanges();
    }


}