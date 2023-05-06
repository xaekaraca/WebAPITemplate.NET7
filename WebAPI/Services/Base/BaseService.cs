using System.Linq.Expressions;
using Data.Context.Base;
using Data.Models.Base;
using Exception;
using Microsoft.EntityFrameworkCore;
using Util;

namespace WebAPI.Services.Base;

public abstract class BaseService<TBaseModel,TCreateModel,TUpdateModel,TId,TQueryFilterModel> : IBaseService<TBaseModel,TCreateModel,TUpdateModel,TId,TQueryFilterModel> 
    where TCreateModel : BaseCreateModel
    where TUpdateModel : BaseUpdateModel
    where TQueryFilterModel : QueryFilterModel
    where TBaseModel : BaseModel
{
    private readonly SqlContext _sqlContext;
    
    protected BaseService(SqlContext sqlContext)
    {
        _sqlContext = sqlContext;
    }
    
    
    protected abstract DbSet<TBaseModel> GeTBaseModelDbSet();

    protected abstract Task<TBaseModel> OnBeforeCreateAsync(TCreateModel model, CancellationToken cancellationToken = default);

    protected abstract Task OnAfterCreateAsync(TBaseModel baseModel, CancellationToken cancellationToken = default);

    protected abstract Task OnBeforeUpdateAsync(TUpdateModel updateModel, TBaseModel baseModel, CancellationToken cancellationToken = default);

    protected abstract Task OnAfterUpdateAsync(TBaseModel baseModel, CancellationToken cancellationToken = default);

    protected abstract Task OnBeforeDeleteAsync(TBaseModel baseModel, CancellationToken cancellationToken = default);

    protected abstract Task OnAfterDeleteAsync(TBaseModel baseModel, CancellationToken cancellationToken = default);
    
    public virtual async Task<TBaseModel?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await GeTBaseModelDbSet().FirstOrDefaultAsync(GenerateLambdaExpressionForId(id), cancellationToken);
        }
        catch (System.Exception e)
        {
            throw new DatabaseException(e);
        }
    }

    public virtual async Task<List<TBaseModel>> GetAllAsync(TQueryFilterModel queryFilterModel, CancellationToken cancellationToken = default)
    {
        try
        {
            return await GeTBaseModelDbSet().ToListAsync(cancellationToken);
        }
        catch (System.Exception e)
        {
            throw new DatabaseException(e);
        }
    }

    public virtual async Task<TBaseModel> CreateAsync(TCreateModel model, CancellationToken cancellationToken = default)
    {
        var entity = await OnBeforeCreateAsync(model, cancellationToken);

        try
        {
            await _sqlContext.AddAsync(entity, cancellationToken);
            await _sqlContext.SaveChangesAsync(cancellationToken);
        }
        catch (System.Exception e)
        {
            throw new DatabaseException(e);
        }

        await OnAfterCreateAsync(entity, cancellationToken);
        return (await GetByIdAsync((TId)Helpers.GetPropertyValue(entity, "Id"), cancellationToken))!;
    }

    public virtual async Task<TBaseModel> UpdateAsync(TId id, TUpdateModel model, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity is null)
            throw new NotFoundException();

        await OnBeforeUpdateAsync(model, entity, cancellationToken);

        try
        {
            _sqlContext.Update(entity);
            await _sqlContext.SaveChangesAsync(cancellationToken);
        }
        catch (System.Exception e)
        {
            throw new DatabaseException(e);
        }

        await OnAfterUpdateAsync(entity, cancellationToken);
        return (await GetByIdAsync((TId)Helpers.GetPropertyValue(entity, "Id"), cancellationToken))!;
    }

    public virtual async Task DeleteAsync(TId id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity is null)
            throw new NotFoundException();

        await OnBeforeDeleteAsync(entity, cancellationToken);

        try
        {
            _sqlContext.Update(entity);
            await _sqlContext.SaveChangesAsync(cancellationToken);
        }
        catch (System.Exception e)
        {
            throw new DatabaseException(e);
        }

        await OnAfterDeleteAsync(entity, cancellationToken);
    }

    private static Expression<Func<TBaseModel, bool>> GenerateLambdaExpressionForId(TId id)
    {
        var mappingEntityParameterExpression = Expression.Parameter(typeof(TBaseModel));
        var memberExpression = Expression.PropertyOrField(mappingEntityParameterExpression, "Id");
        var constantExpression = Expression.Constant(id);

        var nonDeleteExpression = Expression.PropertyOrField(mappingEntityParameterExpression, "IsDeleted");
        var nonDeleteConstantExpression = Expression.Constant(false);

        var deleteExpression = Expression.Equal(nonDeleteExpression, nonDeleteConstantExpression);
        var binaryExpression = Expression.Equal(memberExpression, constantExpression);

        var combinedExpression = Expression.AndAlso(binaryExpression, deleteExpression);

        return Expression.Lambda<Func<TBaseModel, bool>>(combinedExpression, mappingEntityParameterExpression);
    }
    
}