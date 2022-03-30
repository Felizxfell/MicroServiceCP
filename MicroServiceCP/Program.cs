using MicroServiceCP.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();

var mysqlcon = new MysqlConfiguration(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddSingleton(mysqlcon);
builder.Services.AddScoped<TablaClientePagosAnterior>();

builder.Services.AddControllers();
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

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials());

app.UseAuthorization();

app.MapControllers();

app.Run();
