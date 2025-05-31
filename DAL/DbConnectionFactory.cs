using Microsoft.Data.Sqlite;

namespace DrivingStatistic.DAL
{
    public static class DbConnectionFactory
    {
        public static string DbPath
        {
            get
            {
                var folder = Environment.SpecialFolder.LocalApplicationData;
                var path = Environment.GetFolderPath(folder);
                return Path.Join(path, "TruckPlan.db");
            }
        }

        public static SqliteConnection GetConnection()
        {
            return new SqliteConnection($"Data Source={DbPath}");
        }
    }
}