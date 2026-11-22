using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        DbContext context = new DbContext();

        IPlayerRepository playerRepository = new PlayerRepository(context);

        IGameRepository gameRepository = new GameRepository(context);

        IPlayerService playerService = new PlayerService(playerRepository);

        IGameService gameService = new GameService(gameRepository, playerService);

        Console.WriteLine("\nСтворення гравців:");

        playerService.CreateAccount("Andrii", "Standart");
        playerService.CreateAccount("Olena", "HalfLoss");

        Console.WriteLine("Гравців успішно створено.\n");

        Console.WriteLine("\nВсі гравці:");

        foreach (Player player in playerService.ReadPlayers())
        {
            Console.WriteLine(
                $"ID: {player.ID} | " +
                $"Ім'я: {player.Name} | " +
                $"Рейтинг: {player.CurrentRating} | " +
                $"Тип: {player.AccountType}"
            );
        }


        Console.WriteLine("\nПошук гравця з ID 4:");

        Player foundPlayer = playerService.ReadPlayerById(4);

        Console.WriteLine(
            $"ID: {foundPlayer.ID} | " +
            $"Ім'я: {foundPlayer.Name} | " +
            $"Рейтинг: {foundPlayer.CurrentRating} | " +
            $"Тип: {foundPlayer.AccountType}"
        );


        Console.WriteLine("\nОновлення даних гравця з ID 4:");

        playerService.UpdatePlayer(4, "Andrii Updated", 2000, "HalfLoss");

        Player updatedPlayer = playerService.ReadPlayerById(4);

        Console.WriteLine(
            $"ID: {updatedPlayer.ID} | " +
            $"Ім'я: {updatedPlayer.Name} | " +
            $"Рейтинг: {updatedPlayer.CurrentRating} | " +
            $"Тип: {updatedPlayer.AccountType}"
        );


        Console.WriteLine("\nСтворення ігор:");

        Game game1 = GameFactory.CreateGame("Standart", "Andrii", 100);

        Game game2 = GameFactory.CreateGame("Standart", "Andrii", 50);

        Game game3 = GameFactory.CreateGame("Training", "Andrii", 500);

        gameService.WinGame(4, "Oleh", game1);
        gameService.LoseGame(4, "Dima", game2);
        gameService.WinGame(4, "Sofia", game3);

        Console.WriteLine("Ігри успішно записані до бази даних.");


        Console.WriteLine("\nВсі ігри:");

        foreach (GameRecord game in gameService.ReadGames())
        {
            Console.WriteLine(
                $"ID: {game.Id} | " +
                $"Гравець ID: {game.PlayerId} | " +
                $"Суперник: {game.OpponentName} | " +
                $"Результат: {game.Result} | " +
                $"Рейтинг: {game.Rating}"
            );
        }


        Console.WriteLine("\nПошук гри з ID 1:");

        GameRecord foundGame = gameService.ReadGameById(1);

        Console.WriteLine(
            $"ID: {foundGame.Id} | " +
            $"Суперник: {foundGame.OpponentName} | " +
            $"Результат: {foundGame.Result} | " +
            $"Рейтинг: {foundGame.Rating}"
        );


        Console.WriteLine("\nІгри гравця з ID 4:");

        foreach (GameRecord game in gameService.ReadPlayerGames(4))
        {
            Console.WriteLine(
                $"ID: {game.Id} | " +
                $"Суперник: {game.OpponentName} | " +
                $"Результат: {game.Result} | " +
                $"Рейтинг: {game.Rating}"
            );
        }


        Console.WriteLine("\nОновлення даних про гру з ID 1:");

        gameService.UpdateGame(1, "Petro", 150, "Lose");

        GameRecord updatedGame = gameService.ReadGameById(1);

        Console.WriteLine(
            $"ID: {updatedGame.Id} | " +
            $"Суперник: {updatedGame.OpponentName} | " +
            $"Результат: {updatedGame.Result} | " +
            $"Рейтинг: {updatedGame.Rating}"
        );


        Console.WriteLine("\nВидалення гри з ID 2:");

        gameService.DeleteGame(2);

        Console.WriteLine("Гру з ID 2 видалено.");

        Console.WriteLine("Ігри після видалення:");

        foreach (GameRecord game in gameService.ReadGames())
        {
            Console.WriteLine(
                $"ID: {game.Id} | " +
                $"Суперник: {game.OpponentName} | " +
                $"Результат: {game.Result}"
            );
        }


        Console.WriteLine("\nВидалення гравця з ID 5:");

        playerService.DeletePlayer(5);

        Console.WriteLine("Гравця з ID 5 видалено.");

        Console.WriteLine("Гравці після видалення:");

        foreach (Player player in playerService.ReadPlayers())
        {
            Console.WriteLine(
                $"ID: {player.ID} | " +
                $"Ім'я: {player.Name} | " +
                $"Рейтинг: {player.CurrentRating} | " +
                $"Тип: {player.AccountType}"
            );
        }
    }
}
