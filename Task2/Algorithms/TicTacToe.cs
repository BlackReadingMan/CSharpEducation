using Task2.CustomDataTypes;

namespace Task2.Algorithms;

internal class TicTacToe
{
	#region [ Поля и свойства ]

	public Dictionary<Cell, char> Board { get; } = new()
	{
		{ Cell.LeftTop, '1' },
		{ Cell.CenterTop, '2' },
		{ Cell.RightTop, '3' },
		{ Cell.LeftCenter, '4' },
		{ Cell.Center, '5' },
		{ Cell.RightCenter, '6' },
		{ Cell.LeftBottom, '7' },
		{ Cell.CenterBottom, '8' },
		{ Cell.RightBottom, '9' }
	};

	private Player _firstMove = Player.Player1;

	public Player CurrentPlayer { get; private set; }

	private readonly Dictionary<Player, char> _playersSymbols = new()
	{
		{ Player.Player1, 'X' },
		{ Player.Player2_Or_AI, 'O' }
	};

	public char FirstPlayerSymbol
	{
		get => _playersSymbols[Player.Player1];
		set => _playersSymbols[Player.Player1] = value;
	}

	public char SecondPlayerSymbol
	{
		get => _playersSymbols[Player.Player2_Or_AI];
		set => _playersSymbols[Player.Player2_Or_AI] = value;
	}

	private readonly Dictionary<Player, int> _playersWinsCount = new()
	{
		{ Player.Player1, 0 },
		{ Player.Player2_Or_AI, 0 }
	};

	public int FirstPlayerWinsCount => _playersWinsCount[Player.Player1];

	public int SecondPlayerWinsCount => _playersWinsCount[Player.Player2_Or_AI];

	public int DrawsCount { get; private set; }

	#endregion

	#region [ Методы ]

	//перезапуск игры
	public void Start()
	{
		_playersWinsCount[Player.Player1] = 0;
		_playersWinsCount[Player.Player2_Or_AI] = 0;
		DrawsCount = 0;
		Board[Cell.LeftTop] = '1';
		Board[Cell.CenterTop] = '2';
		Board[Cell.RightTop] = '3';
		Board[Cell.LeftCenter] = '4';
		Board[Cell.Center] = '5';
		Board[Cell.RightCenter] = '6';
		Board[Cell.LeftBottom] = '7';
		Board[Cell.CenterBottom] = '8';
		Board[Cell.RightBottom] = '9';
		CurrentPlayer = _firstMove;
	}

	//следующая итерация игры
	public void NextIteration()
	{
		Board[Cell.LeftTop] = '1';
		Board[Cell.CenterTop] = '2';
		Board[Cell.RightTop] = '3';
		Board[Cell.LeftCenter] = '4';
		Board[Cell.Center] = '5';
		Board[Cell.RightCenter] = '6';
		Board[Cell.LeftBottom] = '7';
		Board[Cell.CenterBottom] = '8';
		Board[Cell.RightBottom] = '9';
		_firstMove = _firstMove == Player.Player1 ? Player.Player2_Or_AI : Player.Player1;
		CurrentPlayer = _firstMove;
	}

	//смена игрока
	private void NextPlayer()
	{
		CurrentPlayer = CurrentPlayer == Player.Player1 ? Player.Player2_Or_AI : Player.Player1;
	}

	//ход игрока
	public MoveResult MakeMove(in Cell position)
	{
		Board[position] = _playersSymbols[CurrentPlayer];
		if (IsWin())
		{
			_playersWinsCount[CurrentPlayer]++;
			return MoveResult.Win;
		}

		if (!IsDraw())
		{
			DrawsCount++;
			return MoveResult.Draw;
		}

		NextPlayer();
		return MoveResult.Success;
	}

	#region [ Проверки состония игры ]

	//проверка на ничью
	private bool IsDraw()
	{
		return Board.Any(cell =>
			cell.Value != _playersSymbols[Player.Player1] && cell.Value != _playersSymbols[Player.Player2_Or_AI]);
	}

	//проверка на победу
	private bool IsWin()
	{
		return (Board[Cell.LeftTop] == Board[Cell.CenterTop] &&
		        Board[Cell.CenterTop] == Board[Cell.RightTop]) ||
		       (Board[Cell.LeftCenter] == Board[Cell.Center] &&
		        Board[Cell.Center] == Board[Cell.RightCenter]) ||
		       (Board[Cell.LeftBottom] == Board[Cell.CenterBottom] &&
		        Board[Cell.CenterBottom] == Board[Cell.RightBottom]) ||
		       (Board[Cell.LeftTop] == Board[Cell.LeftCenter] &&
		        Board[Cell.LeftCenter] == Board[Cell.LeftBottom]) ||
		       (Board[Cell.CenterTop] == Board[Cell.Center] &&
		        Board[Cell.Center] == Board[Cell.CenterBottom]) ||
		       (Board[Cell.RightTop] == Board[Cell.RightCenter] &&
		        Board[Cell.RightCenter] == Board[Cell.RightBottom]) ||
		       (Board[Cell.LeftTop] == Board[Cell.Center] && Board[Cell.Center] == Board[Cell.RightBottom]) ||
		       (Board[Cell.RightTop] == Board[Cell.Center] && Board[Cell.Center] == Board[Cell.LeftBottom]);
	}

	#endregion

	#region [ Логика робота ]

	//ход бота
	public MoveResult ComputerMove()
	{
		var bestVal = int.MinValue;
		var bestPosition = Cell.Wrong;

		for (var i = 0; i < 9; i++)
		{
			if (Board[(Cell)i] == _playersSymbols[Player.Player1] ||
			    Board[(Cell)i] == _playersSymbols[Player.Player2_Or_AI]) continue;
			Board[(Cell)i] = _playersSymbols[CurrentPlayer];
			var moveVal = Minimax(0, false);
			Board[(Cell)i] = (char)(i + 49);

			if (moveVal <= bestVal) continue;
			bestPosition = (Cell)i;
			bestVal = moveVal;
		}

		return MakeMove(bestPosition);
	}

