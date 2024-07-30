using System.Collections.Generic;
using System.Linq;
using Task2.CustomDataTypes;

namespace Task2.Algorithms;
/// <summary>
/// Класс игровой доски.
/// </summary>
internal sealed class Board
{
  #region Константы
  /// <summary>
  /// Символ использующийся игровой доской как пустой.
  /// </summary>
  public const char EmptySymbol = ' ';

  #endregion

  #region Поля и свойства
  /// <summary>
  /// Клетки игровой доски.
  /// </summary>
  private Dictionary<Cell, char> _playingField;

  public char this[in Cell cell]
  {
    get => this._playingField[cell];
    set => this._playingField[cell] = value;
  }

  #endregion

  #region Методы
  /// <summary>
  /// Приводит игровую доску к начальному состоянию.
  /// </summary>
  public void PrepareBoard()
  {
    this._playingField = new Dictionary<Cell, char>
        {
            { Cell.LeftTop, EmptySymbol },
            { Cell.CenterTop, EmptySymbol },
            { Cell.RightTop, EmptySymbol },
            { Cell.LeftCenter, EmptySymbol },
            { Cell.Center, EmptySymbol },
            { Cell.RightCenter, EmptySymbol },
            { Cell.LeftBottom, EmptySymbol },
            { Cell.CenterBottom, EmptySymbol },
            { Cell.RightBottom, EmptySymbol }
        };
  }
  /// <summary>
  /// Проверяет игровую доску на ничью.
  /// </summary>
  /// <returns>Возвращает <see langword="true"/> если на доске ничья, либо <see langword="false"/> если её нет.</returns>
  public bool IsDraw()
  {
    return this._playingField.All(cell => cell.Value != EmptySymbol);
  }
  /// <summary>
  /// Проверяет игровую доску на победную комбинацию.
  /// </summary>
  /// <returns>Возвращает <see langword="true"/> если на доске победная комбинация, либо <see langword="false"/> если её нет.</returns>
  public bool IsWin()
  {
    var leftTop = this._playingField[Cell.LeftTop];
    var centerTop = this._playingField[Cell.CenterTop];
    var rightTop = this._playingField[Cell.RightTop];
    var leftCenter = this._playingField[Cell.LeftCenter];
    var center = this._playingField[Cell.Center];
    var rightCenter = this._playingField[Cell.RightCenter];
    var leftBottom = this._playingField[Cell.LeftBottom];
    var centerBottom = this._playingField[Cell.CenterBottom];
    var rightBottom = this._playingField[Cell.RightBottom];

    return leftTop == centerTop && centerTop == rightTop && leftTop != EmptySymbol ||
           leftCenter == center && center == rightCenter && leftCenter != EmptySymbol ||
           leftBottom == centerBottom && centerBottom == rightBottom && leftBottom != EmptySymbol ||
           leftTop == leftCenter && leftCenter == leftBottom && leftTop != EmptySymbol ||
           centerTop == center && center == centerBottom && centerTop != EmptySymbol ||
           rightTop == rightCenter && rightCenter == rightBottom && rightTop != EmptySymbol ||
           leftTop == center && center == rightBottom && leftTop != EmptySymbol ||
           rightTop == center && center == leftBottom && rightTop != EmptySymbol;
  }

  #endregion

  #region Конструкторы

  public Board()
  {
    this.PrepareBoard();
  }

  #endregion

}