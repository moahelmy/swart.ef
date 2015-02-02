using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Swart.DomainDrivenDesign.Repositories;
using Swart.DomainDrivenDesign.Repositories.Exceptions;

namespace Swart.Repositories.EntityFramework
{
    public class QueryableUnitOfWork : DbContext, IQueryableUnitOfWork
    {
        #region Contructor(s)
        public QueryableUnitOfWork() { }

        public QueryableUnitOfWork(string connectionString)
            : base(connectionString)
        {
        }
        #endregion

        #region IQueryableUnitOfWork

        public ITransaction BeginTransaction()
        {
            return new DbTransaction(Database.BeginTransaction());
        }

        public void Commit()
        {
            try
            {
                base.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (ex.Message.Contains("Store update, insert, or delete statement affected an unexpected number of rows (0)"))
                    throw new RecordNotFoundException("Check inner exception for more details",ex);
            }
        }

        public void RollbackChanges()
        {
            // set all entities in change tracker 
            // as 'unchanged state'
            ChangeTracker.Entries()
                        .ToList()
                        .ForEach(entry => entry.State = EntityState.Unchanged);
        }

        public IDbSet<TEntity> CreateSet<TEntity>() where TEntity : class
        {
            return Set<TEntity>();
        }

        public void Attach<TEntity>(TEntity item) where TEntity : class
        {
            //attach and set as unchanged
            Entry(item).State = EntityState.Unchanged;
        }

        #endregion
    }
}
