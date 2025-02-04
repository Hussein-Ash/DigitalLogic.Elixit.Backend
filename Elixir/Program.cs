using System.Globalization;
using Amazon.S3;
using Elixir.Extensions;
using Elixir.Helpers;
using Elixir.Services.StaticService.Storage;
using FFMpegCore;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog;

using ConfigurationProvider = Elixir.Helpers.ConfigurationProvider;

var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Error()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithExposedHeaders("status"));
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSingleton<IStorageService, MinioStorageService>();
}
else
{
    builder.Services.AddSingleton<IAmazonS3, AmazonS3Client>();
    builder.Services.AddSingleton<IStorageService, S3StorageService>();
}


string appBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;

string ffmpegPath = Path.Combine(appBaseDirectory, "ffmpeg");

GlobalFFOptions.Configure(options => options.BinaryFolder = ffmpegPath);


// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
        options.SerializerSettings.Converters.Add(new IsoDateTimeConverter
        { DateTimeStyles = DateTimeStyles.AssumeUniversal });
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });
;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(options => { options.OperationFilter<PascalCaseQueryParameterFilter>(); });
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
IConfiguration configuration = builder.Configuration;
ConfigurationProvider.Configuration = configuration;
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

var app = builder.Build();


// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.UseMiddleware<CustomUnauthorizedMiddleware>();
app.UseMiddleware<CustomPayloadTooLargeMiddleware>();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.MapControllers();

app.Run();