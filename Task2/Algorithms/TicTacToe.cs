using System;
using System.Collections.Generic;
using Task2.CustomDataTypes;
using Task2.Players;

namespace Task2.Algorithms;
/// <summary>
/// Класс игры крестики-нолики.
/// </summary>
internal sealed class TicTacToe
{
  #region Поля и свойства
  /// <summary>
  /// Игровая доска.
  /// </summary>
  private readonly Board _board = new();
  /// <summary>
  /// Первый игрок.
  /// </summary>
  private readonly Player _firstPlayer;
  /// <summary>
  /// Второй игрок.
  /// </summary>
  private readonly Player _secondPlayer;
  /// <summary>
  /// Игрок ходивший первый в этой итерации.
  /// </summary>
  private Player _firstMove;
  /// <summary>
  /// Палитра для вывода символов игроков.
  /// </summary>
  private readonly Dictionary<char, ConsoleColor> _palette;
  /// <summary>
  /// Игрок, который ходит сейчас.
  /// </summary>
  public Player CurrentPlayer { get; private set; }
  /// <summary>
  /// Количество побед игроков.
  /// </summary>
  private readonly Dictionary<Player, int> _playersWinsCount;
  private int FirstPlayerWinsCount => this._playersWinsCount[this._firstPlayer];
  private int SecondPlayerWinsCount => this._playersWinsCount[this._secondPlayer];

  /// <summary>
  /// Количество ничьих.
  /// </summary>
  private int _drawsCount;

  #endregion

  #region Методы
  /// <summary>
  /// Начинает и руководит игровым процессом.
  /// </summary>
  public void Start()
  {
    Console.WriteLine(this._secondPlayer is AiPlayer ? $"НУ ЗДРАВСТВУЙ КОЖАНЫЙ." : "Игра началась.");
    Console.WriteLine(this.DrawBoard());

    while (true)
    {
      var moveResult = this.MakeMove(this.CurrentPlayer.GetPlayerMove());

      switch (moveResult)
      {
        case MoveResult.Win:
          this.ProcessTheVictory();

          if (this.IsEnd())
            return;
          this.NextRound();
          continue;
        case MoveResult.Draw:
          this.ProcessTheDraw();

          if (this.IsEnd())
            return;
          this.NextRound();
          continue;
        case MoveResult.Success:
          Helper.ColorWrite(this.DrawBoard(), this._palette);

          this.NextPlayer();
          continue;
        case MoveResult.Failure:
          Console.WriteLine(this._secondPlayer is AiPlayer
              ? $"ЭТА КЛЕТКА ЗАНЯТА КОЖАНЫЙ! СМОТРИ КУДА ХОДИШЬ!"
              : $"Эта клетка занята");

          continue;
      }
    }
  }
  /// <summary>
  /// Вывод в консоль информацию о победе.
  /// </summary>
  private void ProcessTheVictory()
  {
    Helper.ColorWrite(this.DrawBoard(), this._palette);
    Console.WriteLine(this.CurrentPlayer is AiPlayer
      ? $"ПОБЕДИЛ РОБОТ {this.CurrentPlayer.Name}! ПЛОТЬ СЛАБА!"
      : $"ПОБЕДИЛ {this.CurrentPlayer.Name}");
    this.WriteStats();
  }
  /// <summary>
  /// Вывод в консоль информацию о ничьей.
  /// </summary>
  private void ProcessTheDraw()
  {
    Helper.ColorWrite(this.DrawBoard(), this._palette);
    Console.WriteLine(this._secondPlayer is AiPlayer ? "НИЧЬЯ КОЖАНЫЙ." : "НИЧЬЯ");
    this.WriteStats();
  }
  /// <summary>
  /// Приводит игру к начальному состоянию.
  /// </summary>
  public void ReStart()
  {
    this._firstMove = this._firstPlayer;
    this.CurrentPlayer = this._firstMove;
    this._playersWinsCount[this._firstPlayer] = 0;
    this._playersWinsCount[this._secondPlayer] = 0;
    this._drawsCount = 0;

    this._board.PrepareBoard();
  }

  /// <summary>
  /// Подготавлиет игру к следующему раунду.
  /// </summary>
  public void NextRound()
  {
    this._firstMove = this._firstMove == this._firstPlayer ? this._secondPlayer : this._firstPlayer;
    this.CurrentPlayer = this._firstMove;

    this._board.PrepareBoard();

    Console.WriteLine(this.DrawBoard());
  }

