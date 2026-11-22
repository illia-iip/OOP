using System;
using System.Collections.Generic;
using System.Linq;

interface IPlayerService
{
    GameAccount CreateAccount(string name, string accountType);

    List<Player> ReadPlayers();

    Player ReadPlayerById(int id);

    GameAccount GetAccountById(int id);

    void UpdatePlayer(int id, string name, int currentRating, string accountType);

    void DeletePlayer(int id);
}

class PlayerService : IPlayerService
{
    private IPlayerRepository _playerRepository;

    private Dictionary<int, GameAccount> _accounts = new Dictionary<int, GameAccount>();

    public PlayerService(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;

        foreach (Player player in _playerRepository.ReadAll())
        {
            _accounts.Add(player.ID, CreateGameAccount(player));
        }
    }

    public GameAccount CreateAccount(string name, string accountType)
    {
        Player player = new Player
        {
            Name = name,
            CurrentRating = 1000,
            AccountType = accountType
        };

        GameAccount account = CreateGameAccount(player);
        _playerRepository.Create(player);
        _accounts.Add(player.ID, account);

        return account;
    }

    private GameAccount CreateGameAccount(Player player)
    {
        switch (player.AccountType)
        {
            case "Standart":
                return new StandartAccount(player);

            case "HalfLoss":
                return new HalfLossAccount(player);

            case "Streak":
                return new StreakAccount(player);

            default:
                throw new ArgumentException("Невідомий тип акаунта");
        }
    }

    public List<Player> ReadPlayers()
    {
        return _playerRepository.ReadAll();
    }

    public Player ReadPlayerById(int id)
    {
        return _playerRepository.ReadById(id);
    }

    public GameAccount GetAccountById(int id)
    {
        if (!_accounts.ContainsKey(id))
        {
            throw new Exception("Акаунт не знайдено");
        }

        return _accounts[id];
    }


    public void UpdatePlayer(int id, string name, int currentRating, string accountType)
    {
        Player existingPlayer = _playerRepository.ReadById(id);

        string oldAccountType = existingPlayer.AccountType;

        _playerRepository.Update(id, name, currentRating, accountType);

        if (oldAccountType != accountType)
        {
            _accounts[id] = CreateGameAccount(existingPlayer);
        }
    }

    public void DeletePlayer(int id)
    {
        _playerRepository.Delete(id);

        _accounts.Remove(id);
    }
}


interface IGameService
{
    GameRecord WinGame(int playerId, string opponentName, Game game);

    GameRecord LoseGame(int playerId, string opponentName, Game game);

    List<GameRecord> ReadGames();

    List<GameRecord> ReadPlayerGames(int playerId);

    GameRecord ReadGameById(int id);

    void UpdateGame(int id, string opponentName, int rating, string result);

    void DeleteGame(int id);
}


class GameService : IGameService
{
    private IGameRepository _gameRepository;
    private IPlayerService _playerService;

    public GameService(IGameRepository gameRepository, IPlayerService playerService)
    {
        _gameRepository = gameRepository;
        _playerService = playerService;
    }

    public GameRecord WinGame(int playerId, string opponentName, Game game)
    {
        GameAccount account = _playerService.GetAccountById(playerId);

        GameRecord record = account.WinGame(opponentName, game);

        _gameRepository.Create(record);

        return record;
    }

    public GameRecord LoseGame(int playerId, string opponentName, Game game)
    {
        GameAccount account = _playerService.GetAccountById(playerId);

        GameRecord record = account.LoseGame(opponentName, game);

        _gameRepository.Create(record);

        return record;
    }

    public List<GameRecord> ReadGames()
    {
        return _gameRepository.ReadAll();
    }

    public List<GameRecord> ReadPlayerGames(int playerId)
    {
        return _gameRepository.ReadByPlayerId(playerId);
    }

    public GameRecord ReadGameById(int id)
    {
        return _gameRepository.ReadById(id);
    }

    public void UpdateGame(int id, string opponentName, int rating, string result)
    {
        _gameRepository.Update(id, opponentName, rating, result);
    }

    public void DeleteGame(int id)
    {
        _gameRepository.Delete(id);
    }
}
