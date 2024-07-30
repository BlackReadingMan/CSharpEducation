using System;
using System.Collections.Generic;

namespace Task2.Algorithms;

/// <summary>
/// Статический класс со вспомогательными функциями.
/// </summary>
internal static class Helper
{
  /// <summary>
  /// Спрашивает у пользователя [y/n].
  /// </summary>
  /// <returns>Возвращает <see langword="true"/> если [y], либо <see langword="false"/> если [n].</returns>
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
  /// <summary>
  /// Выводит строку <see cref="rawtext"/> в консоль, раскрашивая задний фон символа в конретный цвет заданный в <see cref="palette"/>.
  /// </summary>
  /// <param name="rawtext">Строка которую нужно вывести.</param>
  /// <param name="palette">Словарь с парой ключ - значение: символ <see cref="char"/> - цвет для него <see cref="ConsoleColor"/>.</param>
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
}