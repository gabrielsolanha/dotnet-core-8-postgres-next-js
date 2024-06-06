using System;
using System.Threading.Tasks;

namespace AplicacaoWeb.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        void Rollback();
        void BeginTransaction();
        Task SaveAsync();
    }
}
