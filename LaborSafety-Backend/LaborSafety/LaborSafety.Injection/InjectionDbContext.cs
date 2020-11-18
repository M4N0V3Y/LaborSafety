using System;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using LaborSafety.Persistencia;
using LaborSafety.Utils.Security;

namespace LaborSafety.Injection
{
    public partial class InjectionDbContext : IDbLaborSafetyEntities, IDisposable
    {
        private bool UseDbCrypto
        {
            get
            {
                try
                {
                    if (ConfigurationManager.AppSettings["UseDbCripto"] == null)
                        return false;

                    return Convert.ToBoolean(ConfigurationManager.AppSettings["UseDbCripto"]);
                }
                catch
                {
                    return false;
                }
            }
        }

        private DB_LaborSafetyEntities _DB_LaborSafetyEntities;
        public DB_LaborSafetyEntities GetDB_LaborSafetyEntities()
        {
            return _DB_LaborSafetyEntities;
        }

        public InjectionDbContext()
        {
            InitializeDatabase(new Persistencia.DB_LaborSafetyEntities());
#if !DEBUG && UseDbCrypto
            _DB_EntregaColetaEntities.Database.Connection.ConnectionString = CreateConnectionConfigEncript();
#endif
            _DB_LaborSafetyEntities.Configuration.ProxyCreationEnabled = false;
            _DB_LaborSafetyEntities.Configuration.LazyLoadingEnabled = false;

            //TODO: TESTE

           // ((IObjectContextAdapter)this._DB_LaborSafetyEntities).ObjectContext.CommandTimeout = 600;
        }


        private static string CreateConnectionConfigEncript()
        {
            EntityConnectionStringBuilder ecsb = new EntityConnectionStringBuilder(ConfigurationManager.ConnectionStrings["DB_LaborSafetyEntities"].ConnectionString.ToString());

            var scxb = new SqlConnectionStringBuilder
                (ecsb.ProviderConnectionString);
            //decrypt your password.
            scxb.Password = ManagedAESCrypto.Decrypt(scxb.Password);

            return scxb.ConnectionString;
        }

        public void InitializeDatabase(DB_LaborSafetyEntities context)
        {
            _DB_LaborSafetyEntities = context;
        }

        public void Dispose()
        {
            _DB_LaborSafetyEntities = null;
        }
    }
}
