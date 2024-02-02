var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.MapGet("",() => "Use \"/Nerkh\" for currencies.");
app.MapGet("/Nerkh", () => Random.Shared.Next(40000,60000).ToString());
app.Run();
