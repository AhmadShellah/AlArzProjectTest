using System.Threading.Tasks;

namespace AlArz.Data;

public interface IAlArzDbSchemaMigrator
{
    Task MigrateAsync();
}
