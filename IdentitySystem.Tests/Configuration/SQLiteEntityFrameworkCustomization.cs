using AutoFixture;
using IdentitySystem.DataContext.DataContexts;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace IdentitySystem.Tests.Configuration;

public class SQLiteEntityFrameworkCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        var options = new DbContextOptionsBuilder<IdentityContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;

        var context = new IdentityContext(options);
        context.Database.OpenConnection();
        context.Database.EnsureCreated();
        fixture.Register<IdentityContext>(() => context);
    }
}