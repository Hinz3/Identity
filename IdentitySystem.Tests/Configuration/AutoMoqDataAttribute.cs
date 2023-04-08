using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using IdentitySystem.Core.Mappings;
using IdentitySystem.DataContext.DataContexts;

namespace IdentitySystem.Tests;

public class AutoMoqDataAttribute : AutoDataAttribute
{
    public AutoMoqDataAttribute()
        : base(() => GetFixture())
    {
    }

    public static IFixture GetFixture()
    {
        var fixture = new Fixture();

        var options = new DbContextOptionsBuilder<IdentityContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        var context = new IdentityContext(options);

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        fixture.Register<IdentityContext>(() => context);
        fixture.Customize(new AutoMoqCustomization());

        var mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(EventMappings));
            cfg.AddProfile(typeof(EntityMappings));
        }));

        fixture.Register<IMapper>(() => mapper);

        return fixture;
    }
}
