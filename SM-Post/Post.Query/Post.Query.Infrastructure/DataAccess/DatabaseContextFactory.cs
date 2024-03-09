using Microsoft.EntityFrameworkCore;

namespace Post.Query.Infrastructure.DataAccess;

public class DatabaseContextFactory
{
    public readonly Action<DbContextOptionsBuilder> _configureDBContext;

    public DatabaseContextFactory(Action<DbContextOptionsBuilder> configureDBContext)
    {
        _configureDBContext = configureDBContext;
    }

    public DatabaseContext CreateDBContext(){
        DbContextOptionsBuilder<DatabaseContext> optionsBuilder = new();
        _configureDBContext(optionsBuilder);
        return new DatabaseContext(optionsBuilder.Options);
    }
}
