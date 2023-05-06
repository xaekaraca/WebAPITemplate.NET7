using Data.Models.Base;

namespace Data.Mappers.Base;

public interface IBaseMapper<in TSource, out TDestination> 
    where TSource:BaseModel 
    where TDestination:BaseModel
{
    TDestination Map(TSource source);
    
    IEnumerable<TDestination> Map(IEnumerable<TSource> source);
}


/*public class UserMapper : IMapper<User, UserModel>
{
    public UserModel Map(User source)
    {
        return new UserModel
        {
            Id = source.Id,
            Name = source.Name,
            Email = source.Email
        };
    }

    public IEnumerable<UserModel> Map(IEnumerable<User> sources)
    {
        return sources.Select(source => Map(source));
    }
}*/