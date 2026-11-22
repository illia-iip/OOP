using System;
using System.Collections.Generic;

class GameHistory
{
    public string OpponentName;
    public int Rating;
    public string Result;
    public int ID;
    private static int NextID = 1;

    public GameHistory(string opponentName, int rating, string result)
    {
        OpponentName = opponentName;
        Rating = rating;
        Result = result;
        ID = NextID;
        NextID++;
    }
}

abstract class GameAccount{
    string UserName;
    int CurrentRating;
    List<GameHistory> Games = new List<GameHistory>();

    protected virtual int CalculateWin(int rating) => rating; 
    protected virtual int CalculateLoss(int rating) => rating;

    public virtual void WinGame(string OpponentName, Game game)
    {
        int rating = game.CalculateRating();

        if (rating < 0)
        {
            throw new ArgumentException("Рейтинг не може бути від'ємним");
        } 
        
        if (game is TrainingGame)
        {
            Games.Add(new GameHistory(OpponentName, 0, "Training"));
            return;
        }

        int points = CalculateWin(rating);
        CurrentRating += points;

        GameHistory newGame = new GameHistory(OpponentName, points, "Win");
        Games.Add(newGame);
    }

    public virtual void LoseGame(string OpponentName, Game game){
        int rating = game.CalculateRating();

        if (rating < 0)
        {
            throw new ArgumentException("Рейтинг не може бути від'ємним");
        } else if (game is TrainingGame)
        {
            Games.Add(new GameHistory(OpponentName, 0, "Training"));
            return;
        } else
        {
            int points = CalculateLoss(rating);
            if (CurrentRating <= points)
            {
                CurrentRating = 1;
            } else
            {
                CurrentRating -= points;
            }
            GameHistory newGame = new GameHistory(OpponentName, points, "Lose");
            Games.Add(newGame);
        }
    }

    public void GetStats()
    {
        Console.WriteLine($"Статистика гравця: {UserName}");

        foreach (GameHistory game in Games)
        {
            Console.WriteLine(
                $"Гра №{game.ID} | Суперник: {game.OpponentName} | " +
                $"Результат: {game.Result} | Рейтинг: {game.Rating}"
            );
        }
        
        Console.WriteLine($"Кількість зіграних ігор: {Games.Count}");
        Console.WriteLine($"Поточний рейтинг: {CurrentRating}");
    }

    public GameAccount(string userName, int currentRating)
    {
        UserName = userName;
        CurrentRating = currentRating;
    }
}

class StandartAccount : GameAccount{
    public StandartAccount(string userName, int currentRating) : base(userName, currentRating)
    {
    }

}

class HalfLossAccount : GameAccount{
    public HalfLossAccount(string userName, int currentRating) : base(userName, currentRating)
    {
    }

    protected override int CalculateWin(int rating)
    {
        return rating;
    }

    protected override int CalculateLoss(int rating)
    {
        return rating / 2;
    }
}

class StreakAccount : GameAccount{
    public StreakAccount(string userName, int currentRating) : base(userName, currentRating)
    {
    }

    public int Streak = 0;
    double Bonus = 0.1;

    protected override int CalculateWin(int rating)
    {
        Streak++;
        return rating + (int)(rating * Streak * Bonus);
    }

    protected override int CalculateLoss(int rating)
    {
        Streak = 0;
        return rating;
    }
}

abstract class Game
{
    protected int Rating;
    public int ID;
    static int NextID = 1;
    public string PlayerName;

    public Game(string playerName, int rating)
    {
        if (rating < 0)
            throw new ArgumentException("Рейтинг не може бути від'ємним");

        PlayerName = playerName;
        Rating = rating;
        ID = NextID;
        NextID++;
    }

    public abstract int CalculateRating();
}

class StandartGame : Game
{
    public StandartGame(string playerName, int rating) : base(playerName, rating)
    {
}

    public override int CalculateRating() => Rating;
}

class TrainingGame : Game
{
    public TrainingGame(string playerName, int rating) : base(playerName, 0)
    {
    }

    public override int CalculateRating() => 0;
}

class SingleGame : Game
{
    public SingleGame(string playerName, int rating) : base(playerName, rating)
    {
    }

    public override int CalculateRating() => Rating;
}

class GameFactory
{
    public static Game CreateGame(string type, string playerName, int rating)
    {
        switch (type)
        {
            case "Standart":
                return new StandartGame(playerName, rating);

            case "Training":
                return new TrainingGame(playerName, rating);

            case "Single":
                return new SingleGame(playerName, rating);

            default:
                throw new ArgumentException("Невідомий тип гри");
        }
    }
}

class Program
{
    static void Main()
    {
        GameAccount player = new StandartAccount("Illia", 1000);

        Game winGame = GameFactory.CreateGame("Training", "Illia", 100);
        Game loseGame = GameFactory.CreateGame("Training", "Illia", 50000);

        player.WinGame("Oleh", winGame);
        player.LoseGame("Oleh", loseGame);

        player.GetStats();


        GameAccount player2 = new HalfLossAccount("Dima", 1000);

        Game winGame2 = GameFactory.CreateGame("Standart", "Dima", 100);
        Game loseGame2 = GameFactory.CreateGame("Standart", "Dima", 50);
        Game loseGame3 = GameFactory.CreateGame("Standart", "Dima", 201);

        player2.WinGame("Oleh", winGame2);
        player2.LoseGame("Oleh", loseGame2);
        player2.LoseGame("Oleh", loseGame3);

        player2.GetStats();


        GameAccount player3 = new StreakAccount("Sofia", 1000);

        Game winGame4 = GameFactory.CreateGame("Standart", "Sofia", 100);
        Game winGame5 = GameFactory.CreateGame("Standart", "Sofia", 150);
        Game winGame6 = GameFactory.CreateGame("Standart", "Sofia", 200);
        Game loseGame4 = GameFactory.CreateGame("Standart", "Sofia", 80);

        player3.WinGame("Oleh", winGame4);
        player3.WinGame("Oleh", winGame5);
        player3.WinGame("Oleh", winGame6);
        player3.LoseGame("Oleh", loseGame4);

        player3.GetStats();

    }
}