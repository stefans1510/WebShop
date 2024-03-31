using System.Collections;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShopContext context;
        private Hashtable repositories;

        public UnitOfWork(ShopContext context)
        {
            this.context = context;
        }

        public async Task<int> Complete()
        {
           return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if (repositories == null) repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                var reposoryInstance = Activator.CreateInstance(
                    repositoryType.MakeGenericType(typeof(TEntity)), context
                );

                repositories.Add(type, reposoryInstance);
            }

            return (IGenericRepository<TEntity>) repositories[type];
        }
    }
}