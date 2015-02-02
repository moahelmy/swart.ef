using System;
using System.Data.Entity;
using System.Linq;
using Swart.DomainDrivenDesign.Domain;
using Swart.DomainDrivenDesign.Domain.Specification;
using Swart.DomainDrivenDesign.Repositories;
using Swart.DomainDrivenDesign.Repositories.Exceptions;

namespace Swart.Repositories.EntityFramework
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : AbstractEntity, IEntity<TKey> where TKey : IComparable
    {
        protected readonly IQueryableUnitOfWork QueryableUnitOfWork;

        public Repository(IQueryableUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("unitOfWork");

            QueryableUnitOfWork = unitOfWork;
        }

        #region Basic
        public IUnitOfWork UnitOfWork { get { return QueryableUnitOfWork; } }

        public IQueryable<TEntity> GetAll()
        {
            return GetSet();
        }

        public virtual TEntity Get(TKey id)
        {
            return GetSet().Find(id);
        }
        #endregion        

        #region List
        public virtual void Add(TEntity item)
        {
            if (item != null)
                GetSet().Add(item); // add new item in this set
            else
                throw new ArgumentNullException();
        }

        public virtual void Remove(TEntity item)
        {
            if (item == null)
                throw new ArgumentNullException();

            //attach item if not exist
            QueryableUnitOfWork.Attach(item);

            //set as "removed"
            GetSet().Remove(item);
        }

        public virtual TEntity Remove(TKey id)
        {
            var item = Get(id);
            if(item == null)
                throw new RecordNotFoundException();
            Remove(item);
            return item;
        }
        #endregion               

        #region Queryable
        public IQueryable<TEntity> Filter(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter)
        {
            return GetSet().Where(filter);
        }

        public IQueryable<TEntity> Filter(ISpecification<TEntity> specification)
        {
            return Filter(specification.SatisfiedBy());
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            QueryableUnitOfWork.Dispose();
        }
        #endregion

        #region Private Methods

        IDbSet<TEntity> GetSet()
        {
            return QueryableUnitOfWork.CreateSet<TEntity>();
        }
        #endregion        
    }
}
