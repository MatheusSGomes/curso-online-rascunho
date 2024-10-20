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
    [Fact]
    public void NaoDevePublicoAlvoDeAlunoECursoSeremDiferentes()
    {
        var cursoRepositorio = new Mock<ICursoRepositorio>();
        var alunoRepositorio = new Mock<IAlunoRepository>();

        var curso = CursoBuilder.Novo().ComPublicoAlvo(PublicoAlvo.Empregado).Build();
        var aluno = AlunoBuilder.Novo().ComPublicoAlvo(PublicoAlvo.Estudante).Build();

        // Sempre que buscar pelo Id no repositório, deve retornar o curso criado.
        cursoRepositorio.Setup(repo => repo.ObterPorId(curso.Id)).Returns(curso);
        alunoRepositorio.Setup(repo => repo.ObterPorId(aluno.Id)).Returns(aluno);

        var matriculaDto = new MatriculaDto
        {
            AlunoId = aluno.Id,
            CursoId = curso.Id
        };

        // Instancia a Domain Service
        var criacaoDaMatricula = new CriacaoDaMatricula(alunoRepositorio.Object, cursoRepositorio.Object);

        // Quando criar a matricula deve retornar um erro
        // Os 2 públicos alvo não podem ser iguais
        // criacaoDaMatricula.Criar(matriculaDto);
        Assert.Throws<ExcecaoDeDominio>(() =>
                criacaoDaMatricula.Criar(matriculaDto))
            .ComMensagem(Resource.PublicosAlvoDiferentes);
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
