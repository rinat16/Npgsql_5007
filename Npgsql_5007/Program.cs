using Npgsql;

const string pgPort = "7432";
const string pgbouncerPort = "5432";
const string connectionStringTemplate = "Host=localhost;Port={0};Database=test;Username=test;Password=test;Application Name={1};";

await Test(string.Format(connectionStringTemplate, pgPort, "direct_app"));
await Test(string.Format(connectionStringTemplate, pgbouncerPort, "wit_pgbouncer_app"));

static async Task Test(string connectionString)
{
    await using var conn = new NpgsqlConnection(connectionString);
    Console.WriteLine($"connected to <{connectionString}>");

    await conn.OpenAsync();
    Console.WriteLine("first request");
    await using var command = new NpgsqlCommand("SHOW application_name", conn);
    Console.WriteLine(await command.ExecuteScalarAsync());
    await conn.CloseAsync();
    
    await conn.OpenAsync();
    Console.WriteLine("second request");
    Console.WriteLine(await command.ExecuteScalarAsync());
    await conn.CloseAsync();
}