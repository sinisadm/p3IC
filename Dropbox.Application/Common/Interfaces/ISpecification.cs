using System;
using System.Linq.Expressions;

namespace Dropbox.Application.Common.Interfaces
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Predicate { get; }
    }
}
