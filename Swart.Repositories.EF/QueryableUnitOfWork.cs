using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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

        public virtual ITransaction BeginTransaction()
        {
            return new DbTransaction(Database.BeginTransaction());
        }

        public new virtual void SaveChanges()
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

        public virtual IDbSet<TEntity> CreateSet<TEntity>() where TEntity : class
        {
            return Set<TEntity>();
        }

        public void SetModified<TEntity>(TEntity entity)
            where TEntity : class
        {
            //this operation also attach entity in object state manager
            Entry(entity).State = EntityState.Modified;
        }

        #endregion
    }
}
