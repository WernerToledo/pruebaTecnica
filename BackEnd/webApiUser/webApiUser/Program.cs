using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using webApiUser.AuthenticationService;
using webApiUser.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Configuraci�n de servicios para la aplicaci�n
builder.Services.AddControllers(); // A�ade servicios necesarios para los controladores de API

builder.Services.AddEndpointsApiExplorer(); // A�ade servicios necesarios para explorar los endpoints de la API
builder.Services.AddSwaggerGen(); // A�ade servicios para generar la documentaci�n Swagger

builder.Services.AddSingleton<usuarioService>(); // Registra usuarioService como un singleton (una sola instancia)

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]); // Obtiene la clave de JWT desde la configuraci�n
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true, // Valida el emisor del token
        ValidateAudience = true, // Valida el receptor del token
        ValidateLifetime = true, // Valida la fecha de expiraci�n del token
        ValidateIssuerSigningKey = true, // Valida la clave de firma del token
        ValidIssuer = builder.Configuration["Jwt:Issuer"], // Emisor esperado del token
        ValidAudience = builder.Configuration["Jwt:Audience"], // Receptor esperado del token
        IssuerSigningKey = new SymmetricSecurityKey(key) // Clave de firma del token
    };
});

// Configuraci�n de autorizaci�n
builder.Services.AddAuthorization(); // A�ade servicios necesarios para la autorizaci�n

builder.Services.AddScoped<AuthService>(); // Registra AuthService con un tiempo de vida Scoped (una instancia por solicitud HTTP)


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
