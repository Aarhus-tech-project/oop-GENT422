using Microsoft.EntityFrameworkCore;
using ReservationSystem.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Server=(LocalDB)\\mssqllocaldb;Database=ReservationDb;Trusted_Connection=true;";

builder.Services.AddDbContext<ReservationDbContext>(options =>
    options.UseSqlServer(connectionString)
);

builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddScoped<IVenueService, VenueService>();
builder.Services.AddScoped<IReservationService, ReservationService>();

builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}


app.UseStaticFiles();

app.UseCors("AllowAll");
app.MapControllers();

app.MapFallbackToFile("index.html");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ReservationDbContext>();
    context.Database.EnsureCreated();
    
    if (!context.Venues.Any())
    {
        var venueService = scope.ServiceProvider.GetRequiredService<IVenueService>();
           
        await venueService.CreateVenue("The Fork Restaurant", "Restaurant", 50);
        await venueService.CreateVenue("Aarhus Cinema", "Cinema", 200);
        await venueService.CreateVenue("SAS Flight SK1234", "Airplane", 180);
        
        Console.WriteLine(" Database seeded with example data");
    }
}

app.Run();
