using System.Data;

namespace MiniAppLauncher.Application.Interfaces.DataAccess
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
