namespace Task2.CustomDataTypes;

internal abstract class Player(string name, char symbol)
{
	public string Name { get; set; } = name;
	public char Symbol { get; set; } = symbol;

	public abstract Cell GetPlayerMove();
}