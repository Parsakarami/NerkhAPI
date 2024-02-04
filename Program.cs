var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var app = builder.Build();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Main}/{action=Index}/{id?}"
);
// app.MapGet("",() => "Use \"/Nerkh\" for currencies.");
// app.MapGet("/Nerkh", () => Random.Shared.Next(40000,60000).ToString());
app.Run();
