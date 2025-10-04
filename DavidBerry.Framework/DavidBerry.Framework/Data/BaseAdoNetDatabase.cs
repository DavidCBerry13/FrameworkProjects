using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidBerry.Framework.Data
{
    public abstract class BaseAdoNetDatabase<T> : IUnitOfWork, IDisposable where T : IDbConnection, new()
    {


        public BaseAdoNetDatabase(string connectionString)
        {
            _connectionString = connectionString;
            DbConnection = Activator.CreateInstance<T>();
            DbConnection.ConnectionString = _connectionString;
            DbConnection.Open();
            DbTransaction = DbConnection.BeginTransaction();
        }




        private string _connectionString;



        protected IDbConnection DbConnection { get; init; }


        protected IDbTransaction DbTransaction { get; init; }


        public void CommitChanges()
        {
            DbTransaction.Commit();

        }

        public void RollbackChanges()
        {
            DbTransaction.Rollback();
        }

        public void Dispose()
        {
            if ( DbTransaction != null )
            {
                DbTransaction.Dispose();
            }

            if ( DbConnection != null )
            {
                if (DbConnection.State == ConnectionState.Open)
                    DbConnection.Close();
                DbConnection.Dispose();
            }
        }
    }
}
