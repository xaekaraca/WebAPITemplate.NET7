
using Data.Models.Base;

namespace WebAPI.Services.Base;

/// <summary>
/// This is a base service interface that all services should implement.
/// This interface is generic and has 4 generic parameters.
/// This interface has a warning which indicates a recommendation. Generic usage is not recommended in an interface when generic entities more than 2.
/// In this case, it is safe to use since entities are different from each other and general structure of project will work around this interface.
/// </summary>
/// <typeparam name="TBaseModel">This is the base entity model of implemented service.</typeparam>
/// <typeparam name="TCreateModel">This is the base create model of implemented service.</typeparam>
/// <typeparam name="TUpdateModel">This is the base update model of implemented service.</typeparam>
/// <typeparam name="TId">Id can be a long or string depends on service and model so we will use generic TId to be able to use this interface with all services.</typeparam>
/// <typeparam name="TQueryFilterModel">This model references to query filter which is implemented for queries which returns a list of content.</typeparam>
public interface IBaseService<TBaseModel,in TCreateModel,in TUpdateModel, in TId,in TQueryFilterModel> 
    where TBaseModel : BaseModel
    where TCreateModel : BaseCreateModel
    where TUpdateModel : BaseUpdateModel
    where TQueryFilterModel : QueryFilterModel
{
    /// <summary>
    /// This method queries the database by id and returns the result.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Base entity model of implemented service</returns>
    Task<TBaseModel?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// This method queries the database by filter and returns the result.
    /// </summary>
    /// <param name="filter">Filter comes from controller which is asked from api user (user itself via api or frontend)</param>
    /// <param name="cancellationToken"></param>
    /// <returns>List of base entity model of implemented service</returns>
    Task<List<TBaseModel>> GetAllAsync(TQueryFilterModel filter, CancellationToken cancellationToken = default);
    /// <summary>
    /// This method creates a new entity in database and returns the result.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Base entity model of implemented service</returns>
    Task<TBaseModel> CreateAsync(TCreateModel model, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// This method updates an entity in database and returns the result.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Base entity model of implemented service</returns>
    Task<TBaseModel> UpdateAsync(TId id, TUpdateModel model, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// This method deletes an entity in database and returns the result.
    /// In general, we don't delete an entity from database, we just update IsDeleted field to true.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteAsync(TId id, CancellationToken cancellationToken = default);
}