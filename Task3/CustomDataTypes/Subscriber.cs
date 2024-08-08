namespace Task3.CustomDataTypes;
/// <summary>
/// Класс абонента.
/// </summary>
internal class Subscriber(string name, PhoneNumber number)
{
  /// <summary>
  /// Имя абонента.
  /// </summary>
  public string Name { get; } = name;
  /// <summary>
  /// Номер абонента.
  /// </summary>
  public PhoneNumber Number { get; } = number;
}