using System.Collections.Generic;
using System.Linq;
using Task2.CustomDataTypes;

namespace Task2.Algorithms;

internal sealed class Board
{
	public const char EmptySymbol = ' ';

	private readonly Dictionary<Cell, char> _playingField = new()
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

	public char this[in Cell cell]
	{
		get => this._playingField[cell];
		set => this._playingField[cell] = value;
	}

	public void Clear()
	{
		this._playingField[Cell.LeftTop] = EmptySymbol;
		this._playingField[Cell.CenterTop] = EmptySymbol;
		this._playingField[Cell.RightTop] = EmptySymbol;
		this._playingField[Cell.LeftCenter] = EmptySymbol;
		this._playingField[Cell.Center] = EmptySymbol;
		this._playingField[Cell.RightCenter] = EmptySymbol;
		this._playingField[Cell.LeftBottom] = EmptySymbol;
		this._playingField[Cell.CenterBottom] = EmptySymbol;
		this._playingField[Cell.RightBottom] = EmptySymbol;
	}

	public bool IsDraw()
	{
		return this._playingField.All(cell => cell.Value != EmptySymbol);
	}

	public bool IsWin()
	{
		return this._playingField[Cell.LeftTop] == this._playingField[Cell.CenterTop] &&
		       this._playingField[Cell.CenterTop] == this._playingField[Cell.RightTop] &&
		       this._playingField[Cell.LeftTop] != EmptySymbol ||
		       this._playingField[Cell.LeftCenter] == this._playingField[Cell.Center] &&
		       this._playingField[Cell.Center] == this._playingField[Cell.RightCenter] &&
		       this._playingField[Cell.LeftCenter] != EmptySymbol ||
		       this._playingField[Cell.LeftBottom] == this._playingField[Cell.CenterBottom] &&
		       this._playingField[Cell.CenterBottom] == this._playingField[Cell.RightBottom] &&
		       this._playingField[Cell.LeftBottom] != EmptySymbol ||
		       this._playingField[Cell.LeftTop] == this._playingField[Cell.LeftCenter] &&
		       this._playingField[Cell.LeftCenter] == this._playingField[Cell.LeftBottom] &&
		       this._playingField[Cell.LeftTop] != EmptySymbol ||
		       this._playingField[Cell.CenterTop] == this._playingField[Cell.Center] &&
		       this._playingField[Cell.Center] == this._playingField[Cell.CenterBottom] &&
		       this._playingField[Cell.CenterTop] != EmptySymbol ||
		       this._playingField[Cell.RightTop] == this._playingField[Cell.RightCenter] &&
		       this._playingField[Cell.RightCenter] == this._playingField[Cell.RightBottom] &&
		       this._playingField[Cell.RightTop] != EmptySymbol ||
		       this._playingField[Cell.LeftTop] == this._playingField[Cell.Center] &&
		       this._playingField[Cell.Center] == this._playingField[Cell.RightBottom] &&
		       this._playingField[Cell.LeftTop] != EmptySymbol ||
		       this._playingField[Cell.RightTop] == this._playingField[Cell.Center] &&
		       this._playingField[Cell.Center] == this._playingField[Cell.LeftBottom] &&
		       this._playingField[Cell.RightTop] != EmptySymbol;
	}
}