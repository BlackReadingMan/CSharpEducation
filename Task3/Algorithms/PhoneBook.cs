using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Task3.CustomDataTypes;

namespace Task3.Algorithms;
/// <summary>
/// Класс телефонной книги.
/// </summary>
internal class PhoneBook
{
  #region Поля и свойства

  /// <summary>
  /// Коллекция с абонентами.
  /// </summary>
  private readonly List<Subscriber> _subscribers;

  /// <summary>
  /// Синглтон момент.
  /// </summary>
  public static PhoneBook Instance { get; }

  #endregion

  #region Методы

  /// <summary>
  /// Добавяет абонента в коллекцию и файл.
  /// </summary>
  public async Task<ActionResult> AddSubscriberAsync(string name, PhoneNumber number)
  {
    if (this._subscribers.Any(x => x.Number == number)) return ActionResult.FailureInNumber;
    if (this._subscribers.Any(x => x.Name == name)) return ActionResult.FailureInName;
    this._subscribers.Add(new Subscriber(name, number));
    await this.UpdateFileAsync();
    return ActionResult.Success;
  }
  /// <summary>
  /// Удаляет абонента по номеру телефона из коллекции и файла.
  /// </summary>
  public async Task<ActionResult> RemoveSubscriberAsync(PhoneNumber number)
  {
    if (!this._subscribers.Remove(this._subscribers.First(x => x.Number == number))) return ActionResult.RemoveFailure;
    await this.UpdateFileAsync();
    return ActionResult.Success;
  }
  /// <summary>
  /// Удаляет абонента по имени телефона из коллекции и файла.
  /// </summary>
  public async Task<ActionResult> RemoveSubscriberAsync(string name)
  {
    if (!this._subscribers.Remove(this._subscribers.First(x => x.Name == name))) return ActionResult.RemoveFailure;
    await this.UpdateFileAsync();
    return ActionResult.Success;
  }
  /// <summary>
  /// Получает абонента по номеру телефона.
  /// </summary>
  public Subscriber? GetSubscriber(PhoneNumber number)
  {
    return this._subscribers.FirstOrDefault(sub => sub.Number == number);
  }
  /// <summary>
  /// Получает абонента по имени.
  /// </summary>
  public Subscriber? GetSubscriber(string name)
  {
    return this._subscribers.FirstOrDefault(sub => sub.Name == name);
  }
  /// <summary>
  /// Загружает сохранённых в файле абонентов в коллекцию.
  /// </summary>
  private void LoadSubscribersFromFile()
  {
    using var sr = new StreamReader(new FileStream("phoneBook.txt", FileMode.OpenOrCreate));
    while (sr.Peek() > 0)
    {
      var s = sr.ReadLine().Split(',');
      this._subscribers.Add(new Subscriber(s[0], new PhoneNumber(s[1])));
    }
  }
  /// <summary>
  /// Загружает сохранённых в коллекции абонентов в файл.
  /// </summary>
  private async Task UpdateFileAsync()
  {
    await using var sr = new StreamWriter(new FileStream("phoneBook.txt", FileMode.Create));
    foreach (var sub in this._subscribers)
    {
      await sr.WriteLineAsync($"{sub.Name},{sub.Number.GetString()}");
    }
  }

  #endregion

  #region Конструкторы

  private PhoneBook()
  {
    this._subscribers = [];
    this.LoadSubscribersFromFile();
  }
  static PhoneBook()
  {
    Instance = new PhoneBook();
  }
  ~PhoneBook()
  {
    _ = this.UpdateFileAsync();
  }

  #endregion
}
