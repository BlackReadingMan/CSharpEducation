using System.Collections.Generic;
using System.Linq;
using Task2.CustomDataTypes;
using Task2.Players;

namespace Task2.Algorithms;
/// <summary>
/// Класс игровой доски.
/// </summary>
internal sealed class Board
{
  #region Поля и свойства
  /// <summary>
  /// Клетки игровой доски.
  /// </summary>
  private Dictionary<Cell, Player?> _playingField;

  public Player? this[in Cell cell]
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
    this._playingField = new Dictionary<Cell, Player?>
        {
            { Cell.LeftTop, null },
            { Cell.CenterTop, null },
            { Cell.RightTop, null },
            { Cell.LeftCenter, null },
            { Cell.Center, null },
            { Cell.RightCenter, null },
            { Cell.LeftBottom, null },
            { Cell.CenterBottom, null },
            { Cell.RightBottom, null }
        };
  }
  /// <summary>
  /// Проверяет игровую доску на ничью.
  /// </summary>
  /// <returns>Возвращает <see langword="true"/> если на доске ничья, либо <see langword="false"/> если её нет.</returns>
  public bool IsDraw()
  {
    return this._playingField.All(cell => cell.Value is not null);
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

    return leftTop == centerTop && centerTop == rightTop && leftTop is not null ||
           leftCenter == center && center == rightCenter && leftCenter is not null ||
           leftBottom == centerBottom && centerBottom == rightBottom && leftBottom is not null ||
           leftTop == leftCenter && leftCenter == leftBottom && leftTop is not null ||
           centerTop == center && center == centerBottom && centerTop is not null ||
           rightTop == rightCenter && rightCenter == rightBottom && rightTop is not null ||
           leftTop == center && center == rightBottom && leftTop is not null ||
           rightTop == center && center == leftBottom && rightTop is not null;
  }

  #endregion

  #region Конструкторы

  public Board()
  {
    this.PrepareBoard();
  }

  #endregion

}