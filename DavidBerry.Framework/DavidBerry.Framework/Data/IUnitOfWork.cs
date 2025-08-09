using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DavidBerry.Framework.Data
{
    public interface IUnitOfWork
    {

        public void SaveChanges();

        public Task SaveChangesAsync();
    }
}
