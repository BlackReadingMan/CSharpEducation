using System;
using System.Linq;

namespace Task3.CustomDataTypes;

/// <summary>
/// Структера для номера телефона.
/// </summary>
internal struct PhoneNumber
{
  #region Поля и свойства

  /// <summary>
  /// Номер телефона.
  /// </summary>
  public int[] Number { get; }

  #endregion

  #region Методы

  /// <summary>
  /// Возвращает номер телефона в виде строки.
  /// </summary>
  /// <returns>Номер телефона в формате: +X(XXX)XXX-XX-XX.</returns>
  public string GetString()
  {
    var phoneNumber = string.Empty;
    phoneNumber += '+';
    phoneNumber += (char)(this.Number[0] + 48);
    phoneNumber += '(';
    phoneNumber += (char)(this.Number[1] + 48);
    phoneNumber += (char)(this.Number[2] + 48);
    phoneNumber += (char)(this.Number[3] + 48);
    phoneNumber += ')';
    phoneNumber += (char)(this.Number[4] + 48);
    phoneNumber += (char)(this.Number[5] + 48);
    phoneNumber += (char)(this.Number[6] + 48);
    phoneNumber += '-';
    phoneNumber += (char)(this.Number[7] + 48);
    phoneNumber += (char)(this.Number[8] + 48);
    phoneNumber += '-';
    phoneNumber += (char)(this.Number[9] + 48);
    phoneNumber += (char)(this.Number[10] + 48);
    return phoneNumber;
  }

  /// <summary>
  /// Получает номер телефона из консоли.
  /// </summary>
  public static PhoneNumber GetPhoneByConsole()
  {
    var phoneNumber = new int[11];
    Console.WriteLine("Введите номер телефона абонента:");
    while (true)
    {
      Console.Write('+');
      phoneNumber[0] = Console.ReadKey().KeyChar - 48;
      Console.Write('(');
      phoneNumber[1] = Console.ReadKey().KeyChar - 48;
      phoneNumber[2] = Console.ReadKey().KeyChar - 48;
      phoneNumber[3] = Console.ReadKey().KeyChar - 48;
      Console.Write(')');
      phoneNumber[4] = Console.ReadKey().KeyChar - 48;
      phoneNumber[5] = Console.ReadKey().KeyChar - 48;
      phoneNumber[6] = Console.ReadKey().KeyChar - 48;
      Console.Write('-');
      phoneNumber[7] = Console.ReadKey().KeyChar - 48;
      phoneNumber[8] = Console.ReadKey().KeyChar - 48;
      Console.Write('-');
      phoneNumber[9] = Console.ReadKey().KeyChar - 48;
      phoneNumber[10] = Console.ReadKey().KeyChar - 48;
      Console.WriteLine();
      if (NumberCheck(phoneNumber))
        break;
      Console.WriteLine("Ошибка при вводе. Введите ещё раз:");
    }

    return new PhoneNumber(phoneNumber);

  }

  public static bool operator !=(PhoneNumber left, PhoneNumber right)
  {
    return !left.Number.SequenceEqual(right.Number);
  }
  public static bool operator ==(PhoneNumber left, PhoneNumber right)
  {
    return left.Number.SequenceEqual(right.Number);
  }

  /// <summary>
  /// Проверяет корректность введённого номера.
  /// </summary>
  /// <returns>Если корректный возвращает <see langword="true"/>, если нет <see langword="false"/>.</returns>
  private static bool NumberCheck(int[] number)
  {
    return number.All(i => i is >= 0 and <= 9);
  }

  #endregion

  #region Конструкторы

  public PhoneNumber(string number)
  {
    this.Number = new int[11];
    this.Number[0] = number[1] - 48;
    this.Number[1] = number[3] - 48;
    this.Number[2] = number[4] - 48;
    this.Number[3] = number[5] - 48;
    this.Number[4] = number[7] - 48;
    this.Number[5] = number[8] - 48;
    this.Number[6] = number[9] - 48;
    this.Number[7] = number[11] - 48;
    this.Number[8] = number[12] - 48;
    this.Number[9] = number[14] - 48;
    this.Number[10] = number[15] - 48;
  }
  private PhoneNumber(int[] number)
  {
    this.Number = number;
  }

  #endregion
}