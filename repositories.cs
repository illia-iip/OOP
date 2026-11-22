using System;
using System.Collections.Generic;
using System.Linq;

interface IPlayerRepository
{
    void Create(Player player);
    List<Player> ReadAll();
    Player ReadById(int id);
    void Update(int id, string name, int currentRating, string accountType);
    void Delete(int id);
}

class PlayerRepository : IPlayerRepository
{
    private DbContext _context;

    public PlayerRepository(DbContext context)
    {
        _context = context;
    }

    public void Create(Player player)
    {
        if (_context.Players.Count == 0)
        {
            player.ID = 1;
        }
        else
        {
            player.ID = _context.Players.Max(p => p.ID) + 1;
        }
        _context.Players.Add(player);
    }

    public List<Player> ReadAll()
    {
        return _context.Players;
    }

    public Player ReadById(int id)
    {
        Player? player = _context.Players.FirstOrDefault(p => p.ID == id);

        if (player == null)
        {
            throw new ArgumentException($"Гравець з ID {id} не знайдений.");
        }
        return player;
    }

    public void Update(int id, string name, int currentRating, string accountType)
    {
        Player existingPlayer = ReadById(id);

        existingPlayer.Name = name;
        existingPlayer.CurrentRating = currentRating;
        existingPlayer.AccountType = accountType;
    }

    public void Delete(int id)
    {
        var playerToRemove = ReadById(id);
        if (playerToRemove != null)
        {
            _context.Players.Remove(playerToRemove);
        }
    }
}

interface IGameRepository
{
    void Create(GameRecord game);
    List<GameRecord> ReadAll();
    GameRecord ReadById(int id);
    List<GameRecord> ReadByPlayerId(int playerId);
    void Update(int id, string opponentName, int rating, string result);
    void Delete(int id);
}

class GameRepository : IGameRepository
{
    private DbContext _context;

    public GameRepository(DbContext context)
    {
        _context = context;
    }

    public void Create(GameRecord game)
    {
        Player? player = _context.Players.FirstOrDefault(p => p.ID == game.PlayerId);

        if (player == null)
        {
            throw new Exception("Гравця не знайдено");
        }

        List<GameRecord> allGames = ReadAll();

        if (allGames.Count == 0)
        {
            game.Id = 1;
        }
        else
        {
            game.Id = allGames.Max(g => g.Id) + 1;
        }

        player.Games.Add(game);
    }

    public List<GameRecord> ReadAll()
    {
        return _context.Players.SelectMany(p => p.Games).ToList();
    }

    public GameRecord ReadById(int id)
    {
        GameRecord? game = ReadAll().FirstOrDefault(g => g.Id == id);

        if (game == null)
        {
            throw new Exception("Гру не знайдено");
        }

        return game;
    }

    public List<GameRecord> ReadByPlayerId(int playerId)
    {
        Player? player = _context.Players.FirstOrDefault(p => p.ID == playerId);

        if (player == null)
        {
            throw new Exception("Гравця не знайдено");
        }

        return player.Games;
    }

    public void Update(int id, string opponentName, int rating, string result)
    {
        GameRecord existingGame = ReadById(id);

        existingGame.OpponentName = opponentName;
        existingGame.Rating = rating;
        existingGame.Result = result;
    }

    public void Delete(int id)
    {
        Player? player = _context.Players.FirstOrDefault(p => p.Games.Any(g => g.Id == id));

        if (player == null)
        {
            throw new Exception("Гру не знайдено");
        }

        GameRecord game = player.Games.First(g => g.Id == id);

        player.Games.Remove(game);
    }
}