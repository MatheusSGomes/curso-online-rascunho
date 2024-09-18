﻿using CursoOnline.Dados.Contextos;
using CursoOnline.Dados.Repositorios;
using CursoOnline.Dominio._Base;
using CursoOnline.Dominio.Cursos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CursoOnline.Ioc;

public static class StartupIoc
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => 
            options.UseSqlServer("ConnectionString"));

        services.AddScoped(typeof(IRepositorio<>), typeof(RepositorioBase<>));

        services.AddScoped(typeof(ICursoRepositorio), typeof(CursoRepositorio));

        services.AddScoped<ArmazenadorDeCurso>();
    }
}
