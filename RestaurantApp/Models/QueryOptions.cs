using System.Linq.Expressions;

namespace RestaurantApp.Models;

public class QueryOptions<T> where T : class
{
    // takes an instance of type T and returns an object
    // allows LINQ providers to translate the Where clause into real queries.
    public Expression<Func<T , object>> OrderBy { get; set; }
    
    public Expression<Func<T , bool>> Where { get; set; }
    
    private string[] includes = Array.Empty<string>();
    
    public string Includes
    {
        set => includes = value.Replace(" " , "").Split(',');
    }
    
    public string[] GetIncludes() => includes;
    
    // checks if the Where property has been set
    //It doesn’t need a setter because it is derived from the Where property.
    // It’s just a read-only flag that’s calculated on the fly
    public bool HasWhere => Where != null;
    
    public bool HasOrderBy => OrderBy != null;
    
}