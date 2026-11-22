using System;
using System.Collections.Generic;

abstract class GameAccount{
    protected Player Player;

    protected virtual int CalculateWin(int rating) => rating; 
    protected virtual int CalculateLoss(int rating) => rating;

    public GameAccount(Player player)
    {
        Player = player;
    }

    public virtual GameRecord WinGame(string opponentName, Game game)
    {
        int rating = game.CalculateRating();

        if (rating < 0)
        {
            throw new ArgumentException("Рейтинг не може бути від'ємним");
        } 
        
        if (game is TrainingGame)
        {
            return new GameRecord
            {
                PlayerId = Player.ID,
                OpponentName = opponentName,
                Rating = 0,
                Result = "Training"
            };
        }

        int points = CalculateWin(rating);
        Player.CurrentRating += points;

        return new GameRecord
        {
            PlayerId = Player.ID,
            OpponentName = opponentName,
            Rating = points,
            Result = "Win"
        };
    }

    public virtual GameRecord LoseGame(string opponentName, Game game){
        int rating = game.CalculateRating();

        if (rating < 0)
        {
            throw new ArgumentException("Рейтинг не може бути від'ємним");
        }
        else if (game is TrainingGame)
        {
            return new GameRecord
            {
                PlayerId = Player.ID,
                OpponentName = opponentName,
                Rating = 0,
                Result = "Training"
            };
        } else
        {
            int points = CalculateLoss(rating);
            if (Player.CurrentRating <= points)
            {
                Player.CurrentRating = 1;
            }else
            {
                Player.CurrentRating -= points;
            }

            return new GameRecord
            {
                PlayerId = Player.ID,
                OpponentName = opponentName,
                Rating = points,
                Result = "Lose"
            };
        }
    }
}

class StandartAccount : GameAccount
{
    public StandartAccount(Player player) : base(player)
    {
    }
}

class HalfLossAccount : GameAccount
{
    public HalfLossAccount(Player player) : base(player)
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

class StreakAccount : GameAccount
{
    public int Streak = 0;
    double Bonus = 0.1;

    public StreakAccount(Player player) : base(player)
    {
    }

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