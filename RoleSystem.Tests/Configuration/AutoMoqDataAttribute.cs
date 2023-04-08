﻿using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoleSystem.Core.Mappings;
using RoleSystem.DataContext.DataContexts;

namespace RoleSystem.Tests.Configuration;

public class AutoMoqDataAttribute : AutoDataAttribute
{
    public AutoMoqDataAttribute()
        : base(() => GetFixture())
    {
    }

    public static IFixture GetFixture()
    {
        var fixture = new Fixture();

        var options = new DbContextOptionsBuilder<RoleContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        var context = new RoleContext(options);

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        fixture.Register<RoleContext>(() => context);
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
