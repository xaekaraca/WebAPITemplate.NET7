
using Data.Models.Base;

namespace WebAPI.Services.Base;

public interface IBaseService<TBaseEntity,in TCreateEntity,in TUpdateEntity, in TId,in TFilterModel> 
    where TBaseEntity : BaseModel
    where TCreateEntity : BaseCreateModel
    where TUpdateEntity : BaseUpdateModel
    where TFilterModel : FilterModel
{
    Task<TBaseEntity> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    Task<List<TBaseEntity>> GetListAsync(TFilterModel filter, CancellationToken cancellationToken = default);
    Task<TBaseEntity> CreateAsync(TCreateEntity model, CancellationToken cancellationToken = default);
    Task<TBaseEntity> UpdateAsync(TId id, TUpdateEntity model, CancellationToken cancellationToken = default);
    Task DeleteAsync(TId id, CancellationToken cancellationToken = default);
}