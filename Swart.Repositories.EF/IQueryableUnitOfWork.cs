using System.Data.Entity;
using Swart.DomainDrivenDesign.Repositories;

namespace Swart.Repositories.EntityFramework
{
    public interface IQueryableUnitOfWork : IUnitOfWork
    {
        
        IDbSet<TEntity> CreateSet<TEntity>() where TEntity : class;
        
        void Attach<TEntity>(TEntity item) where TEntity : class;
        
        void ApplyCurrentValues<TEntity>(TEntity original, TEntity current) where TEntity : class;
    }
}
