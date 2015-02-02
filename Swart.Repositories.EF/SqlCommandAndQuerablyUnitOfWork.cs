using System.Collections.Generic;
using Swart.DomainDrivenDesign.Repositories;

namespace Swart.Repositories.EntityFramework
{
    public class SqlCommandAndQuerablyUnitOfWork : QueryableUnitOfWork, ISqlCommand
    {
        #region Contructor(s)
        public SqlCommandAndQuerablyUnitOfWork(){}

        public SqlCommandAndQuerablyUnitOfWork(string connectionString)
            :base(connectionString)
        {}
        #endregion

        #region ISqlCommand
        public IEnumerable<TObject> ExecuteQuery<TObject>(string sqlQuery, params object[] parameters)
        {
            return Database.SqlQuery<TObject>(sqlQuery, parameters);
        }

        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return Database.ExecuteSqlCommand(sqlCommand, parameters);
        }
        #endregion        
    }    
}
