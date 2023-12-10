using VideoServicePrj.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddScoped<IMovieDataService, MoviesInfoService>();
builder.Services.AddScoped<ITrailerService, YoutubeTrailerService>();

builder.Services.AddHttpClient<IMovieDataService, MoviesInfoService>(client =>
{
    client.BaseAddress = new Uri("https://moviesminidatabase.p.rapidapi.com/");
    client.DefaultRequestHeaders.Add("X-RapidAPI-Key", builder.Configuration["RapidAPI:Key"]);
    client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "moviesminidatabase.p.rapidapi.com");
});

builder.Services.AddHttpClient<ITrailerService, YoutubeTrailerService>(client =>
{
    client.BaseAddress = new Uri("https://www.googleapis.com/youtube/v3/");
});

builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactAppOrigin",
        corsBuilder => corsBuilder.WithOrigins("https://localhost:44444")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowReactAppOrigin");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();