

using TaskManagementSystem.Extension;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddJsonSerilize();
builder.Services.AddDbConntextExtention(builder.Configuration);
builder.Services.AddService();
builder.Services.AddCustomerRatelimiting();
builder.Services.AddJWT(builder.Configuration);
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "MyApiCache:";
});

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

app.MapControllers();

app.Run();
