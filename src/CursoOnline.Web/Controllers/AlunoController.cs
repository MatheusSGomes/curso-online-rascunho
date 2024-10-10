using CursoOnline.Dominio._Base;
using CursoOnline.Dominio.Alunos;
using CursoOnline.Web.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CursoOnline.Web.Controllers;

public class AlunoController : Controller
{
    private readonly ArmazenadorDeAluno _armazenadorDeAluno;
    private readonly IRepositorio<Aluno> _alunoRepositorio;

    public AlunoController(ArmazenadorDeAluno armazenadorDeAluno, IRepositorio<Aluno> alunoRepositorio)
    {
        _armazenadorDeAluno = armazenadorDeAluno;
        _alunoRepositorio = alunoRepositorio;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var alunos = _alunoRepositorio.Consultar();

        if (alunos.Any())
        {
            var dtos = alunos.Select(a => new AlunosParaListagemDto
            {
                Id = a.Id,
                Nome = a.Nome,
                Cpf = a.Cpf,
                Email = a.Email
            });
            
            return Ok(dtos);
        }

        return NotFound("Nenhum aluno encontrado");
    }
}
