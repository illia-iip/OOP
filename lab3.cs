using System;
using System.Collections.Generic;
using System.Linq;

class GameRecord
{
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public string OpponentName { get; set; }
    public int Rating { get; set; }
    public string Result { get; set; }
}

class Player
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int CurrentRating { get; set; }
    public string AccountType { get; set; }
    public List<GameRecord> Games { get; set; } = new List<GameRecord>();
}

class DbContext
{
    public List<Player> Players { get; set; } = new List<Player>();

    public DbContext()
    {
        Players.Add(new Player
        {
            ID = 1,
            Name = "Illia",
            CurrentRating = 1000,
            AccountType = "Standart"
        });

        Players.Add(new Player
        {
            ID = 2,
            Name = "Dima",
            CurrentRating = 1200,
            AccountType = "HalfLoss"
        });

        Players.Add(new Player
        {
            ID = 3,
            Name = "Sofia",
            CurrentRating = 1500,
            AccountType = "Streak"
        });
    }
}