using System.Data.Entity;
using Swart.DomainDrivenDesign.Repositories;

namespace Swart.Repositories.EntityFramework
{
    public class DbTransaction:ITransaction
    {
        private readonly DbContextTransaction _dbContextTransaction;

        public DbTransaction(DbContextTransaction dbContextTransaction)
        {
            _dbContextTransaction = dbContextTransaction;
        }

        public void Dispose()
        {
            _dbContextTransaction.Dispose();
        }

        public void Commit()
        {
            _dbContextTransaction.Commit();
        }

        public void Rollback()
        {
            _dbContextTransaction.Rollback();
        }
    }
}
