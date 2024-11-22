using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;

namespace TravelOrganization.Data;

public class DataContext
{
    private readonly IConfiguration _configuration;

    public DataContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    // Behind the scenes the connections are pooled (reused).
    // It is safer to create a connection and dispose it each SQL query.
    // https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-connection-pooling
    public IDbConnection CreateConnection()
    {
        return new SqliteConnection(_configuration.GetConnectionString("TravelDatabase"));
    }

    public async Task Init()
    {
        string? dbPath = _configuration["Paths:Db"];
        
        if (File.Exists(dbPath))
            return;

        string? dbInitSql = _configuration["Paths:DbInitSql"];
        
        if (!File.Exists(dbInitSql))
            throw new FileNotFoundException(dbInitSql);
        
        using var connection = CreateConnection();
        
        string sql = File.ReadAllText(dbInitSql);
        
        await connection.ExecuteAsync(sql);
    }
}