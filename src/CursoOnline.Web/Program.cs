using CursoOnline.Dominio._Base;
using CursoOnline.Ioc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

StartupIoc.ConfigureServices(builder.Services, builder.Configuration); // chama o projeto para resolver as dependências

var app = builder.Build();

app.Use(async (context, next) =>
{
    // Após invocar toda a aplicação (classes de negócio, controllers e etc..)
    await next.Invoke(); 

    // Recuperor a classe UnitOfWork 
    // Lembrando que instancio IUnitOfWork, que chama UnitOfWork
    var unitOfWork = (IUnitOfWork) context.RequestServices.GetService(typeof(IUnitOfWork));
    
    // O método commit aplica o SaveChanges
    await unitOfWork.Commit();
});

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
