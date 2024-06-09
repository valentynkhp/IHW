using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace IDZ
{
    internal class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCP(uint codePage);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleOutputCP(uint codePage);

        static void Main(string[] args)
        {
            SetConsoleCP(1251);
            SetConsoleOutputCP(1251);

            Container<Product> productContainer = new Container<Product>("Products");

            while (true)
            {
                Console.WriteLine("Виберіть дію:");
                Console.WriteLine("1. Додати об'єкт до контейнера");
                Console.WriteLine("2. Видалити об'єкт за індексом");
                Console.WriteLine("3. Упорядкувати контейнер");
                Console.WriteLine("4. Переглянути контейнер");
                Console.WriteLine("5. Отримати об'єкт за індексом");
                Console.WriteLine("6. Отримати об'єкт за ім'ям");
                Console.WriteLine("7. Знайти об'єкти за префіксом");
                Console.WriteLine("8. Зберегти контейнер у файл");
                Console.WriteLine("9. Серіалізувати контейнер");
                Console.WriteLine("10. Десеріалізувати контейнер");
                Console.WriteLine("11. Сортування за параметром");
                Console.WriteLine("12. Пошук за лямбдою");
                Console.WriteLine("13. Отримати сумарну ціну");
                Console.WriteLine("14. Найдорожчий товар");
                Console.WriteLine("15. Найдешевший товар");
                Console.WriteLine("16. Середня вартість для кожної категорії");
                Console.WriteLine("17. Вийти");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddItem(productContainer);
                        break;
                    case "2":
                        RemoveItem(productContainer);
                        break;
                    case "3":
                        productContainer.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.Ordinal));
                        Console.WriteLine("Контейнер упорядкований!");
                        break;
                    case "4":
                        Console.WriteLine(productContainer.ToString());
                        break;
                    case "5":
                        GetItemByIndex(productContainer);
                        break;
                    case "6":
                        GetItemByName(productContainer);
                        break;
                    case "7":
                        GetItemsByPrefix(productContainer);
                        break;
                    case "8":
                        productContainer.SaveToFile();
                        break;
                    case "9":
                        SerializeContainer(productContainer);
                        break;
                    case "10":
                        DeserializeContainer(productContainer);
                        break;
                    case "11":
                        SortItems(productContainer); 
                        break;
                    case "12":
                        FindItemsByCondition(productContainer); 
                        break;
                    case "13":
                        GetTotalValue(productContainer); 
                        break;
                    case "14":
                        FindMostExpensiveItem(productContainer); 
                        break;
                    case "15":
                        FindCheapestProduct(productContainer);
                        break;
                    case "16":
                        GetAveragePriceByCategory(productContainer); 
                        break;
                    case "17":
                        return;
                    default:
                        Console.WriteLine("Невірний вибір, спробуйте ще раз.");
                        break;
                }
            }
        }

        static void AddItem(Container<Product> container)
        {
            Console.WriteLine("Виберіть тип продукту: 1 - Електроніка, 2 - Одяг");
            string? type = Console.ReadLine();

            Console.Write("Введіть назву: ");
            string? name = Console.ReadLine();

            Console.Write("Введіть ціну: ");
            string? priceInput = Console.ReadLine();
            if (!decimal.TryParse(priceInput, out decimal price))
            {
                Console.WriteLine("Невірний формат ціни.");
                return;
            }

            switch (type)
            {
                case "1":
                    Console.Write("Введіть виробника: ");
                    string? manufacturer = Console.ReadLine();

                    Console.Write("Введіть гарантійний термін: ");
                    string? warrantyPeriodInput = Console.ReadLine();
                    if (!int.TryParse(warrantyPeriodInput, out int warrantyPeriod))
                    {
                        Console.WriteLine("Невірний формат гарантійного терміну.");
                        return;
                    }

                    container.Add(new Electronics(name ?? "", price, manufacturer ?? "", warrantyPeriod));
                    break;

                case "2":
                    Console.Write("Введіть розмір: ");
                    string? size = Console.ReadLine();

                    Console.Write("Введіть матеріал: ");
                    string? material = Console.ReadLine();

                    container.Add(new Clothing(name ?? "", price, size ?? "", material ?? ""));
                    break;

                default:
                    Console.WriteLine("Невірний вибір типу продукту.");
                    break;
            }
        }

        static void RemoveItem(Container<Product> container)
        {
            Console.Write("Введіть індекс для видалення: ");
            string? indexInput = Console.ReadLine();
            if (!int.TryParse(indexInput, out int index))
            {
                Console.WriteLine("Невірний формат індексу.");
                return;
            }
            container.RemoveAt(index);
            Console.WriteLine("Об'єкт видалено.");
        }

        static void SortItems(Container<Product> container)
        {
            Console.WriteLine("Виберіть спосіб сортування:");
            Console.WriteLine("1. За ціною (зростання)");
            Console.WriteLine("2. За ціною (спадання)");
            Console.WriteLine("3. За назвою (зростання)");
            Console.WriteLine("4. За назвою (спадання)");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    container.Sort((x, y) => x.Price.CompareTo(y.Price));
                    break;
                case "2":
                    container.Sort((x, y) => y.Price.CompareTo(x.Price));
                    break;
                case "3":
                    container.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.Ordinal));
                    break;
                case "4":
                    container.Sort((x, y) => string.Compare(y.Name, x.Name, StringComparison.Ordinal));
                    break;
                default:
                    Console.WriteLine("Невірний вибір, сортування не виконано.");
                    return;
            }

            Console.WriteLine("Контейнер упорядковано.");
        }
        static void GetItemByIndex(Container<Product> container)
        {
            Console.Write("Введіть індекс: ");
            string? indexInput = Console.ReadLine();
            if (!int.TryParse(indexInput, out int index))
            {
                Console.WriteLine("Невірний формат індексу.");
                return;
            }
            var item = container[index];
            Console.WriteLine(item);
        }

        static void GetItemByName(Container<Product> container)
        {
            Console.Write("Введіть ім'я: ");
            string? name = Console.ReadLine();
            var item = container.GetByName(name ?? "");
            if (item != null)
            {
                Console.WriteLine(item);
            }
            else
            {
                Console.WriteLine("Об'єкт не знайдено.");
            }
        }

        static void GetItemsByPrefix(Container<Product> container)
        {
            Console.Write("Введіть префікс: ");
            string? prefix = Console.ReadLine();
            if (string.IsNullOrEmpty(prefix))
            {
                Console.WriteLine("Префікс не може бути порожнім.");
                return;
            }
            var items = container.GetItemsByPrefix(prefix);
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
        }

        static void SerializeContainer(Container<Product> container)
        {
            Console.Write("Введіть шлях для збереження файлу: ");
            string? filePath = Console.ReadLine();
            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("Шлях не може бути порожнім.");
                return;
            }
            container.Serialize(filePath);
            Console.WriteLine("Контейнер серіалізовано.");
        }

        static void DeserializeContainer(Container<Product> container)
        {
            Console.Write("Введіть шлях до файлу: ");
            string? filePath = Console.ReadLine();
            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("Шлях не може бути порожнім.");
                return;
            }
            container.Deserialize(filePath);
            Console.WriteLine("Контейнер десеріалізовано.");
        }

        static void FindItemsByCondition(Container<Product> container)
        {
            Console.WriteLine("Виберіть умову для пошуку:");
            Console.WriteLine("1. Ціна більше заданого значення");
            Console.WriteLine("2. Назва містить певний рядок");

            string? choice = Console.ReadLine();

            IEnumerable<Product> foundItems = Enumerable.Empty<Product>();

            switch (choice)
            {
                case "1":
                    Console.Write("Введіть значення ціни: ");
                    string? priceInput = Console.ReadLine();
                    if (decimal.TryParse(priceInput, out decimal price))
                    {
                        foundItems = container.Find(item => item.Price > price);
                    }
                    else
                    {
                        Console.WriteLine("Невірний формат ціни.");
                    }
                    break;
                case "2":
                    Console.Write("Введіть рядок для пошуку в назві: ");
                    string? substring = Console.ReadLine();
                    if (!string.IsNullOrEmpty(substring))
                    {
                        foundItems = container.Find(item => item.Name.Contains(substring, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        Console.WriteLine("Рядок не може бути порожнім.");
                    }
                    break;
                default:
                    Console.WriteLine("Невірний вибір.");
                    break;
            }

            foreach (var item in foundItems)
            {
                Console.WriteLine(item);
            }
        }

        static void GetTotalValue(Container<Product> container)
        {
            decimal totalValue = container.GetTotalValue();
            Console.WriteLine($"Загальна вартість товарів на складі: {totalValue:C}");
        }


        static void FindMostExpensiveItem(Container<Product> container)
        {
            try
            {
                var mostExpensiveItem = container.FindMostExpensiveItem();
                Console.WriteLine($"Найдорожчий товар: {mostExpensiveItem.Name}, Ціна: {mostExpensiveItem.Price:C}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static void FindCheapestProduct(Container<Product> container)
        {
            try
            {
                var cheapestProduct = container.FindCheapestItem();
                Console.WriteLine("Найдешевший продукт: " + cheapestProduct);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void GetAveragePriceByCategory(Container<Product> container)
        {
            var averagePrices = container.GetAveragePriceByCategory();
            foreach (var category in averagePrices)
            {
                Console.WriteLine($"Категорія: {category.Key}, Середня ціна: {category.Value:C}");
            }
        }

    }
}
