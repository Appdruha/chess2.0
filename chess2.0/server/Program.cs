var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5000");
var app = builder.Build();

// добавляем роуты (маршруты)
app.MapGet("/", () => "Это минимальное API сайта");
app.MapGet("/version", () => $"Версия приложения 0.0.1");

// запускаем приложение
app.Run();