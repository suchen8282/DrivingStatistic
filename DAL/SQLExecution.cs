using DrivingStatistic.BLL;
using DrivingStatistic.DAL.Model;

namespace DrivingStatistic.DAL;
public class SQLExecution : ISQLExecution
{
    public async Task<List<Driver>> GetDriversAsync()
    {
        var drivers = new List<Driver>();
        using var conn = DbConnectionFactory.GetConnection();
        await conn.OpenAsync();
        var cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT * FROM Driver";
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            drivers.Add(new Driver
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Birthday = reader.GetDateTime(2)
            });
        }
        return drivers;
    }

    public async Task<Driver?> GetDriverAsync(int id)
    {
        using var conn = DbConnectionFactory.GetConnection();
        await conn.OpenAsync();
        var cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT * FROM Driver WHERE id = $id";
        cmd.Parameters.AddWithValue("$id", id);

        using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
            return null; 
        }

        return new Driver
        {
            Id = id,
            Name = reader.GetString(1),
            Birthday = reader.GetDateTime(2)
        };
    }

    public async Task<Driver> AddDriverAsync(Driver driver)
    {
        using var conn = DbConnectionFactory.GetConnection();
        await conn.OpenAsync();
        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO Driver (Name, Birthday) 
            VALUES ($name, $birthday)";
        cmd.Parameters.AddWithValue("$name", driver.Name);
        cmd.Parameters.AddWithValue("$birthday", driver.Birthday);
        await cmd.ExecuteNonQueryAsync();

        // Get the last inserted ID
        var idCmd = conn.CreateCommand();
        idCmd.CommandText = "SELECT last_insert_rowid();";
        var idObj = await idCmd.ExecuteScalarAsync();

        return new Driver
        {
            Id = Convert.ToInt32(idObj),
            Name = driver.Name,
            Birthday = driver.Birthday
        };
    }

    public async Task<List<TruckStats>> GetAllTruckStatsAsync()
    {
        var stats = new List<TruckStats>();
        using var conn = DbConnectionFactory.GetConnection();
        await conn.OpenAsync();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM TruckStats";
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            stats.Add(new TruckStats
            {
                Id = reader.GetInt32(0),
                DriverId = reader.GetInt32(1),
                Latitude = reader.GetDouble(2),
                Longitude = reader.GetDouble(3),
                TimeStamp = reader.GetDateTime(4),
                DriverAgeAtTimeStamp = reader.GetInt32(5),
                Country = await reader.IsDBNullAsync(6) ? null : reader.GetString(6)
            });
        }
        return stats;
    }

    public async Task<List<TruckStats>> GetTruckStatsAsync(int driverId)
    {
        var stats = new List<TruckStats>();
        using var conn = DbConnectionFactory.GetConnection();
        await conn.OpenAsync();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM TruckStats WHERE DriverId = $driverId";
        cmd.Parameters.AddWithValue("$driverId", driverId);

        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            stats.Add(new TruckStats
            {
                Id = reader.GetInt32(0),
                DriverId = reader.GetInt32(1),
                Latitude = reader.GetDouble(2),
                Longitude = reader.GetDouble(3),
                TimeStamp = reader.GetDateTime(4),
                DriverAgeAtTimeStamp = reader.GetInt32(5),
                Country = await reader.IsDBNullAsync(6) ? null : reader.GetString(6)
            });
        }
        return stats;
    }

    public async Task<bool> AddTruckStatsAsync(TruckStats TruckStats)
    {
        using var conn = DbConnectionFactory.GetConnection();
        await conn.OpenAsync();
        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
        INSERT INTO TruckStats (DriverId, Latitude, Longitude, TimeStamp) 
        VALUES ($driverId, $latitude, $longitude, $timeStamp)";

        cmd.Parameters.AddWithValue("$driverId", TruckStats.DriverId);
        cmd.Parameters.AddWithValue("$latitude", TruckStats.Latitude);
        cmd.Parameters.AddWithValue("$longitude", TruckStats.Longitude);
        cmd.Parameters.AddWithValue("$timeStamp", TruckStats.TimeStamp);
        var rows = await cmd.ExecuteNonQueryAsync();
        return rows > 0;
    }

    public async Task<List<TruckStats>> GetStatsWithoutCountryAsync()
    {
        var stats = new List<TruckStats>();
        using var conn = DbConnectionFactory.GetConnection();
        await conn.OpenAsync();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, Latitude, Longitude FROM TruckStats WHERE IsCountryResolved = 0";

        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            stats.Add(new TruckStats
            {
                Id = reader.GetInt32(0),
                Latitude = reader.GetDouble(1),
                Longitude = reader.GetDouble(2)
            });
        }
        return stats;
    }

    public async Task<bool> UpdateStatsWithCountryAsync(TruckStats TruckStats)
    {
        using var conn = DbConnectionFactory.GetConnection();
        await conn.OpenAsync();
        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
        UPDATE TruckStats
        SET Country = $country, IsCountryResolved = 1
        WHERE Id = $id";
        cmd.Parameters.AddWithValue("$country", TruckStats.Country);
        cmd.Parameters.AddWithValue("$id", TruckStats.Id);
        var rows = await cmd.ExecuteNonQueryAsync();
        return rows > 0;
    }

    public async Task<List<TruckStats>> GetStatsWithoutAgeAsync()
    {
        var stats = new List<TruckStats>();
        using var conn = DbConnectionFactory.GetConnection();
        await conn.OpenAsync();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT DriverId, TimeStamp FROM TruckStats WHERE IsDriverAgeResolved = 0";

        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            stats.Add(new TruckStats
            {
                DriverId = reader.GetInt32(0),
                TimeStamp = reader.GetDateTime(1),
            });
        }
        return stats;
    }
    public async Task<bool> UpdateStatsWithAgeAsync(TruckStats TruckStats)
    {
        using var conn = DbConnectionFactory.GetConnection();
        await conn.OpenAsync();
        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
        UPDATE TruckStats
        SET DriverAgeAtTimeStamp = $driverAge, IsDriverAgeResolved = 1
        WHERE DriverId = $driverId
        ";
        cmd.Parameters.AddWithValue("$driverAge", TruckStats.DriverAgeAtTimeStamp);
        cmd.Parameters.AddWithValue("$driverId", TruckStats.DriverId);
        var rows = await cmd.ExecuteNonQueryAsync();
        return rows > 0;
    }

    public async Task<List<TruckStats>> GetStatsByQueryAsync(DistanceQuery query)
    {
        var stats = new List<TruckStats>();
        using var conn = DbConnectionFactory.GetConnection();
        await conn.OpenAsync();
        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
        SELECT DriverId, Latitude, Longitude FROM TruckStats
        WHERE DriverAgeAtTimeStamp > $queryAge AND Country = $queryCountry
        AND TimeStamp >= $periodStart AND TimeStamp <= $periodEnd
        ";
        cmd.Parameters.AddWithValue("$queryAge", query.Age);
        cmd.Parameters.AddWithValue("$queryCountry", query.Country);
        cmd.Parameters.AddWithValue("$periodStart", query.PeriodStart);
        cmd.Parameters.AddWithValue("$periodEnd", query.PeriodEnd);

        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            stats.Add(new TruckStats
            {
                DriverId = reader.GetInt32(0),
                Latitude = reader.GetDouble(1),
                Longitude = reader.GetDouble(2),
            });
        }
        return stats;
    }
}
