using Microsoft.EntityFrameworkCore;
using SipayApi.Base;
using System.Linq.Expressions;

namespace SipayApi.Data.Repository;

public class GenericRepository<Entity> : IGenericRepository<Entity> where Entity : BaseModel
{
    private readonly SimDbContext dbContext;
    public GenericRepository(SimDbContext dbContext)
    {
        this.dbContext = dbContext;
    }


	public IEnumerable<Entity> Where(Expression<Func<Entity, bool>> expression)
	{
		return dbContext.Set<Entity>().Where(expression).ToList();
	}
}