	//алгоритм МИНИМАКС для робота
	private int Minimax(int depth, bool isMax)
	{
		var score = Evaluate();

		switch (score)
		{
			case 10:
				return score - depth;
			case -10:
				return score + depth;
		}

		if (!IsDraw()) return 0;

		if (isMax)
		{
			var best = int.MinValue;
			for (var i = 0; i < 9; i++)
			{
				if (Board[(Cell)i] == _playersSymbols[Player.Player1] ||
				    Board[(Cell)i] == _playersSymbols[Player.Player2_Or_AI]) continue;
				Board[(Cell)i] = _playersSymbols[CurrentPlayer];
				best = Math.Max(best, Minimax(depth + 1, !isMax));
				Board[(Cell)i] = (char)(i + 49);
			}

			return best;
		}
		else
		{
			var best = int.MaxValue;
			for (var i = 0; i < 9; i++)
			{
				if (Board[(Cell)i] == _playersSymbols[Player.Player1] ||
				    Board[(Cell)i] == _playersSymbols[Player.Player2_Or_AI]) continue;
				Board[(Cell)i] =
					_playersSymbols[CurrentPlayer == Player.Player1 ? Player.Player2_Or_AI : Player.Player1];
				best = Math.Min(best, Minimax(depth + 1, !isMax));
				Board[(Cell)i] = (char)(i + 49);
			}

			return best;
		}
	}

	//полезность хода
	private int Evaluate()
	{
		if (Board[Cell.LeftTop] == Board[Cell.CenterTop] && Board[Cell.CenterTop] == Board[Cell.RightTop])
		{
			if (Board[Cell.LeftTop] == _playersSymbols[CurrentPlayer])
				return +10;
			if (Board[Cell.LeftTop] ==
			    _playersSymbols[CurrentPlayer == Player.Player1 ? Player.Player2_Or_AI : Player.Player1])
				return -10;
		}

		if (Board[Cell.LeftCenter] == Board[Cell.Center] && Board[Cell.Center] == Board[Cell.RightCenter])
		{
			if (Board[Cell.LeftCenter] == _playersSymbols[CurrentPlayer])
				return +10;
			if (Board[Cell.LeftCenter] ==
			    _playersSymbols[CurrentPlayer == Player.Player1 ? Player.Player2_Or_AI : Player.Player1])
				return -10;
		}

		if (Board[Cell.LeftBottom] == Board[Cell.CenterBottom] &&
		    Board[Cell.CenterBottom] == Board[Cell.RightBottom])
		{
			if (Board[Cell.LeftBottom] == _playersSymbols[CurrentPlayer])
				return +10;
			if (Board[Cell.LeftBottom] ==
			    _playersSymbols[CurrentPlayer == Player.Player1 ? Player.Player2_Or_AI : Player.Player1])
				return -10;
		}

		if (Board[Cell.LeftTop] == Board[Cell.LeftCenter] && Board[Cell.LeftCenter] == Board[Cell.LeftBottom])
		{
			if (Board[Cell.LeftTop] == _playersSymbols[CurrentPlayer])
				return +10;
			if (Board[Cell.LeftTop] ==
			    _playersSymbols[CurrentPlayer == Player.Player1 ? Player.Player2_Or_AI : Player.Player1])
				return -10;
		}

		if (Board[Cell.CenterTop] == Board[Cell.Center] && Board[Cell.Center] == Board[Cell.CenterBottom])
		{
			if (Board[Cell.CenterTop] == _playersSymbols[CurrentPlayer])
				return +10;
			if (Board[Cell.CenterTop] ==
			    _playersSymbols[CurrentPlayer == Player.Player1 ? Player.Player2_Or_AI : Player.Player1])
				return -10;
		}

		if (Board[Cell.RightTop] == Board[Cell.RightCenter] &&
		    Board[Cell.RightCenter] == Board[Cell.RightBottom])
		{
			if (Board[Cell.RightTop] == _playersSymbols[CurrentPlayer])
				return +10;
			if (Board[Cell.RightTop] ==
			    _playersSymbols[CurrentPlayer == Player.Player1 ? Player.Player2_Or_AI : Player.Player1])
				return -10;
		}

		if (Board[Cell.LeftTop] == Board[Cell.Center] && Board[Cell.Center] == Board[Cell.RightBottom])
		{
			if (Board[Cell.LeftTop] == _playersSymbols[CurrentPlayer])
				return +10;
			if (Board[Cell.LeftTop] ==
			    _playersSymbols[CurrentPlayer == Player.Player1 ? Player.Player2_Or_AI : Player.Player1])
				return -10;
		}

		if (Board[Cell.RightTop] == Board[Cell.Center] && Board[Cell.Center] == Board[Cell.LeftBottom])
		{
			if (Board[Cell.RightTop] == _playersSymbols[CurrentPlayer])
				return +10;
			if (Board[Cell.RightTop] ==
			    _playersSymbols[CurrentPlayer == Player.Player1 ? Player.Player2_Or_AI : Player.Player1])
				return -10;
		}

		return 0;
	}

	#endregion

	#endregion

	#region [ Конструкторы ]

	public TicTacToe()
	{

	}

	public TicTacToe(Player firstMove, char firstPlayerSymbol, char secondPlayerSymbol)
	{
		_firstMove = firstMove;
		FirstPlayerSymbol = firstPlayerSymbol;
		SecondPlayerSymbol = secondPlayerSymbol;

		Start();
	}

	#endregion
}