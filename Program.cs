using System;
using System.Collections.Generic;
using System.Linq;

namespace Project
{
    public class Drink
    {
        public string Name { get; }
        public int Price { get; }

        public Drink(string name, int price)
        {
            Name = name;
            Price = price;
        }
    }

    class Program
    {
        static void Main()
        {
            List<Drink> drinks = new List<Drink>
            {
                new Drink("Латте", 200),
                new Drink("Капучино", 150),
                new Drink("Экспрессо", 100)
            };

            Dictionary<int, int> cashStorage = new Dictionary<int, int>
            {
                { 10, 5 },
                { 50, 5 },
                { 100, 5 },
                { 200, 5 },
                { 500, 5 }
            };

            Console.WriteLine("-------------------------");
            Console.WriteLine("  Добро пожаловать!");
            Console.WriteLine("-------------------------");

            while (true)
            {
                Console.WriteLine("\nМеню напитков:\n");
                for (int i = 0; i < drinks.Count; i++)
                    Console.WriteLine($"{i + 1}. {drinks[i].Name,-10} - {drinks[i].Price} тг");

                Console.Write("\nВыберите напиток (1-3): ");
                string input = Console.ReadLine() ?? "";

                if (!int.TryParse(input.Trim(), out int choice) || choice < 1 || choice > drinks.Count)
                {
                    Console.WriteLine("Ошибка: неверный выбор напитка!");
                    continue;
                }

                Drink selectedDrink = drinks[choice - 1];
                Console.WriteLine($"\nВы выбрали: {selectedDrink.Name} — {selectedDrink.Price} тг");

                Console.Write("Введите сумму оплаты: ");
                string moneyInput = Console.ReadLine() ?? "";
                if (!int.TryParse(moneyInput.Trim(), out int money) || money <= 0)
                {
                    Console.WriteLine("Ошибка: неверная сумма!");
                    continue;
                }

                if (money < selectedDrink.Price)
                {
                    Console.WriteLine("Недостаточно средств. Попробуйте снова.");
                    continue;
                }

                int change = money - selectedDrink.Price;

                if (change == 0)
                {
                    Console.WriteLine("\nСпасибо за покупку! Сдачи нет.");
                }
                else
                {
                    var result = GetChange(change, cashStorage);
                    if (result == null)
                    {
                        Console.WriteLine("\nНевозможно выдать сдачу доступными купюрами!");
                    }
                    else
                    {
                        Console.WriteLine($"\nВаша сдача: {change} тг");
                        Console.WriteLine("Выдано:");
                        foreach (var kvp in result)
                        {
                            Console.WriteLine($"  {kvp.Key} тг × {kvp.Value}");
                            cashStorage[kvp.Key] -= kvp.Value;
                        }
                    }
                    Console.WriteLine("\nСпасибо за покупку!");
                }

                // хочет ли пользователь купить ещё
                Console.Write("\nХотите купить ещё? (да/нет): ");
                string again = (Console.ReadLine() ?? "").Trim().ToLower();

                if (again == "нет" || again == "net")
                {
                    Console.WriteLine("\nСпасибо, что воспользовались автоматом!");
                    return; // Завершение программы
                }
                else if (again == "да" || again == "da")
                {
                
                    continue;
                }
                else
                {
                    Console.WriteLine("Неверный ввод. Автомат завершает работу.");
                    return;
                }
            }
        }

        static Dictionary<int, int>? GetChange(int change, Dictionary<int, int> storage)
        {
            var available = storage.OrderByDescending(x => x.Key);
            Dictionary<int, int> result = new Dictionary<int, int>();
            int remaining = change;

            foreach (var item in available)
            {
                int denom = item.Key;
                int availableCount = item.Value;

                int needed = remaining / denom;
                int used = Math.Min(needed, availableCount);

                if (used > 0)
                {
                    result[denom] = used;
                    remaining -= used * denom;
                }

                if (remaining == 0) break;
            }

            if (remaining > 0) return null;

            return result;
        }
    }
}