namespace Data.Models.Base;

public class BaseModel
{
    public long Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public bool IsDeleted { get; set; }
}

public class BaseCreateModel
{
    
}

public class BaseViewModel
{
    public long Id { get; set; }
}

public class BaseUpdateModel
{
    
}

