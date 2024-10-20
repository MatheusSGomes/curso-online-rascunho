using CursoOnline.Dominio._Base;
using CursoOnline.Dominio.Alunos;
using CursoOnline.Dominio.Cursos;
using CursoOnline.DominioTest._Builders;
using CursoOnline.DominioTest._Utils;
using Moq;

namespace CursoOnline.DominioTest.Matriculas;

// Classe corresponde ao Domain Service (Serviço de domínio)
public class CriacaoDaMatriculaTest
{
    private readonly Mock<ICursoRepositorio> _cursoRepositorio;
    private readonly Mock<IAlunoRepository> _alunoRepositorio;
    private readonly CriacaoDaMatricula _criacaoDaMatricula;

    public CriacaoDaMatriculaTest()
    {
        _cursoRepositorio = new Mock<ICursoRepositorio>();
        _alunoRepositorio = new Mock<IAlunoRepository>();

        // Instancia a Domain Service
        _criacaoDaMatricula = new CriacaoDaMatricula(_alunoRepositorio.Object, _cursoRepositorio.Object);
    }
}

public class CriacaoDaMatricula
{
    private readonly IAlunoRepository _alunoRepositorio;
    private readonly ICursoRepositorio _cursoRepositorio;

    public CriacaoDaMatricula(IAlunoRepository alunoRepositorio, ICursoRepositorio cursoRepositorio)
    {
        _alunoRepositorio = alunoRepositorio;
        _cursoRepositorio = cursoRepositorio;
    }

    public void Criar(MatriculaDto matriculaDto)
    {
        var aluno = _alunoRepositorio.ObterPorId(matriculaDto.AlunoId);
        var curso = _cursoRepositorio.ObterPorId(matriculaDto.CursoId);

        ValidadorDeRegra.Novo()
            .Quando(aluno.PublicoAlvo != curso.PublicoAlvo, Resource.PublicosAlvoDiferentes)
            .DispararExcecaoSeExistir();
    }
}

public class MatriculaDto
{
    public int AlunoId { get; set; }
    public int CursoId { get; set; }
}
