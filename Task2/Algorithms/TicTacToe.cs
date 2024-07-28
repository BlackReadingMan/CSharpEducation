using System;
using System.Collections.Generic;
using Task2.CustomDataTypes;

namespace Task2.Algorithms;

internal sealed class TicTacToe
{
	#region [ Поля и свойства ]

	private readonly Board _board = new();
	private readonly Player _firstPlayer;
	private readonly Player _secondPlayer;
	private Player _firstMove;
	private readonly Dictionary<char, ConsoleColor> _palette;

	public Player CurrentPlayer { get; private set; }

	private readonly Dictionary<Player, int> _playersWinsCount;
	private int FirstPlayerWinsCount => _playersWinsCount[this._firstPlayer];
	private int SecondPlayerWinsCount => _playersWinsCount[this._secondPlayer];

	private int _drawsCount;

	#endregion

	#region [ Методы ]

	public void Start()
	{
		Console.WriteLine(this._secondPlayer is AiPlayer ? $"НУ ЗДРАВСТВУЙ КОЖАНЫЙ." : "Игра началсь.");
		Console.WriteLine(this.DrawBoard());

		while (true)
		{
			var moveResult = this.MakeMove(this.CurrentPlayer.GetPlayerMove());

			switch (moveResult)
			{
				case MoveResult.Win:
					Algorithms.ColorWrite(DrawBoard(), this._palette);
					Console.WriteLine(this.CurrentPlayer is AiPlayer
						? $"ПОБЕДИЛ РОБОТ {this.CurrentPlayer.Name}! ПЛОТЬ СЛАБА!"
						: $"ПОБЕДИЛ {this.CurrentPlayer.Name}");
					this.WriteStats();

					if (this.IsEnd())
						return;

					this.NextIteration();
					continue;
				case MoveResult.Draw:
					Algorithms.ColorWrite(DrawBoard(), this._palette);
					Console.WriteLine(this._secondPlayer is AiPlayer ? "НИЧЬЯ КОЖАНЫЙ." : "НИЧЬЯ");
					this.WriteStats();

					if (this.IsEnd())
						return;

					this.NextIteration();
					continue;
				case MoveResult.Success:
					Algorithms.ColorWrite(this.DrawBoard(), _palette);

					this.NextPlayer();
					continue;
				case MoveResult.NoSuccess:
					Console.WriteLine(this._secondPlayer is AiPlayer
						? $"ЭТА КЛЕТКА ЗАНЯТА КОЖАНЫЙ! СМОТРИ КУДА ХОДИШЬ!"
						: $"Эта клетка занята");

					continue;
			}
		}
	}

	//перезапуск игры
	public void ReStart()
	{
		this._firstMove = this._firstPlayer;
		this.CurrentPlayer = this._firstMove;
		this._playersWinsCount[this._firstPlayer] = 0;
		this._playersWinsCount[this._secondPlayer] = 0;
		this._drawsCount = 0;

		this._board.Clear();
	}

	//следующая итерация игры
	public void NextIteration()
	{
		this._firstMove = this._firstMove == this._firstPlayer ? this._secondPlayer : this._firstPlayer;
		this.CurrentPlayer = this._firstMove;

		this._board.Clear();

		Console.WriteLine(this.DrawBoard());
	}

	//смена игрока
	private void NextPlayer()
	{
		this.CurrentPlayer = this.CurrentPlayer == this._firstPlayer ? this._secondPlayer : this._firstPlayer;
	}

	//ход игрока
	public MoveResult MakeMove(in Cell position)
	{
		if (this._board[position] != Board.EmptySymbol)
			return MoveResult.NoSuccess;

		this._board[position] = this.CurrentPlayer.Symbol;

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

	private bool IsEnd()
	{
		Console.WriteLine(this._secondPlayer is AiPlayer ? "Продолжим кожаный?[y/n]" : "Желаете закончить партию?[y/n]");
		return Algorithms.YesOrNo();
	}

	private string DrawBoard()
	{
		return "-------------\n" +
			   $"| {WhatWrite(Cell.LeftTop)} | {WhatWrite(Cell.CenterTop)} | {WhatWrite(Cell.RightTop)} |\n" +
			   "-------------\n" +
			   $"| {WhatWrite(Cell.LeftCenter)} | {WhatWrite(Cell.Center)} | {WhatWrite(Cell.RightCenter)} |\n" +
			   "-------------\n" +
			   $"| {WhatWrite(Cell.LeftBottom)} | {WhatWrite(Cell.CenterBottom)} | {WhatWrite(Cell.RightBottom)} |\n" +
			   "-------------";
	}

	private void WriteStats()
	{
		Console.WriteLine($"{this._firstPlayer.Name}: {this.FirstPlayerWinsCount}");
		Console.WriteLine($"Ничьих: {this._drawsCount}");
		Console.WriteLine(this._secondPlayer is AiPlayer
			? $"Робот {this._secondPlayer.Name}: {this.SecondPlayerWinsCount}\n"
			: $"{this._secondPlayer.Name}: {this.SecondPlayerWinsCount}\n");
	}

	private char WhatWrite(Cell cell)
	{
		return this._board[cell] == ' ' ? (char)(cell + 49) : this._board[cell];
	}

	#endregion

	#region [ Конструкторы ]

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

		ReStart();
	}

	#endregion
}