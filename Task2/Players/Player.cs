using Task2.CustomDataTypes;

namespace Task2.Players;
/// <summary>
/// Абстрактный класс описывающий игроков.
/// </summary>
/// <param name="name">Имя игрока.</param>
/// <param name="symbol">Символ хода игрока.</param>
internal abstract class Player(string name, char symbol)
{
  /// <summary>
  /// Имя игрока.
  /// </summary>
  public string Name { get; set; } = name;
  /// <summary>
  /// Символ хода игрока.
  /// </summary>
  public char Symbol { get; set; } = symbol;
  /// <summary>
  /// Получает ход игрока.
  /// </summary>
  /// <returns>Возвращет <see cref="Cell"/>, которую игрок выбрал целью хода.</returns>
  public abstract Cell GetPlayerMove();
}