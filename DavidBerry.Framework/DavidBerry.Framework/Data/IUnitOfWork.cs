using System;
using System.Collections.Generic;
using System.Text;

namespace DavidBerry.Framework.Data
{
    public interface IUnitOfWork
    {

        void SaveChanges();

    }
}
