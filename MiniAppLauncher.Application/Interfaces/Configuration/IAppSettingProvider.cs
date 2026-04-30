using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniAppLauncher.Application.Interfaces.Configuration
{
    public  interface IAppSettingProvider
    {
        string GetString(string key);
        int GetInt(string key);
        bool GetBool(string key);
    }
}
