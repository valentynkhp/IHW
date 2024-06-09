using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDZ
{
    public interface IName: IComparable<IName>
    {
        string Name { get; }
    }

    public interface IPrice
    {
        decimal Price { get; }
    }
}
