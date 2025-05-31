namespace DrivingStatistic.DAL
{
    public class DbInit
    {
        public async Task DatabaseInitAsync()
        {
            await using var connection = DbConnectionFactory.GetConnection();
            await connection.OpenAsync();

            // Check Driver table
            var checkDriversCmd = connection.CreateCommand();
            checkDriversCmd.CommandText = @"
                    SELECT COUNT(*) FROM sqlite_master 
                    WHERE type='table' AND name='Driver'";
            
            var result = await checkDriversCmd.ExecuteScalarAsync();
            var driversTableCount = Convert.ToInt64(result);

            if (driversTableCount == 0)
            {
                var createDriversCmd = connection.CreateCommand();
                createDriversCmd.CommandText = @"
                    CREATE TABLE Driver (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Birthday TEXT NOT NULL
                    )";
                await createDriversCmd.ExecuteNonQueryAsync();
            }

            // Check TruckStats table
            var checkLogsCmd = connection.CreateCommand();
            checkLogsCmd.CommandText = @"
                    SELECT COUNT(*) FROM sqlite_master 
                    WHERE type='table' AND name='TruckStats'";
            var logResult = await checkLogsCmd.ExecuteScalarAsync();
            var logsTableCount = Convert.ToInt64(logResult);

            if (logsTableCount == 0)
            {
                using var createLogsCmd = connection.CreateCommand();
                createLogsCmd.CommandText = @"
                    CREATE TABLE TruckStats (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        DriverId INTEGER NOT NULL,
                        Latitude REAL NOT NULL,
                        Longitude REAL NOT NULL,
                        Timestamp TEXT NOT NULL,
                        DriverAgeAtTimeStamp INTEGER DEFAULT 0,
                        Country TEXT NULL,
                        IsDriverAgeResolved INTEGER DEFAULT 0,
                        IsCountryResolved INTEGER DEFAULT 0,
                        FOREIGN KEY (DriverId) REFERENCES Driver(Id)
                    )";
                await createLogsCmd.ExecuteNonQueryAsync();
            }
        }
    }
}