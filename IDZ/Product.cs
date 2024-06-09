namespace IDZ
{
    public class Product : IName, IPrice
    {
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public string Manufacturer { get; private set; }

        public Product()
        {
            Name = "";
            Price = 0.0m;
            Manufacturer = "";
        }

        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
            Manufacturer = "";
        }

        public Product(string name, decimal price, string manufacturer)
        {
            Name = name;
            Price = price;
            Manufacturer = manufacturer;
        }

        public int CompareTo(IName? other)
        {
            if (other == null) return 1;
            return this.Name.CompareTo(other.Name);
        }

        public override string ToString()
        {
            return $"Продукт: {Name}, Ціна: {Price:C}, Виробник: {Manufacturer}";
        }
    }

    public class Electronics : Product
    {
        public int WarrantyPeriod { get; private set; }

        public Electronics() : base()
        {
            WarrantyPeriod = 0;
        }

        public Electronics(string name, decimal price, int warrantyPeriod)
            : base(name, price)
        {
            WarrantyPeriod = warrantyPeriod;
        }

        public Electronics(string name, decimal price, string manufacturer, int warrantyPeriod)
            : base(name, price, manufacturer)
        {
            WarrantyPeriod = warrantyPeriod;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Гарантійний термін: {WarrantyPeriod} місяців";
        }
    }

    public class Clothing : Product
    {
        public string Size { get; private set; }
        public string Material { get; private set; }

        public Clothing() : base()
        {
            Size = "";  
            Material = "";
        }

        public Clothing(string name, decimal price, string size, string material)
            : base(name, price)
        {
            Size = size;
            Material = material;
        }

        public Clothing(string name, decimal price, string manufacturer, string size, string material)
            : base(name, price, manufacturer)
        {
            Size = size;
            Material = material;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Розмір: {Size}, Матеріал: {Material}";
        }
    }
}
