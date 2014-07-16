using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;

namespace ContosoUniversity.DAL
{
    public class SchoolConfiguration : DbConfiguration
    {
        public SchoolConfiguration()
        {
            DbInterception.Add(new SchoolInterceptorLogging());
        }
    }
}