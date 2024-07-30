using System;
using Task2.Algorithms;
using Task2.CustomDataTypes;

namespace Task2;

internal class Program
{
  #region Поля и свойства

  //конфигурация игры
  private static TicTacToe _game;
  private const string FirstPlayerName = "Егор";
  private const char FirstPlayerSymbol = 'X';
  private const string SecondPlayerName = "Захар";
  private const char SecondPlayerSymbol = 'O';

  #endregion

  #region Методы

  //начало программы
  private static void Main()
  {
    var mode = IsPlayWithRobot() ? GameMode.PVP : GameMode.PVE;
    _game = new TicTacToe(mode, FirstPlayerName, FirstPlayerSymbol, SecondPlayerName,
      SecondPlayerSymbol);
    _game.Start();
  }

  /// <summary>
  /// Спрашивает у пользователя - будет один или два игрока.
  /// </summary>
  /// <returns> Возвращает <see langword="true"/> если игрок один, либо <see langword="false"/> если игроков двое.</returns>
  private static bool IsPlayWithRobot()
  {
    Console.WriteLine("\nДва игрока?[y/n]");
    return Helper.YesOrNo();
  }

  #endregion
}