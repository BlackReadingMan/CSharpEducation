using Task2.Algorithms;
using Task2.CustomDataTypes;

namespace Task2;

internal class Program
{
	//конфигурация игры
	private static readonly TicTacToe Game = new(Player.Player1, 'X', 'O');

	//цвета для вывода символов игроков
	private static readonly Dictionary<char, ConsoleColor> Palette = new()
	{
		{ Game.FirstPlayerSymbol, ConsoleColor.Red },
		{ Game.SecondPlayerSymbol, ConsoleColor.Blue }
	};

	//начало программы
	private static void Main()
	{
		if (IsWithRobot())
			StartPlayWithRobot();
		else
			StartPlayWithPayer();
	}

	//выбор игрока или бота
	private static bool IsWithRobot()
	{
		while (true)
		{
			Console.WriteLine("\nДва игрока?[y/n]");
			switch (Console.ReadKey().Key)
			{
				case ConsoleKey.Y:
					return false;
				case ConsoleKey.N:
					return true;
			}
		}
	}

	//игра двух игроков
	private static void StartPlayWithPayer()
	{
		Console.WriteLine();
		while (true)
		{
			ColorWrite(DrawBoard());
			var position = GetPlayerKey();
			Console.WriteLine();
			MakeMove(position);
		}
	}

	//игра против бота
	private static void StartPlayWithRobot()
	{
		Console.WriteLine();
		while (true)
		{
			switch (Game.CurrentPlayer)
			{
				case Player.Player1:
					ColorWrite(DrawBoard());
					var position = GetPlayerKey();
					Console.WriteLine();
					MakeMove(position);
					break;
				case Player.Player2_Or_AI:
					ColorWrite(DrawBoard());
					Console.WriteLine("\nХод бота");
					MakeRobotMove();
					break;
			}
		}
	}

	//получение хода игроков
	private static Cell GetPlayerKey()
	{
		Console.WriteLine(Game.CurrentPlayer == Player.Player1 ? "\nХод игрока 1:" : "\nХод игрока 2:");
		while (true)
		{
			var key = Console.ReadKey().Key;
			var cell = KeyToCell(key);
			if (cell == Cell.Wrong)
			{
				Console.WriteLine("\nНужно вводить число от 1 до 9. Введите корректный номер клетки:");
				continue;
			}

			if (Game.Board[cell] != 'X' && Game.Board[cell] != 'O') return cell;
			Console.WriteLine("\nЭто клетка уже занята. Введите корректный номер клетки:");
		}
	}

	//обработка хода игрока
	private static void MakeMove(Cell cell)
	{
		var result = Game.MakeMove(cell);

		switch (result)
		{
			case MoveResult.Win:
				ColorWrite(DrawBoard());
				Console.WriteLine(Game.CurrentPlayer == Player.Player1 ? "ПОБЕДИЛ ИГРОК 1\n" : "ПОБЕДИЛ ИГРОК 2\n");
				WriteStats();
				Game.NextIteration();
				break;
			case MoveResult.Draw:
				ColorWrite(DrawBoard());
				Console.WriteLine("НИЧЬЯ\n");
				WriteStats();
				Game.NextIteration();
				break;
		}
	}

	//обработка хода бота
	private static void MakeRobotMove()
	{
		var result = Game.ComputerMove();

		switch (result)
		{
			case MoveResult.Win:
				ColorWrite(DrawBoard());
				Console.WriteLine("ПОБЕДИЛ РОБОТ. ПЛОТЬ СЛАБА.\n");
				WriteStatsWithRobot();
				Game.NextIteration();
				break;
			case MoveResult.Draw:
				ColorWrite(DrawBoard());
				Console.WriteLine("НА ЭТО РАЗ НИЧЬЯ КОЖАНЫЙ\n");
				WriteStatsWithRobot();
				Game.NextIteration();
				break;
		}
	}

	//вывод статистики для игроков
	private static void WriteStats()
	{
		Console.WriteLine($"Игрок 1: {Game.FirstPlayerWinsCount}");
		Console.WriteLine($"Ничьих: {Game.DrawsCount}");
		Console.WriteLine($"Игрок 2: {Game.SecondPlayerWinsCount}\n");
	}

	//вывод статистики для робота
	private static void WriteStatsWithRobot()
	{
		Console.WriteLine($"Игрок 1: {Game.FirstPlayerWinsCount}");
		Console.WriteLine($"Ничьих: {Game.DrawsCount}");
		Console.WriteLine($"Робот: {Game.SecondPlayerWinsCount}\n");
	}

	//вывод игровой доски
	private static string DrawBoard()
	{
		return "-------------\n" +
		       $"| {Game.Board[Cell.LeftTop]} | {Game.Board[Cell.CenterTop]} | {Game.Board[Cell.RightTop]} |\n" +
		       "-------------\n" +
		       $"| {Game.Board[Cell.LeftCenter]} | {Game.Board[Cell.Center]} | {Game.Board[Cell.RightCenter]} |\n" +
		       "-------------\n" +
		       $"| {Game.Board[Cell.LeftBottom]} | {Game.Board[Cell.CenterBottom]} | {Game.Board[Cell.RightBottom]} |\n" +
		       "-------------\n";
	}

	//интерпритиция нажатой игроком клавиши
	private static Cell KeyToCell(ConsoleKey key)
	{
		return key switch
		{
			ConsoleKey.D1 => Cell.LeftTop,
			ConsoleKey.D2 => Cell.CenterTop,
			ConsoleKey.D3 => Cell.RightTop,
			ConsoleKey.D4 => Cell.LeftCenter,
			ConsoleKey.D5 => Cell.Center,
			ConsoleKey.D6 => Cell.RightCenter,
			ConsoleKey.D7 => Cell.LeftBottom,
			ConsoleKey.D8 => Cell.CenterBottom,
			ConsoleKey.D9 => Cell.RightBottom,
			ConsoleKey.NumPad1 => Cell.LeftTop,
			ConsoleKey.NumPad2 => Cell.CenterTop,
			ConsoleKey.NumPad3 => Cell.RightTop,
			ConsoleKey.NumPad4 => Cell.LeftCenter,
			ConsoleKey.NumPad5 => Cell.Center,
			ConsoleKey.NumPad6 => Cell.RightCenter,
			ConsoleKey.NumPad7 => Cell.LeftBottom,
			ConsoleKey.NumPad8 => Cell.CenterBottom,
			ConsoleKey.NumPad9 => Cell.RightBottom,
			_ => Cell.Wrong
		};
	}

	//цветной вывод игровой доски
	private static void ColorWrite(string rawtext)
	{
		foreach (var c in rawtext)
		{
			if (Palette.TryGetValue(c, out var color))
			{
				Console.BackgroundColor = color;
				Console.Write(c);
				Console.ResetColor();
				continue;
			}

			Console.Write(c);
		}
	}
}