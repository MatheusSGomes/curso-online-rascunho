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
    private readonly MatriculaDto _matriculaDto;

    public CriacaoDaMatriculaTest()
    {
        _cursoRepositorio = new Mock<ICursoRepositorio>();
        _alunoRepositorio = new Mock<IAlunoRepository>();

        var aluno = AlunoBuilder.Novo().ComId(23).ComPublicoAlvo(PublicoAlvo.Universitario).Build();
        var curso = CursoBuilder.Novo().ComId(1).ComPublicoAlvo(PublicoAlvo.Universitario).Build();

        _alunoRepositorio.Setup(repo => repo.ObterPorId(aluno.Id)).Returns(aluno);
        _cursoRepositorio.Setup(repo => repo.ObterPorId(curso.Id)).Returns(curso);

        _matriculaDto = new MatriculaDto { AlunoId = aluno.Id, CursoId = curso.Id };

        // Instancia a Domain Service
        _criacaoDaMatricula = new CriacaoDaMatricula(_alunoRepositorio.Object, _cursoRepositorio.Object);
    }

    [Fact]
    // Curso deve existir no banco de dados, caso não, retorna uma exceção com "curso não encontrado"
    public void DeveNotificarQuandoCursoNaoForEncontrado()
    {
        Curso cursoInvalido = null!;

        _cursoRepositorio.Setup(repo => repo.ObterPorId(It.IsAny<int>())).Returns(cursoInvalido);

        Assert.Throws<ExcecaoDeDominio>(() =>
                _criacaoDaMatricula.Criar(_matriculaDto))
            .ComMensagem(Resource.CursoNaoEncontrado);
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
            .Quando(curso == null, Resource.CursoNaoEncontrado)
            .DispararExcecaoSeExistir();
    }
}

public class MatriculaDto
{
    public int AlunoId { get; set; }
    public int CursoId { get; set; }
}
