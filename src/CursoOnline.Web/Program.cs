using CursoOnline.Dominio._Base;
using CursoOnline.Ioc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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
    unitOfWork.Commit();
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