  /// <summary>
  /// Меняет ходящего игрока на следующего.
  /// </summary>
  private void NextPlayer()
  {
    this.CurrentPlayer = this.CurrentPlayer == this._firstPlayer ? this._secondPlayer : this._firstPlayer;
  }

  /// <summary>
  /// Делает ход на игровой доске.
  /// </summary>
  /// <param name="position">Клетка в которую сходил ходяйщий игрок.</param>
  /// <returns>Возвращает <see cref="MoveResult"/>.</returns>
  public MoveResult MakeMove(in Cell position)
  {
    if (this._board[position] is not null)
      return MoveResult.Failure;

    this._board[position] = this.CurrentPlayer;

    if (this._board.IsWin())
    {
      this._playersWinsCount[this.CurrentPlayer]++;
      return MoveResult.Win;
    }

    if (this._board.IsDraw())
    {
      this._drawsCount++;
      return MoveResult.Draw;
    }

    return MoveResult.Success;
  }
  /// <summary>
  /// Спрашивает у игрока закончить ли партию.
  /// </summary>
  /// <returns>Возвращает <see langword="true"/> если да, либо <see langword="false"/> если нет.</returns>
  private bool IsEnd()
  {
    Console.WriteLine(this._secondPlayer is AiPlayer ? "Продолжим кожаный?[y/n]" : "Желаете закончить партию?[y/n]");
    return Helper.YesOrNo();
  }
  /// <summary>
  /// Формирует строку с состоянием игровой доски.
  /// </summary>
  private string DrawBoard()
  {
    return "-------------\n" +
           $"| {this.WhatWrite(Cell.LeftTop)} | {this.WhatWrite(Cell.CenterTop)} | {this.WhatWrite(Cell.RightTop)} |\n" +
           "-------------\n" +
           $"| {this.WhatWrite(Cell.LeftCenter)} | {this.WhatWrite(Cell.Center)} | {this.WhatWrite(Cell.RightCenter)} |\n" +
           "-------------\n" +
           $"| {this.WhatWrite(Cell.LeftBottom)} | {this.WhatWrite(Cell.CenterBottom)} | {this.WhatWrite(Cell.RightBottom)} |\n" +
           "-------------";
  }
  /// <summary>
  /// Выводит статистику о победах и ничьих в консоль.
  /// </summary>
  private void WriteStats()
  {
    Console.WriteLine($"{this._firstPlayer.Name}: {this.FirstPlayerWinsCount}");
    Console.WriteLine($"Ничьих: {this._drawsCount}");
    Console.WriteLine(this._secondPlayer is AiPlayer
        ? $"Робот {this._secondPlayer.Name}: {this.SecondPlayerWinsCount}\n"
        : $"{this._secondPlayer.Name}: {this.SecondPlayerWinsCount}\n");
  }
  /// <summary>
  /// Проверяет пустая ли эта клетка.
  /// </summary>
  /// <param name="cell">Номер клетки.</param>
  /// <returns><see cref="char"/> с порядковым номером клетки если она пустая или с символом игрока если нет.</returns>
  private char WhatWrite(Cell cell)
  {
    return this._board[cell] is null ? (char)(cell + 49) : this._board[cell].Symbol;
  }

  #endregion

  #region Конструкторы

  public TicTacToe(GameMode gameMode, string firstPlayerName, char firstPlayerSymbol, string secondPlayerName,
      char secondPlayerSymbol)
  {
    this._firstPlayer = new RealPayer(firstPlayerName, firstPlayerSymbol);

    if (gameMode is GameMode.PVP)
      this._secondPlayer = new AiPlayer(secondPlayerName, secondPlayerSymbol, this._board);
    else
      this._secondPlayer = new RealPayer(secondPlayerName, secondPlayerSymbol);

    this._palette = new Dictionary<char, ConsoleColor>
        {
            { this._firstPlayer.Symbol, ConsoleColor.Red },
            { this._secondPlayer.Symbol, ConsoleColor.Blue }
        };
    this._playersWinsCount = new Dictionary<Player, int>
        {
            { this._firstPlayer, 0 },
            { this._secondPlayer, 0 }
        };

    this.ReStart();
  }

  #endregion
}