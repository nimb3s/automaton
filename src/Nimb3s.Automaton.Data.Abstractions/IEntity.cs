using System;

namespace Nimb3s.Data.Abstractions
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
