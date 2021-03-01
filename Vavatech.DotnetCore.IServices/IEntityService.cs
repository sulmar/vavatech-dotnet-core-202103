using System.Collections.Generic;
using Vavatech.DotnetCore.Models.SearchCriterias;

namespace Vavatech.DotnetCore.IServices
{
    public interface IEntityService<TEntity, TKey, TSearchCriteria>
    {
        IEnumerable<TEntity> Get();
        TEntity Get(TKey id);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TKey id);

        IEnumerable<TEntity> Get(TSearchCriteria searchCriteria);
    }

    public interface IEntityService<TEntity, TSearchCriteria> : IEntityService<TEntity, int, TSearchCriteria>
        where TSearchCriteria : SearchCriteria
    {

    }

}
