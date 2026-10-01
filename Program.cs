using API_PTIC.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();   
    });
});

var ConnectionString = builder.Configuration.GetConnectionString("Connection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString)));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
   
    app.MapOpenApi();


    app.UseSwaggerUI(options =>
    {

        options.SwaggerEndpoint("/openapi/v1.json", "API PTIC v1");
        
        
        options.RoutePrefix = string.Empty; 
    });
}
app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
/*@para*/