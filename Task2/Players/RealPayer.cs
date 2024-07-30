using System;
using Task2.CustomDataTypes;

namespace Task2.Players;
/// <summary>
/// Класс живого игрока.
/// </summary>
/// <param name="name">Имя живого игрока.</param>
/// <param name="symbol">Символ хода игрока.</param>
internal sealed class RealPayer(string name, char symbol) : Player(name, symbol)
{
  #region Методы
  /// <summary>
  /// Спрашивавет у игрока в консоли в какую клетку он будет ходить.
  /// </summary>
  /// <returns>Возвращает <see cref="Cell"/> для хода.</returns>
  public override Cell GetPlayerMove()
  {
    Console.WriteLine($"Игрок {this.Name}, введите номер клетки:");

    while (true)
    {
      var key = Console.ReadKey().Key;
      var cell = KeyToCell(key);

      if (cell == Cell.Wrong)
      {
        Console.WriteLine("\nНужно вводить число от 1 до 9. Введите корректный номер клетки:");
      }
      else
      {
        Console.WriteLine();
        return cell;
      }
    }
  }
  /// <summary>
  /// Интерпритирует нажатую пользователем на клавиатуре кравишу.
  /// </summary>
  /// <param name="key">Нажатая пользователем клавиша.</param>
  /// <returns><see cref="Cell"/> соответсвующая нажатой клавише.</returns>
  private static Cell KeyToCell(in ConsoleKey key)
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

  #endregion
}