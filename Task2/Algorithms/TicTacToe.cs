using Task2.CustomDataTypes;

namespace Task2.Algorithms;

internal class TicTacToe
{
	private Dictionary<Cell, char> _board = [];
	private int _move;
	private Player _firstMove = Player.Player1;
	private Player _currentPlayer;
	private bool _isStarted;

	private readonly Dictionary<Player, char> _playersSymbols = new()
	{
		{ Player.Player1, 'X' },
		{ Player.Player2, 'O' }
	};
	private readonly Dictionary<Player, int> _playersWins = new()
	{
		{ Player.Player1, 0 },
		{ Player.Player2, 0 }
	};
	public TicTacToe()
	{

	}

	public TicTacToe( char firstPlayerSymbol, char secondPlayerSymbol)
	{
		FirstPlayerSymbol = firstPlayerSymbol;
		SecondPlayerSymbol = secondPlayerSymbol;
	}

	public char FirstPlayerSymbol
	{
		get => _playersSymbols[Player.Player1];
		set => _playersSymbols[Player.Player1] = value;
	}

	public char SecondPlayerSymbol
	{
		get => _playersSymbols[Player.Player2];
		set => _playersSymbols[Player.Player2] = value;
	}

	public int FirstPlayerWinsCount => _playersWins[Player.Player1];
	public int SecondPlayerWinsCount => _playersWins[Player.Player2];
	public int DrawsCount { get; private set; }
	public string Start()
	{
		_playersWins[Player.Player1] = 0;
		_playersWins[Player.Player2] = 0;
		DrawsCount = 0;
		_board = new Dictionary<Cell, char>
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
		_move = 0;
		_currentPlayer = _firstMove;
		_isStarted = true;
		return DrawBoard() + "\nХод игрока 1:";
	}

	private void NextIteration()
	{
		_board = new Dictionary<Cell, char>
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
		_firstMove = _firstMove == Player.Player1 ? Player.Player2 : Player.Player1;
		_currentPlayer = _firstMove;
		_move = 0;
	}


	public bool MakeMove(in Cell position, out string status)
	{
		if (!_isStarted)
		{
			status = "Не начата игра.";
			return false;
		}

		if (position == Cell.Wrong)
		{
			status = "Нужно вводить число от 1 до 9. Введите корректный номер клетки:";
			return false;
		}

		if (_board[position] == 'X' || _board[position] == 'O')
		{
			status = "Это клетка уже занята. Введите корректный номер клетки:";
			return false;
		}

		_board[position] = _playersSymbols[_currentPlayer];

		if (CheckForWin())
		{
			_playersWins[_currentPlayer]++;
			status = DrawBoard() +
			         (_currentPlayer == Player.Player1 ? "Победил игрок 1\n\n" : "Победил игрок 2\n\n");
			NextIteration();
			status += DrawBoard() + (_currentPlayer == Player.Player1 ? "\nХод игрока 1:" : "\nХод игрока 2:");
		}
		else
		{
			if (CheckForDraw())
			{
				DrawsCount++;
				status = DrawBoard() + "Ничья\n\n";
				NextIteration();
				status += DrawBoard() + (_currentPlayer == Player.Player1 ? "\nХод игрока 1:" : "\nХод игрока 2:");
			}
			else
			{
				status = DrawBoard();
				NextPlayer();
				status += _currentPlayer == Player.Player1 ? "\nХод игрока 1:" : "\nХод игрока 2:";
			}
		}

		return true;
	}

	// Выводим текущее состояние игрового поля
	private string DrawBoard()
	{
		return "-------------\n" +
		       $"| {_board[Cell.LeftTop]} | {_board[Cell.CenterTop]} | {_board[Cell.RightTop]} |\n" +
		       "-------------\n" +
		       $"| {_board[Cell.LeftCenter]} | {_board[Cell.Center]} | {_board[Cell.RightCenter]} |\n" +
		       "-------------\n" +
		       $"| {_board[Cell.LeftBottom]} | {_board[Cell.CenterBottom]} | {_board[Cell.RightBottom]} |\n" +
		       "-------------\n";
	}

	private void NextPlayer()
	{
		_currentPlayer = _currentPlayer == Player.Player1 ? Player.Player2 : Player.Player1;
		_move++;
	}

	// Проверяем на выигрыш
	public string ComputerMove()
	{
		if (!_isStarted)
		{
			return "Не начата игра.";
		}
		var bestVal = int.MinValue;
		var bestPosition = Cell.Wrong;

		for (var i = 0; i < 9; i++)
		{
			if (_board[(Cell)i] == _playersSymbols[Player.Player1] ||
			    _board[(Cell)i] == _playersSymbols[Player.Player2]) continue;
			_board[(Cell)i] = _playersSymbols[_currentPlayer];
			var moveVal = Minimax(0, false);
			_board[(Cell)i] = (char)(i + 49);

			if (moveVal <= bestVal) continue;
			bestPosition = (Cell)i;
			bestVal = moveVal;
		}

		MakeMove(bestPosition, out var output);
		return output;
	}

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

		if (!IsMovesLeft()) return 0;

