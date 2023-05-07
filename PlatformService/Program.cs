using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt =>
    {
        if (builder.Environment.IsProduction())
        {
            Console.WriteLine("--> Using SqlServer Db");
            opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn"));
        }
        else
        {
            Console.WriteLine("--> Using InMem Db");
            opt.UseInMemoryDatabase("InMem");   
        }
    })
    .AddScoped<IPlatformRepo, PlatformRepo>()
    .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
    .AddSingleton<IMessageBusClient, MessageBusClient>()
    .AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Console.WriteLine($"--> CommandService endpoint {builder.Configuration["CommandService"]}");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

PrepDb.PrepPopulation(app, builder.Environment.IsProduction());

app.Run();
