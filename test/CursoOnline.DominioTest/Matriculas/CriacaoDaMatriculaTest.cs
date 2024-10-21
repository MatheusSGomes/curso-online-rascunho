using CursoOnline.Dominio._Base;
using CursoOnline.Dominio.Alunos;
using CursoOnline.Dominio.Cursos;
using CursoOnline.Dominio.Matriculas;
using CursoOnline.DominioTest._Builders;
using CursoOnline.DominioTest._Utils;
using Moq;

namespace CursoOnline.DominioTest.Matriculas;

public class CriacaoDaMatriculaTest
{
    private readonly Mock<ICursoRepositorio> _cursoRepositorio;
    private readonly Mock<IAlunoRepository> _alunoRepositorio;
    private readonly Mock<IMatriculaRepositorio> _matriculaRepositorio;

    private readonly CriacaoDaMatricula _criacaoDaMatricula;
    private readonly MatriculaDto _matriculaDto;

    private readonly Aluno _aluno;
    private readonly Curso _curso;

    public CriacaoDaMatriculaTest()
    {
        _cursoRepositorio = new Mock<ICursoRepositorio>();
        _alunoRepositorio = new Mock<IAlunoRepository>();
        _matriculaRepositorio = new Mock<IMatriculaRepositorio>();

        _aluno = AlunoBuilder.Novo().ComId(23).ComPublicoAlvo(PublicoAlvo.Universitario).Build();
        _curso = CursoBuilder.Novo().ComId(1).ComPublicoAlvo(PublicoAlvo.Universitario).Build();

        _alunoRepositorio.Setup(repo => repo.ObterPorId(_aluno.Id)).Returns(_aluno);
        _cursoRepositorio.Setup(repo => repo.ObterPorId(_curso.Id)).Returns(_curso);

        _matriculaDto = new MatriculaDto { AlunoId = _aluno.Id, CursoId = _curso.Id, ValorPago = _curso.Valor };

        // Instancia a Domain Service
        _criacaoDaMatricula = new CriacaoDaMatricula(_alunoRepositorio.Object, _cursoRepositorio.Object, _matriculaRepositorio.Object);
    }

    [Fact]
    // Curso deve existir no banco de dados, caso não, retorna uma exceção com "curso não encontrado"
    public void DeveNotificarQuandoCursoNaoForEncontrado()
    {
        Curso cursoInvalido = null!;

        _cursoRepositorio.Setup(repo => repo.ObterPorId(_matriculaDto.CursoId)).Returns(cursoInvalido);

        Assert.Throws<ExcecaoDeDominio>(() =>
                _criacaoDaMatricula.Criar(_matriculaDto))
            .ComMensagem(Resource.CursoNaoEncontrado);
    }

    [Fact]
    // Caso aluno não exista no banco, deve retornar uma exceção
    public void DeveNotificarQuandoAlunoNaoForEncontrado()
    {
        Aluno alunoInvalido = null!;

        _alunoRepositorio.Setup(repo => repo.ObterPorId(_matriculaDto.AlunoId)).Returns(alunoInvalido);

        Assert.Throws<ExcecaoDeDominio>(() =>
                _criacaoDaMatricula.Criar(_matriculaDto))
            .ComMensagem(Resource.AlunoNaoEncontrado);
    }

    [Fact]
    public void DeveAdicionarMatricula()
    {
        _criacaoDaMatricula.Criar(_matriculaDto);

        // Espero que a matricula tenha o mesmo aluno e o mesmo curso configurado no construtor dos testes
        _matriculaRepositorio.Verify(r => r.Adicionar(It.Is<Matricula>(m => m.Aluno == _aluno && m.Curso == _curso)));
    }
}

// Classe corresponde ao Domain Service (Serviço de domínio)

public interface IMatriculaRepositorio : IRepositorio<Matricula>
{

}

public class CriacaoDaMatricula
{
    private readonly IAlunoRepository _alunoRepositorio;
    private readonly ICursoRepositorio _cursoRepositorio;
    private readonly IMatriculaRepositorio _matriculaRepositorio;

    public CriacaoDaMatricula(
        IAlunoRepository alunoRepositorio,
        ICursoRepositorio cursoRepositorio,
        IMatriculaRepositorio matriculaRepositorio)
    {
        _alunoRepositorio = alunoRepositorio;
        _cursoRepositorio = cursoRepositorio;
        _matriculaRepositorio = matriculaRepositorio;
    }

    public void Criar(MatriculaDto matriculaDto)
    {
        var aluno = _alunoRepositorio.ObterPorId(matriculaDto.AlunoId);
        var curso = _cursoRepositorio.ObterPorId(matriculaDto.CursoId);

        ValidadorDeRegra.Novo()
            .Quando(curso == null, Resource.CursoNaoEncontrado)
            .Quando(aluno == null, Resource.AlunoNaoEncontrado)
            .DispararExcecaoSeExistir();

        var matricula = new Matricula(aluno, curso, matriculaDto.ValorPago);

        _matriculaRepositorio.Adicionar(matricula);
    }
}

public class MatriculaDto
{
    public int AlunoId { get; set; }
    public int CursoId { get; set; }
    public decimal ValorPago { get; set; }
}
