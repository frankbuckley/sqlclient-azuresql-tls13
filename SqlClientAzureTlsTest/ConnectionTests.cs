using Microsoft.Data.SqlClient;
using Xunit;
using Xunit.Abstractions;

namespace SqlClientAzureTlsTest;

public class ConnectionTests(ITestOutputHelper output)
{

    [Fact]
    public async Task CanConnect()
    {
        var connectionString = GetConnectionString();

        var sanitized = connectionString
            .Replace(GetServer(), "********")
            .Replace(GetDatabase(), "********")
            .Replace(GetUser(), "********")
            .Replace(GetPassword(), "********");

        output.WriteLine($"Using connection string: {sanitized}");

        var connection = new SqlConnection(GetConnectionString());

        await connection.OpenAsync();

        var command = new SqlCommand("SELECT 1", connection);

        var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            output.WriteLine(reader[0].ToString());
        }

        output.WriteLine("Completed!");
    }
    static string GetConnectionString()
    {
        return $"Server=tcp:{GetServer()},1433;Initial Catalog={GetDatabase()};" +
            $"Persist Security Info=False;User ID={GetUser()};Password={GetPassword()};MultipleActiveResultSets=False;" +
            "Encrypt=Strict;TrustServerCertificate=False;Connection Timeout=30;";
    }

    static string GetServer()
    {
        return Environment.GetEnvironmentVariable("AZURE_SQL_TEST13_SERVER")
            ?? throw new InvalidOperationException();
    }

    static string GetDatabase()
    {
        return Environment.GetEnvironmentVariable("AZURE_SQL_TEST13_DATABASE")
            ?? throw new InvalidOperationException();
    }

    static string GetUser()
    {
        return Environment.GetEnvironmentVariable("AZURE_SQL_TEST13_USER")
            ?? throw new InvalidOperationException();
    }

    static string GetPassword()
    {
        return Environment.GetEnvironmentVariable("AZURE_SQL_TEST13_PASSWORD")
            ?? throw new InvalidOperationException();
    }
}
