using System.Data.Entity;
using LM.Api.Migrations;
using LM.Api.Models;

namespace LM.Api
{
    public   class DbConfig
    {
        public static void ConfigureDb()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<LibraryDbContext, Configuration>());
        }
    }
}