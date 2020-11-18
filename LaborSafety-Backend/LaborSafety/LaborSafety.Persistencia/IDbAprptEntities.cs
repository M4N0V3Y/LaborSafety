using System.Data.Entity;


namespace LaborSafety.Persistencia
{
    public interface IDbLaborSafetyEntities : IDatabaseInitializer<DB_LaborSafetyEntities>
    {
        DB_LaborSafetyEntities GetDB_LaborSafetyEntities();
    }
}
