﻿using System;
using System.Data.Entity;
using System.Linq;
using Swart.DomainDrivenDesign.Domain;
using Swart.DomainDrivenDesign.Repositories;
using Swart.DomainDrivenDesign;

namespace Swart.Repositories.EntityFramework
{
    public class Repository<TEntity, TKey> : RepositoryBase<TEntity, TKey>
        where TEntity : class, IEntity<TKey> where TKey : IComparable, IEquatable<TKey>
    {
        protected readonly IQueryableUnitOfWork QueryableUnitOfWork;

        public Repository(IQueryableUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("unitOfWork");

            QueryableUnitOfWork = unitOfWork;
        }

        protected override IQueryable<TEntity> _List()
        {
            return GetSet();
        }

        #region Basic
        public override IUnitOfWork UnitOfWork { get { return QueryableUnitOfWork; } }        

        #endregion        

        #region List
        protected override void AddEntity(TEntity entity)
        {
            if (entity != null)
                GetSet().Add(entity); // add new entity in this set
            else
                throw new ArgumentNullException();
        }

        protected override void DeleteEntity(TEntity entity)
        {
            //attach entity if not exist
            GetSet().Attach(entity);

            //set as "removed"
            GetSet().Remove(entity);
        }

        protected override IVoidResult UpdateEntity(TEntity entity)
        {
            //attach entity if not exist and mark it as modified
            QueryableUnitOfWork.SetModified(entity);

            return new VoidResult();
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
