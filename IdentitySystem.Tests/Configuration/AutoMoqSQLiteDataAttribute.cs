using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using AutoFixture;
using AutoMapper;
using IdentitySystem.Core.Mappings;

namespace IdentitySystem.Tests.Configuration;

public class AutoMoqSQLiteDataAttribute : AutoDataAttribute
{
    public AutoMoqSQLiteDataAttribute()
        : base(() => GetFixture())
    {
    }

    public static IFixture GetFixture()
    {
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        fixture.Customize(new AutoMoqCustomization());
        fixture.Customize(new SQLiteEntityFrameworkCustomization());

        var mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(EventMappings));
            cfg.AddProfile(typeof(EntityMappings));
        }));

        fixture.Register<IMapper>(() => mapper);

        return fixture;
    }
}