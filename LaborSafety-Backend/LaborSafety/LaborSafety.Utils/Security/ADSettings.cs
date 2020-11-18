using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Utils.Security
{
    public class ADSettings
    {
        public static Object GetUserByImpersonate(string id)
        {
            try
            {
                var adTernium = ConfigurationManager.AppSettings["ADTERNIUM"];
                using (ImpersonateUtils.ImpersonateCurrentUser())
                {
                    DirectorySearcher search = new DirectorySearcher(adTernium);
                    search.Filter = "(samaccountname=" + id + ")";
                    return search.FindAll();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool AuthAD(string id, string password)
        {
# if DEBUG
            return true;
#else
            var adTernium = ConfigurationManager.AppSettings["ADTERNIUM"];
            DirectoryEntry autenticacao = new DirectoryEntry(adTernium, id, password);
            DirectorySearcher search = new DirectorySearcher(autenticacao);
            search.Filter = "(samaccountname=" + id + ")";
            SearchResult result = search.FindOne();
            if (null == result)
                return false;
            else
                return true;
#endif
        }
    }
}
