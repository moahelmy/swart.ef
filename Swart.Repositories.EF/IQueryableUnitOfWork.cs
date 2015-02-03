using System.Data.Entity;
using Swart.DomainDrivenDesign.Repositories;

namespace Swart.Repositories.EntityFramework
{
    public interface IQueryableUnitOfWork : IUnitOfWork
    {        
        IDbSet<TEntity> CreateSet<TEntity>() where TEntity : class;

        void SetModified<TEntity>(TEntity entity) where TEntity : class;
    }
}