		if (isMax)
		{
			var best = int.MinValue;
			for (var i = 0; i < 9; i++)
			{
				if (_board[(Cell)i] == _playersSymbols[Player.Player1] ||
				    _board[(Cell)i] == _playersSymbols[Player.Player2]) continue;
				_board[(Cell)i] = _playersSymbols[_currentPlayer];
				best = Math.Max(best, Minimax(depth + 1, !isMax));
				_board[(Cell)i] = (char)(i + 49);
			}

			return best;
		}
		else
		{
			var best = int.MaxValue;
			for (var i = 0; i < 9; i++)
			{
				if (_board[(Cell)i] == _playersSymbols[Player.Player1] ||
				    _board[(Cell)i] == _playersSymbols[Player.Player2]) continue;
				_board[(Cell)i] =
					_playersSymbols[_currentPlayer == Player.Player1 ? Player.Player2 : Player.Player1];
				best = Math.Min(best, Minimax(depth + 1, !isMax));
				_board[(Cell)i] = (char)(i + 49);
			}

			return best;
		}
	}

	private int Evaluate()
	{
		if (_board[Cell.LeftTop] == _board[Cell.CenterTop] && _board[Cell.CenterTop] == _board[Cell.RightTop])
		{
			if (_board[Cell.LeftTop] == _playersSymbols[_currentPlayer])
				return +10;
			if (_board[Cell.LeftTop] ==
			    _playersSymbols[_currentPlayer == Player.Player1 ? Player.Player2 : Player.Player1])
				return -10;
		}

		if (_board[Cell.LeftCenter] == _board[Cell.Center] && _board[Cell.Center] == _board[Cell.RightCenter])
		{
			if (_board[Cell.LeftCenter] == _playersSymbols[_currentPlayer])
				return +10;
			if (_board[Cell.LeftCenter] ==
			    _playersSymbols[_currentPlayer == Player.Player1 ? Player.Player2 : Player.Player1])
				return -10;
		}

		if (_board[Cell.LeftBottom] == _board[Cell.CenterBottom] &&
		    _board[Cell.CenterBottom] == _board[Cell.RightBottom])
		{
			if (_board[Cell.LeftBottom] == _playersSymbols[_currentPlayer])
				return +10;
			if (_board[Cell.LeftBottom] ==
			    _playersSymbols[_currentPlayer == Player.Player1 ? Player.Player2 : Player.Player1])
				return -10;
		}

		if (_board[Cell.LeftTop] == _board[Cell.LeftCenter] && _board[Cell.LeftCenter] == _board[Cell.LeftBottom])
		{
			if (_board[Cell.LeftTop] == _playersSymbols[_currentPlayer])
				return +10;
			if (_board[Cell.LeftTop] ==
			    _playersSymbols[_currentPlayer == Player.Player1 ? Player.Player2 : Player.Player1])
				return -10;
		}

		if (_board[Cell.CenterTop] == _board[Cell.Center] && _board[Cell.Center] == _board[Cell.CenterBottom])
		{
			if (_board[Cell.CenterTop] == _playersSymbols[_currentPlayer])
				return +10;
			if (_board[Cell.CenterTop] ==
			    _playersSymbols[_currentPlayer == Player.Player1 ? Player.Player2 : Player.Player1])
				return -10;
		}

		if (_board[Cell.RightTop] == _board[Cell.RightCenter] &&
		    _board[Cell.RightCenter] == _board[Cell.RightBottom])
		{
			if (_board[Cell.RightTop] == _playersSymbols[_currentPlayer])
				return +10;
			if (_board[Cell.RightTop] ==
			    _playersSymbols[_currentPlayer == Player.Player1 ? Player.Player2 : Player.Player1])
				return -10;
		}

		if (_board[Cell.LeftTop] == _board[Cell.Center] && _board[Cell.Center] == _board[Cell.RightBottom])
		{
			if (_board[Cell.LeftTop] == _playersSymbols[_currentPlayer])
				return +10;
			if (_board[Cell.LeftTop] ==
			    _playersSymbols[_currentPlayer == Player.Player1 ? Player.Player2 : Player.Player1])
				return -10;
		}

		if (_board[Cell.RightTop] == _board[Cell.Center] && _board[Cell.Center] == _board[Cell.LeftBottom])
		{
			if (_board[Cell.RightTop] == _playersSymbols[_currentPlayer])
				return +10;
			if (_board[Cell.RightTop] ==
			    _playersSymbols[_currentPlayer == Player.Player1 ? Player.Player2 : Player.Player1])
				return -10;
		}

		return 0;
	}

	private bool IsMovesLeft()
	{
		return _board.Any(cell =>
			cell.Value != _playersSymbols[Player.Player1] && cell.Value != _playersSymbols[Player.Player2]);
	}

	private bool CheckForWin()
	{
		return (_board[Cell.LeftTop] == _board[Cell.CenterTop] &&
		        _board[Cell.CenterTop] == _board[Cell.RightTop]) ||
		       (_board[Cell.LeftCenter] == _board[Cell.Center] &&
		        _board[Cell.Center] == _board[Cell.RightCenter]) ||
		       (_board[Cell.LeftBottom] == _board[Cell.CenterBottom] &&
		        _board[Cell.CenterBottom] == _board[Cell.RightBottom]) ||
		       (_board[Cell.LeftTop] == _board[Cell.LeftCenter] &&
		        _board[Cell.LeftCenter] == _board[Cell.LeftBottom]) ||
		       (_board[Cell.CenterTop] == _board[Cell.Center] &&
		        _board[Cell.Center] == _board[Cell.CenterBottom]) ||
		       (_board[Cell.RightTop] == _board[Cell.RightCenter] &&
		        _board[Cell.RightCenter] == _board[Cell.RightBottom]) ||
		       (_board[Cell.LeftTop] == _board[Cell.Center] && _board[Cell.Center] == _board[Cell.RightBottom]) ||
		       (_board[Cell.RightTop] == _board[Cell.Center] && _board[Cell.Center] == _board[Cell.LeftBottom]);
	}

	// Проверяем на ничью
	private bool CheckForDraw()
	{
		return _move == 8;
	}
}