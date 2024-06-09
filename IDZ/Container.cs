using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IDZ
{
    public class Container<T> : IEnumerable<T> where T : IName, IPrice
    {
        private List<T> items;
        private string containerName;

        public Container(string name)
        {
            items = new List<T>();
            containerName = name;
        }

        public void Add(T item)
        {
            try
            {
                if (item == null)
                    throw new ArgumentNullException(nameof(item), "Об'єкт не може бути null");

                T[] newArray = new T[items.Count + 1];
                for (int i = 0; i < items.Count; i++)
                {
                    newArray[i] = items[i];
                }
                newArray[newArray.Length - 1] = item;
                items = new List<T>(newArray);
            }
            catch (ArgumentNullException ex)
            {
             
                Console.WriteLine($"Помилка: {ex.Message}");
                throw; 
            }
        }

        public void RemoveAt(int index)
        {
            try
            {
                if (index < 0 || index >= items.Count)
                    throw new IndexOutOfRangeException("Недійсний індекс!");

                T[] newArray = new T[items.Count - 1];
                for (int i = 0, j = 0; i < items.Count; i++)
                {
                    if (i != index)
                    {
                        newArray[j++] = items[i];
                    }
                }
                items = new List<T>(newArray);
            }
            catch (IndexOutOfRangeException ex)
            {
              
                Console.WriteLine($"Помилка: {ex.Message}");
                throw; 
            }
        }

        public void Sort(Comparison<T> comparison)
        {
            try
            {

                for (int i = 0; i < items.Count - 1; i++)
                {
                    for (int j = 0; j < items.Count - i - 1; j++)
                    {
                        if (comparison(items[j], items[j + 1]) > 0)
                        {
                            T temp = items[j];
                            items[j] = items[j + 1];
                            items[j + 1] = temp;
                        }
                    }
                }
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Сортування невдале!", ex);
            }
        }

        public override string ToString()
        {
            try
            {
                return string.Join(Environment.NewLine, items);
            }
            catch (Exception ex)
            {
                throw new Exception("Не вдалося перетворити контейнер на рядок!", ex);
            }
        }

        public T this[int index]
        {
            get
            {
                try
                {
                    return items[index];
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw new IndexOutOfRangeException("Недійсний індекс!");
                }
            }
        }

        public T? GetByName(string name)
        {
            try
            {
                return items.FirstOrDefault(item => item.Name == name);
            }
            catch (Exception ex)
            {
                throw new Exception("Помилка отримання елемента за назвою", ex);
            }
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            try
            {
                return items.Where(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception("Помилка пошуку елементів", ex);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<T> GetItemsByPrefix(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
                throw new ArgumentException("Префікс не може бути нульовим або порожнім", nameof(prefix));

            return GetItemsByPrefixInternal(prefix);
        }

        private IEnumerable<T> GetItemsByPrefixInternal(string prefix)
        {
            foreach (var item in items)
            {
                if (item.Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    yield return item;
                }
            }
        }

        public void SaveToFile()
        {
            try
            {
                File.WriteAllText($"{containerName}.txt", ToString());
                Console.WriteLine("Вміст контейнера збережено до файлу Products.txt");
            }
            catch (IOException ex)
            {
                throw new IOException("Не вдалося зберегти контейнер у файл", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Неочікувана помилка під час збереження у файл", ex);
            }
        }

        public void Serialize(string filePath)
        {
            try
            {
                using (var fs = new FileStream(filePath, FileMode.Create))
                using (var writer = new BinaryWriter(fs))
                {
                    writer.Write(containerName);
                    writer.Write(items.Count);

                    foreach (var item in items)
                    {
                        if (item is Product product)
                        {
                            writer.Write(product.GetType().FullName ?? string.Empty);
                            writer.Write(product.Name);
                            writer.Write(product.Price);
                            writer.Write(product.Manufacturer);

                            if (product is Electronics electronics)
                            {
                                writer.Write(electronics.WarrantyPeriod);
                            }
                            else if (product is Clothing clothing)
                            {
                                writer.Write(clothing.Size);
                                writer.Write(clothing.Material);
                            }
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                throw new IOException("Не вдалося серіалізувати контейнер", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Неочікувана помилка під час серіалізації", ex);
            }
        }

        public void Deserialize(string filePath)
        {
            try
            {
                using (var fs = new FileStream(filePath, FileMode.Open))
                using (var reader = new BinaryReader(fs))
                {
                    containerName = reader.ReadString();
                    int count = reader.ReadInt32();
                    items = new List<T>(count);

                    for (int i = 0; i < count; i++)
                    {
                        string typeName = reader.ReadString();
                        string name = reader.ReadString();
                        decimal price = reader.ReadDecimal();
                        string manufacturer = reader.ReadString();

                        if (typeName == typeof(Electronics).FullName)
                        {
                            int warrantyPeriod = reader.ReadInt32();
                            items.Add((T)(IName)new Electronics(name, price, manufacturer, warrantyPeriod));
                        }
                        else if (typeName == typeof(Clothing).FullName)
                        {
                            string size = reader.ReadString();
                            string material = reader.ReadString();
                            items.Add((T)(IName)new Clothing(name, price, manufacturer, size, material));
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                throw new IOException("Не вдалося десеріалізувати контейнер", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Неочікувана помилка під час десеріалізації", ex);
            }
        }

        public decimal GetTotalValue()
        {
            return items.Sum(item => item.Price);
        }

        public T FindCheapestItem()
        {
            if (!items.Any())
                throw new InvalidOperationException("Контейнер порожній");

            return items.OrderBy(item => item.Price).First();
        }

        public T FindMostExpensiveItem()
        {
            if (!items.Any())
                throw new InvalidOperationException("Контейнер порожній");

            return items.OrderByDescending(item => item.Price).First();
        }

        public Dictionary<string, decimal> GetAveragePriceByCategory()
        {
            return items
                .GroupBy(item => item.GetType().Name)
                .ToDictionary(g => g.Key, g => g.Average(item => item.Price));
        }
    }
}
