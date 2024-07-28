using System;
using Task2.Algorithms;
using Task2.CustomDataTypes;

namespace Task2;

internal class Program
{
	#region [Поля и свойства]

	//конфигурация игры
	private static TicTacToe _game;
	private const string FirstPlayerName = "Егор";
	private const char FirstPlayerSymbol = 'X';
	private const string SecondPlayerName = "Захар";
	private const char SecondPlayerSymbol = 'O';

	#endregion

	#region [Методы]

	//начало программы
	private static void Main()
	{
		if (IsWithRobot())
			_game = new TicTacToe(GameMode.PVP, FirstPlayerName, FirstPlayerSymbol, SecondPlayerName,
				SecondPlayerSymbol);
		else
			_game = new TicTacToe(GameMode.PVE, FirstPlayerName, FirstPlayerSymbol, SecondPlayerName,
				SecondPlayerSymbol);
		_game.Start();
	}

	//выбор игрока или бота
	private static bool IsWithRobot()
	{
		Console.WriteLine("\nДва игрока?[y/n]");
		return Algorithms.Algorithms.YesOrNo();
	}

	#endregion
}