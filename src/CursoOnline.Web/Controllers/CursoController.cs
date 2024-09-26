using System.Diagnostics;
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

    public CursoController(ArmazenadorDeCurso armazenadorDeCurso)
    {
        _armazenadorDeCurso = armazenadorDeCurso;
    }

    [HttpPost]
    public IActionResult Salvar(CursoDto model)
    {
        _armazenadorDeCurso.Armazenar(model);
        return Ok();
    }
}
