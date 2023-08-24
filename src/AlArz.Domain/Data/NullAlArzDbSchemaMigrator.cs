using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace AlArz.Data;

/* This is used if database provider does't define
 * IAlArzDbSchemaMigrator implementation.
 */
public class NullAlArzDbSchemaMigrator : IAlArzDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
