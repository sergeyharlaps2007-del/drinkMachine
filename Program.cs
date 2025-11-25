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
                // Показываем меню
                Console.WriteLine("\nМеню напитков:\n");
                for (int i = 0; i < drinks.Count; i++)
                    Console.WriteLine($"{i + 1}. {drinks[i].Name,-10} - {drinks[i].Price} тг");

                Console.Write("\nВыберите напиток (1-3): ");
                string input = (Console.ReadLine() ?? "").Trim();

                if (!int.TryParse(input, out int choice) || choice < 1 || choice > drinks.Count)
                {
                    Console.WriteLine("Ошибка: неверный выбор напитка!");
                    continue;
                }

                Drink selectedDrink = drinks[choice - 1];
                Console.WriteLine($"\nВы выбрали: {selectedDrink.Name} — {selectedDrink.Price} тг");

                // Ввод денег
                Console.Write("Введите сумму оплаты: ");
                string moneyInput = (Console.ReadLine() ?? "").Trim();
                if (!int.TryParse(moneyInput, out int money) || money <= 0)
                {
                    Console.WriteLine("Ошибка: неверная сумма!");
                    continue;
                }

                if (money < selectedDrink.Price)
                {
                    Console.WriteLine("Недостаточно средств. Попробуйте снова.");
                    continue;
                }

                int changeAmount = money - selectedDrink.Price;

                if (changeAmount == 0)
                {
                    Console.WriteLine("\nСпасибо за покупку! Сдачи нет.");
                }
                else
                {
                    var change = GetChangeBalanced(changeAmount, cashStorage);

                    if (change == null)
                    {
                        Console.WriteLine("\nНевозможно выдать сдачу доступными купюрами!");
                    }
                    else
                    {
                        Console.WriteLine($"\nВаша сдача: {changeAmount} тг");
                        Console.WriteLine("Выдано:");

                        // Сортируем по возрастанию для аккуратного вывода
                        foreach (var kvp in change.OrderBy(x => x.Key))
                        {
                            Console.WriteLine($"  {kvp.Key} тг × {kvp.Value}");
                        }
                    }
                }

                // Спросить, хочет ли пользователь купить ещё
                Console.Write("\nХотите купить ещё? (da/net): ");
                string again = (Console.ReadLine() ?? "").Trim().ToLower();

                if (again == "нет" || again == "net")
                {
                    Console.WriteLine("\nСпасибо, что воспользовались автоматом!");
                    return;
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

        // Метод выдачи равномерной сдачи
        static Dictionary<int, int>? GetChangeBalanced(int change, Dictionary<int, int> storage)
        {
            Dictionary<int, int> result = new Dictionary<int, int>();
            int remaining = change;

            // Перемешиваем номиналы случайно для равномерного распределения
            var denominations = storage.Keys.OrderBy(x => Guid.NewGuid()).ToList();

            // Локальная копия хранилища, чтобы не уменьшать реальные купюры пока не выдаем
            Dictionary<int, int> tempStorage = new Dictionary<int, int>(storage);

            while (remaining > 0)
            {
                bool gave = false;

                foreach (var denom in denominations)
                {
                    if (tempStorage[denom] > 0 && remaining >= denom)
                    {
                        result.TryGetValue(denom, out int count);
                        result[denom] = count + 1;
                        tempStorage[denom]--;
                        remaining -= denom;
                        gave = true;
                    }
                }

                if (!gave)
                {
                    // Если на этом шаге не смогли выдать ни одну купюру — сдача невозможна
                    return null;
                }
            }

            // Сдача успешно рассчитана, уменьшаем реальные купюры
            foreach (var kvp in result)
            {
                storage[kvp.Key] -= kvp.Value;
            }

            return result;
        }
    }
}