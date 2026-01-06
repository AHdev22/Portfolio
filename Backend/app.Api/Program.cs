using app.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. Configuration & Security
// ==========================================


builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // 👈 لازم تحدد رابط الفرونت بالظبط (بدون / في الآخر)
                  .AllowAnyHeader()
                  .AllowAnyMethod();
            // .AllowCredentials() is not needed for JWT Bearer token auth, which is cleaner.
        });
});


var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.MapControllers();

app.Run();