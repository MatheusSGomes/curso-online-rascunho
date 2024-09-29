using System.Diagnostics;
using CursoOnline.Dominio._Base;
using CursoOnline.Dominio.Cursos;
using Microsoft.AspNetCore.Mvc;
using CursoOnline.Web.Models;
using CursoOnline.Web.Utils;

namespace CursoOnline.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class CursoController : Controller
{
    private readonly ArmazenadorDeCurso _armazenadorDeCurso;
    private readonly IRepositorio<Curso> _cursoRepositorio;

    public CursoController(ArmazenadorDeCurso armazenadorDeCurso, IRepositorio<Curso> cursoRepositorio)
    {
        _armazenadorDeCurso = armazenadorDeCurso;
        _cursoRepositorio = cursoRepositorio;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var cursos = _cursoRepositorio.Consultar();

        if (cursos.Any())
        {
            // Nunca renderizar o domínio, sempre fazer a conversão para DTO
            // Posso usar um adapter aqui
            var dtos = cursos.Select(c => new CursoParaListagemDto
            {
                Id = c.Id,
                Nome = c.Nome,
                CargaHoraria = c.CargaHoraria,
                PublicoAlvo = c.PublicoAlvo.ToString(),
                Valor = c.Valor
            });
            
            return Ok(dtos);
        }

        return NotFound("Nenhum curso encontrado");
    }

    [HttpPost]
    public IActionResult Salvar(CursoDto model)
    {
        _armazenadorDeCurso.Armazenar(model);
        return Ok();
    }
}
