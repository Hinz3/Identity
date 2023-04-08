using AutoFixture;
using RoleSystem.DataContext.DataContexts;
using Microsoft.EntityFrameworkCore;

namespace RoleSystem.Tests.Configuration;

public class SQLiteEntityFrameworkCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        var options = new DbContextOptionsBuilder<RoleContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;

        var context = new RoleContext(options);
        context.Database.OpenConnection();
        context.Database.EnsureCreated();
        fixture.Register<RoleContext>(() => context);
    }
}