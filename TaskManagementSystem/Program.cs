

using TaskManagementSystem.Extension;
using TaskManagementSystem.MiddelWare;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddJsonSerilize();
builder.Services.AddDbConntextExtention(builder.Configuration);
builder.Services.AddService();
builder.Services.AddCustomerRatelimiting();
builder.Services.AddJWT(builder.Configuration);
builder.Services.AddRedishCache(builder.Configuration);
builder.Services.AutoMapExtention();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseRateLimiter();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();

app.Run();
