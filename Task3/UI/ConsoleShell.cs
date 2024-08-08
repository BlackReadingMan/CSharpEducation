using System;
using System.Threading.Tasks;
using Task3.Algorithms;
using Task3.CustomDataTypes;

namespace Task3.UI;
/// <summary>
/// Класс для работы с пользователем.
/// </summary>
internal class ConsoleShell
{
  /// <summary>
  /// Ссылка на текущую телефонную книгу.
  /// </summary>
  private PhoneBook _phoneBook;

  /// <summary>
  /// Начинает работу пользователя с тел. книгой.
  /// </summary>
  public async Task StartAsync(PhoneBook phoneBook)
  {
    this._phoneBook = phoneBook;
    while (true)
    {
      var action = GetUserAction();
      if (action == UserAction.EndSession)
      {
        break;
      }
      switch (action)
      {
        case UserAction.Add:
          await this.AddSubscriberAsync();
          continue;
        case UserAction.RemoveByNumber:
          await this.RemoveSubscriberByNumberAsync();
          continue;
        case UserAction.RemoveByName:
          await this.RemoveSubscriberByNameAsync();
          continue;
        case UserAction.GetByNumber:
          this.GetSubscriberByNumber();
          continue;
        case UserAction.GetByName:
          this.GetSubscriberByName();
          continue;
      }
    }
  }
  /// <summary>
  /// Узнает у пользователя, какое действие он хочет совершить.
  /// </summary>
  private static UserAction GetUserAction()
  {
    Console.WriteLine("Выберите действие:");
    Console.WriteLine("[1]-добавить абонента");
    Console.WriteLine("[2]-удалить абонента по номеру");
    Console.WriteLine("[3]-удалить абонента по имени");
    Console.WriteLine("[4]-получить имя по номеру абонента");
    Console.WriteLine("[5]-получить номер по имени абонента");
    Console.WriteLine("[6]-выход");
    UserAction result;
    while (true)
    {
      result = KeyToAction(Console.ReadKey().Key);
      Console.WriteLine();
      if (result == UserAction.Wrong)
      {
        Console.WriteLine("Неправильный выбор. Введите ещё раз:");
      }
      else
      {
        break;
      }
    }
    return result;
  }
  /// <summary>
  /// Преобразует нажатую клавишу пользователя в <see cref="UserAction"/>.
  /// </summary>
  /// <param name="key">Нажатая пользователем клавиша.</param>
  private static UserAction KeyToAction(ConsoleKey key)
  {
    return key switch
    {
      ConsoleKey.D1 => UserAction.Add,
      ConsoleKey.D2 => UserAction.RemoveByNumber,
      ConsoleKey.D3 => UserAction.RemoveByName,
      ConsoleKey.D4 => UserAction.GetByNumber,
      ConsoleKey.D5 => UserAction.GetByName,
      ConsoleKey.D6 => UserAction.EndSession,
      ConsoleKey.NumPad1 => UserAction.Add,
      ConsoleKey.NumPad2 => UserAction.RemoveByNumber,
      ConsoleKey.NumPad3 => UserAction.RemoveByName,
      ConsoleKey.NumPad4 => UserAction.GetByNumber,
      ConsoleKey.NumPad5 => UserAction.GetByName,
      ConsoleKey.NumPad6 => UserAction.EndSession,
      _ => UserAction.Wrong
    };
  }

  /// <summary>
  /// Получает от пользователя данные абоента и пытается занести его в тел. книгу.
  /// </summary>
  private async Task AddSubscriberAsync()
  {
    var name = GetName();
    var number = PhoneNumber.GetPhoneByConsole();
    var result = await this._phoneBook.AddSubscriberAsync(name, number);
    switch (result)
    {
      case ActionResult.FailureInName:
        Console.WriteLine("Абонент с таким именем уже существует.");
        break;
      case ActionResult.FailureInNumber:
        Console.WriteLine("Абонент с таким номером уже существует.");
        break;
      case ActionResult.Success:
        Console.WriteLine("Абонент успешно записан.");
        break;
    }
  }
  /// <summary>
  /// Получает от пользователя номер абоента и пытается удалить его из тел. книги.
  /// </summary>
  private async Task RemoveSubscriberByNumberAsync()
  {
    var number = PhoneNumber.GetPhoneByConsole();
    var result = await this._phoneBook.RemoveSubscriberAsync(number);
    Console.WriteLine(result == ActionResult.Success
      ? "Абонент успешно удалён."
      : "Абонент с таким номером не найден.");
  }
  /// <summary>
  /// Получает от пользователя имя абоента и пытается удалить его из тел. книги.
  /// </summary>
  private async Task RemoveSubscriberByNameAsync()
  {
    var name = GetName();
    var result = await this._phoneBook.RemoveSubscriberAsync(name);
    Console.WriteLine(result == ActionResult.Success
      ? "Абонент успешно удалён."
      : "Абонент с таким именем не найден.");
  }
  /// <summary>
  /// Получает от пользователя номер абоента и пытается найти его в тел. книге.
  /// </summary>
  private void GetSubscriberByNumber()
  {
    var number = PhoneNumber.GetPhoneByConsole();
    var sub = this._phoneBook.GetSubscriber(number);
    Console.WriteLine(sub is null
      ? "Абонент с таким номером не найден."
      : $"Абонент: Имя - {sub.Name}, номер - {sub.Number.GetString()}");
  }
  /// <summary>
  /// Получает от пользователя имя абоента и пытается найти его в тел. книге.
  /// </summary>
  private void GetSubscriberByName()
  {
    var name = GetName();
    var sub = this._phoneBook.GetSubscriber(name);
    Console.WriteLine(sub is null
      ? "Абонент с таким именем не найден."
      : $"Абонент: Имя - {sub.Name}, номер - {sub.Number.GetString()}");
  }
  /// <summary>
  /// Получает имя абонента из консоли.
  /// </summary>
  private static string GetName()
  {
    string name;
    Console.WriteLine("Введите имя абонента:");

    while (true)
    {
      name = Console.ReadLine();
      if (name is null)
      {
        Console.WriteLine("Вы не ввели имя абонента. Введите ещё раз:");
      }
      else
      {
        break;
      }
    }
    return name;
  }
}