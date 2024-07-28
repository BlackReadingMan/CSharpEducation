using System;
using System.Collections.Generic;
using Task2.CustomDataTypes;

namespace Task2.Algorithms;

internal static class Algorithms
{
	public static bool YesOrNo()
	{
		while (true)
		{
			switch (Console.ReadKey().Key)
			{
				case ConsoleKey.Y:
					Console.WriteLine();
					return false;
				case ConsoleKey.N:
					Console.WriteLine();
					return true;
			}
			Console.WriteLine("Неккоректный ответ. [y] либо [n]");
		}
	}
	public static void ColorWrite(in string rawtext, Dictionary<char, ConsoleColor> palette)
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

		Console.WriteLine();
	}
	public static char FindAnotherSymbol(Board board, in char symbol)
	{
		for (var i = 0; i < 9; i++)
		{
			if (board[(Cell)i] == symbol || board[(Cell)i] == Board.EmptySymbol) continue;
			return board[(Cell)i];
		}

		return (char)(symbol + 1);
	}
}