using Asp.Versioning;
using ColomBEE_CSharp_NoSQL.API.DbContexts;
using ColomBEE_CSharp_NoSQL.API.Interfaces;
using ColomBEE_CSharp_NoSQL.API.Repositories;
using ColomBEE_CSharp_NoSQL.API.Services;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

//Aqui agregamos los servicios requeridos

// ***************************************************************************
// --- Configuración del DB Context --
// ***************************************************************************

builder.Services.AddSingleton<MongoDbContext>();

// ***************************************************************************
// --- Configuración de los repositorios --
// ***************************************************************************

builder.Services.AddScoped<IEstadisticaRepository, EstadisticaRepository>();

// ***************************************************************************
// --- Configuraci�n de los servicios asociados  --
// ***************************************************************************

builder.Services.AddScoped<EstadisticaService>();

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

// ***************************************************************************
// --- Configuración del versionamiento para el API  --
// ***************************************************************************

builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new HeaderApiVersionReader("api-version"),
                new QueryStringApiVersionReader("api-version")
            );
        }
    )
    .AddMvc()
    .AddApiExplorer(setup =>
    {
        setup.GroupNameFormat = "'v'VVV";
        setup.SubstituteApiVersionInUrl = true;
    });

// ***************************************************************************
// --- Configuración del Swagger/OpenAPI  --
// ***************************************************************************

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ColomBEE-EBM.API v1 - MongoDB"
    });

    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = "ColomBEE-EBM.API v2 - MongoDB"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(config =>
        {
            // Configuración manual de cada endpoint
            config.SwaggerEndpoint("/swagger/v1/swagger.json", "ColomBEE-EBM.API v1");
            config.SwaggerEndpoint("/swagger/v2/swagger.json", "ColomBEE-EBM.API v2");
        }
    );
}

//Modificamos el encabezado de las peticiones para ocultar el web server utilizado
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Server", "ColomBEEServer");
    await next();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();