using System.Collections.Generic;

namespace Vavatech.DotnetCore.IServices
{
    public interface IEntityService<TEntity, TKey>
    {
        IEnumerable<TEntity> Get();
        TEntity Get(TKey id);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TKey id);
    }

    public interface IEntityService<TEntity> : IEntityService<TEntity, int>
    {

    }

}
