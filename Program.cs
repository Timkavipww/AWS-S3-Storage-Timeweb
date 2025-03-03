var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder
    .AddEnvVariableSupport()
    .AddAwsSupport();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.MapControllers();
app.UseRouting();
    

app.Run();

 