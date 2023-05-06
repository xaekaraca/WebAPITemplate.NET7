using System.Diagnostics.CodeAnalysis;
using Data.Models.Base;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Result;
using WebAPI.Services.Base;

namespace WebAPI.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public abstract class BaseController<TBaseModel, TCreateModel, TViewModel, TUpdateModel, TId, TQueryFilterModel> : ControllerBase
        where TCreateModel : BaseCreateModel
        where TViewModel : BaseViewModel
        where TUpdateModel : BaseUpdateModel
        where TQueryFilterModel : QueryFilterModel
        where TBaseModel : BaseModel
    {
        protected readonly IBaseService<TBaseModel, TCreateModel, TUpdateModel, TId, TQueryFilterModel> Service;

        protected BaseController(IBaseService<TBaseModel, TCreateModel, TUpdateModel, TId, TQueryFilterModel> service)
        {
            Service = service;
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        protected abstract Task<IServiceResult<TViewModel>> ToViewModelAsync(TBaseModel entity, CancellationToken cancellationToken = default);

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        protected abstract Task<IServiceResult<List<TViewModel?>>> ToViewModelListAsync(IEnumerable<TBaseModel> entities, CancellationToken cancellationToken = default);

        [HttpGet]
        public virtual async Task<IServiceResult<List<TViewModel?>>> GetAsync([FromQuery] TQueryFilterModel query, CancellationToken cancellationToken = default)
        {
            var entities = await Service.GetAllAsync(query, cancellationToken);
            if (entities.Count == 0)
                return ServiceResult<List<TViewModel?>>.NoContent(new List<TViewModel?>());

            return await ToViewModelListAsync(entities, cancellationToken);
        }

        [HttpGet("{id}")]
        public virtual async Task<IServiceResult<TViewModel>> GetAsync([FromRoute] TId id, CancellationToken cancellationToken = default)
        {
            var entity = await Service.GetByIdAsync(id, cancellationToken);
            if (entity is null)
                return ServiceResult<TViewModel>.NoContent(null);

            return await ToViewModelAsync(entity, cancellationToken);
        }

        [HttpPost]
        public virtual async Task<IServiceResult<TViewModel>> CreateAsync([FromBody] TCreateModel model, CancellationToken cancellationToken = default)
        {
            var entity = await Service.CreateAsync(model, cancellationToken);
            return await ToViewModelAsync(entity, cancellationToken);
        }

        [HttpPut("{id}")]
        public virtual async Task<IServiceResult<TViewModel>> UpdateAsync([FromRoute] TId id, [FromBody] TUpdateModel model, CancellationToken cancellationToken = default)
        {
            var entity = await Service.UpdateAsync(id, model, cancellationToken);
            return await ToViewModelAsync(entity, cancellationToken);
        }

        [HttpDelete("{id}")]
        public virtual async Task<IServiceResult> DeleteAsync([FromRoute] TId id, CancellationToken cancellationToken = default)
        {
            await Service.DeleteAsync(id, cancellationToken);
            return ServiceResult.NoContent();
        }
    }
}