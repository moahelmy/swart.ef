using System;
using System.Data.Entity;
using System.Linq;
using Swart.DomainDrivenDesign.Domain;
using Swart.DomainDrivenDesign.Repositories;

namespace Swart.Repositories.EntityFramework
{
    public class Repository<TEntity, TKey> : RepositoryBase<TEntity, TKey>
        where TEntity : class, IEntity<TKey> where TKey : IComparable
    {
        protected readonly IQueryableUnitOfWork QueryableUnitOfWork;

        public Repository(IQueryableUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("unitOfWork");

            QueryableUnitOfWork = unitOfWork;
        }

        #region Basic
        public override IUnitOfWork UnitOfWork { get { return QueryableUnitOfWork; } }

        public override IQueryable<TEntity> List()
        {
            return GetSet();
        }

        public override TEntity Get(TKey id)
        {
            return GetSet().Find(id);
        }
        #endregion        

        #region List
        public override void Add(TEntity entity)
        {
            if (entity != null)
                GetSet().Add(entity); // add new entity in this set
            else
                throw new ArgumentNullException();
        }

        public override void Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException();

            //attach entity if not exist
            GetSet().Attach(entity);

            //set as "removed"
            GetSet().Remove(entity);
        }

        public override void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException();

            //attach entity if not exist and mark it as modified
            QueryableUnitOfWork.SetModified(entity);
        }
        #endregion

        #region IDisposable
        public override void Dispose()
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
