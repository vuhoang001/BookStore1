using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Chassis.Mapper;

public static class Extensions
{
    public static void AddMapper(this IServiceCollection services, Type type)
    {
        services.Scan(scan =>
            scan.FromAssemblies(type.Assembly)
                .AddClasses(classes =>
                    classes.AssignableTo(typeof(IMapper<,>)).Where(t => !t.IsAbstract)
                )
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );
    }
}
