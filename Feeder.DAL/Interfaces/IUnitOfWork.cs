using Feeder.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Feeder.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRssRepository Rsses { get; }
       // IRepository<Atom> Atoms { get; }
        void Save();
    }
}
