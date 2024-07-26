using Task2.Algorithms;
using Task2.CustomDataTypes;

namespace Task2;

internal class Program
{
	private static void Main()
	{
		var game = new TicTacToe('X', 'O');
		var palette = new Dictionary<char, ConsoleColor>
		{
			{ game.FirstPlayerSymbol, ConsoleColor.Red },
			{ game.SecondPlayerSymbol, ConsoleColor.Blue }
		};
		var flag = true;
		var mode = true;
		while (flag)
		{
			Console.WriteLine("Два игрока?[y/n]");
			switch (Console.ReadKey().Key)
			{
				case ConsoleKey.Y:
					flag = false;
					mode = true;
					break;
				case ConsoleKey.N:
					flag = false;
					mode = false;
					break;
				default:
					Console.WriteLine("Введите [y] либо [n]:");
					break;
			}
		}

		Console.WriteLine();
		Console.WriteLine(game.Start());

		while (true)
		{
			var key = Console.ReadKey().Key;
			if (key == ConsoleKey.Escape)
				break;
			var cell = KeyToCell(key);

			Console.WriteLine();
			if (!game.MakeMove(cell, out var output))
			{
				Console.WriteLine(output);
				continue;
			}

			ColorWrite(output, palette, true);
			if (!mode)
				ColorWrite(game.ComputerMove(), palette, true);
		}
	}

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

	private static void ColorWrite(string rawtext, Dictionary<char, ConsoleColor> palette, bool endline)
	{
		foreach (var c in rawtext)
		{
			if (palette.TryGetValue(c, out var color))
			{
				Console.BackgroundColor = color;
				Console.Write(c);
				Console.ResetColor();
				continue;
			}

			Console.Write(c);
		}

		if (endline)
		{
			Console.WriteLine();
		}

	}
}