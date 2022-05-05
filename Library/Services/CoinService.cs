using System.Runtime.InteropServices;
using mtg_lib.Library.Models;

namespace mtg_lib.Library.Services;

public class CoinService
{
    
    private mtgdevContext context;

    private readonly int _defaultCoins = 100;
    private readonly int _dailyCoins = 50;


    public CoinService()
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
    
    
    public bool checkIfReceivedDefaultCoins(string userId)
    {
        UserCoin userCoin = GetUserCoinFromId(userId);

        return userCoin.CoinsClaimed;
    }

    public void receiveDefaultCoins(string userId)
    {
        UserCoin userCoin = GetUserCoinFromId(userId);

        if (!userCoin.CoinsClaimed)
        {
            userCoin.Coins += _defaultCoins;
        }

        context.SaveChanges();
    }


    public bool checkDailyCoinsClaimed(string userId)
    {
        UserCoin userCoin = GetUserCoinFromId(userId);

        DateTime? userCoinDataTime = userCoin.ClaimedTimeStamp;
        
        DateTime? currentDateTime = DateTime.Now;

        var difference = currentDateTime - userCoinDataTime;

        if (difference.Value.Hours > 24)
        {
            return true;
        }

        return false;
    }

    public void addDailyCoins(string userId)
    {
        UserCoin userCoin = GetUserCoinFromId(userId);

        DateTime? userCoinDataTime = userCoin.ClaimedTimeStamp;
        
        DateTime? currentDateTime = DateTime.Now;

        if (checkDailyCoinsClaimed(userId))
        {
            userCoin.Coins += _dailyCoins;
        }

        context.SaveChanges();
    }


}