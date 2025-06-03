using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie_Recommendation.Utilities
{
    public class DbSetting
    {
		public static string ConnectionString(IConfiguration configuration, string key)
		{
			var serverIp = configuration["Database:" + key + ":ServerIp"];
			var dbName = configuration["Database:" + key + ":DbName"];
			var dbUser = configuration["Database:" + key + ":DbUser"];
			var dbPassword = configuration["Database:" + key + ":DbPassword"];
			var conString = $"Server={serverIp};Database={dbName};User Id={dbUser};password={dbPassword};";
			return conString;
		}
	}
}
