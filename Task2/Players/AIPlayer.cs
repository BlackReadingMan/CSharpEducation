using System;
using Task2.Algorithms;
using Task2.CustomDataTypes;

namespace Task2.Players;

/// <summary>
/// Класс игрока искуственного интелекта.
/// </summary>
/// <param name="name">Имя бота.</param>
/// <param name="symbol">Символ хода игрока.</param>
/// <param name="board">Игровая доска для которой расчитывать ходы.</param>
internal sealed class AiPlayer(string name, char symbol, Board board) : Player(name, symbol)
{
  /// <summary>
  /// Символ другого игрока.
  /// </summary>
  private char _anotherSymbol;

  #region Методы

  /// <summary>
  /// Расчитывает идеальной ход ИИ.
  /// </summary>
  /// <returns>Возвращает <see cref="Cell"/> для идеального хода.</returns>
  public override Cell GetPlayerMove()
  {
    Console.WriteLine($"Ходит робот {this.Name}:");
    this._anotherSymbol = this.FindAnotherSymbol();
    var bestVal = int.MinValue;
    var bestPosition = Cell.Wrong;
    if (IsFirstStep())
    {
      bestPosition = Cell.Center;
    }
    else
    {
      for (var i = 0; i < 9; i++)
      {
        if (board[(Cell)i] == this.Symbol ||
            board[(Cell)i] == this._anotherSymbol) continue;
        board[(Cell)i] = this.Symbol;
        var moveVal = this.Minimax(0, false);
        board[(Cell)i] = ' ';

        if (moveVal <= bestVal) continue;
        bestPosition = (Cell)i;
        bestVal = moveVal;
      }
    }

    Console.WriteLine($"{(int)(bestPosition + 1)}");
    return bestPosition;
  }

  /// <summary>
  /// Алгоритм МИНИМАКС.
  /// </summary>
  private int Minimax(in int depth, in bool isMax)
  {
    var score = this.Evaluate();

    switch (score)
    {
      case 10:
        return score - depth;
      case -10:
        return score + depth;
    }

    if (board.IsDraw()) return 0;

    if (isMax)
    {
      var best = int.MinValue;
      for (var i = 0; i < 9; i++)
      {
        if (board[(Cell)i] == this.Symbol ||
            board[(Cell)i] == this._anotherSymbol) continue;
        board[(Cell)i] = this.Symbol;
        best = Math.Max(best, this.Minimax(depth + 1, !isMax));
        board[(Cell)i] = ' ';
      }

      return best;
    }
    else
    {
      var best = int.MaxValue;
      for (var i = 0; i < 9; i++)
      {
        if (board[(Cell)i] == this.Symbol ||
            board[(Cell)i] == this._anotherSymbol) continue;
        board[(Cell)i] = this._anotherSymbol;
        best = Math.Min(best, this.Minimax(depth + 1, !isMax));
        board[(Cell)i] = ' ';
      }

      return best;
    }
  }

  private int Evaluate()
  {
    if (board[Cell.LeftTop] == board[Cell.CenterTop] && board[Cell.CenterTop] == board[Cell.RightTop])
    {
      if (board[Cell.LeftTop] == this.Symbol)
        return +10;
      if (board[Cell.LeftTop] == this._anotherSymbol)
        return -10;
    }

    if (board[Cell.LeftCenter] == board[Cell.Center] && board[Cell.Center] == board[Cell.RightCenter])
    {
      if (board[Cell.LeftCenter] == this.Symbol)
        return +10;
      if (board[Cell.LeftCenter] == this._anotherSymbol)
        return -10;
    }

    if (board[Cell.LeftBottom] == board[Cell.CenterBottom] &&
        board[Cell.CenterBottom] == board[Cell.RightBottom])
    {
      if (board[Cell.LeftBottom] == this.Symbol)
        return +10;
      if (board[Cell.LeftBottom] == this._anotherSymbol)
        return -10;
    }

    if (board[Cell.LeftTop] == board[Cell.LeftCenter] && board[Cell.LeftCenter] == board[Cell.LeftBottom])
    {
      if (board[Cell.LeftTop] == this.Symbol)
        return +10;
      if (board[Cell.LeftTop] == this._anotherSymbol)
        return -10;
    }

    if (board[Cell.CenterTop] == board[Cell.Center] && board[Cell.Center] == board[Cell.CenterBottom])
    {
      if (board[Cell.CenterTop] == this.Symbol)
        return +10;
      if (board[Cell.CenterTop] == this._anotherSymbol)
        return -10;
    }

    if (board[Cell.RightTop] == board[Cell.RightCenter] && board[Cell.RightCenter] == board[Cell.RightBottom])
    {
      if (board[Cell.RightTop] == this.Symbol)
        return +10;
      if (board[Cell.RightTop] == this._anotherSymbol)
        return -10;
    }

    if (board[Cell.LeftTop] == board[Cell.Center] && board[Cell.Center] == board[Cell.RightBottom])
    {
      if (board[Cell.LeftTop] == this.Symbol)
        return +10;
      if (board[Cell.LeftTop] == this._anotherSymbol)
        return -10;
    }

    if (board[Cell.RightTop] == board[Cell.Center] && board[Cell.Center] == board[Cell.LeftBottom])
    {
      if (board[Cell.RightTop] == this.Symbol)
        return +10;
      if (board[Cell.RightTop] == this._anotherSymbol)
        return -10;
    }

    return 0;
  }
  /// <summary>
  /// Проверяет первый ли это ход.
  /// </summary>
  /// <returns>Возвращает <see langword="true"/> если первый, <see langword="false"/> если нет.</returns>
  private bool IsFirstStep()
  {
    for (var i = 0; i < 9; i++)
    {
      if (board[(Cell)i] != Board.EmptySymbol)
        return false;
    }

    return true;
  }
  /// <summary>
  /// Ищет на игровой доске символ другого игрока.
  /// </summary>
  /// <returns>Возвращает найденный <see cref="char"/> символ другого игрока.</returns>
  private char FindAnotherSymbol()
  {
    for (var i = 0; i < 9; i++)
    {
      if (board[(Cell)i] == this.Symbol || board[(Cell)i] == Board.EmptySymbol) continue;
      return board[(Cell)i];
    }

    return (char)(this.Symbol + 1);
  }
  #endregion
}