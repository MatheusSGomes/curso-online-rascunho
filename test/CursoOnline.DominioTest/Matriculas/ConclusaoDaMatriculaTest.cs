using CursoOnline.Dominio._Base;
using CursoOnline.Dominio.Matriculas;
using CursoOnline.DominioTest._Builders;
using CursoOnline.DominioTest._Utils;
using Moq;

namespace CursoOnline.DominioTest.Matriculas;

public class ConclusaoDaMatriculaTest
{
    [Fact]
    public void DeveInformarNotaDoAluno()
    {
        // O que um Domain Service precisa? Repository com as consultas.
        var matriculaRepositorio = new Mock<IMatriculaRepositorio>();

        // O que uma repository retorna? Uma matricula (Domain)
        var matricula = MatriculaBuilder.Novo().Build(); // O objeto matricula indica que a matricula deu certo.
        matriculaRepositorio.Setup(r => r.ObterPorId(matricula.Id)).Returns(matricula);

        // Fazer "Setup" do Domain Service
        // Ele vai receber via "injeção de dependência" a repository
        var conclusaoMatricula = new ConclusaoDaMatricula(matriculaRepositorio.Object);

        // Chamar o método que vai executar o objetivo
        var notaDoAlunoEsperada = 8;
        conclusaoMatricula.Concluir(matricula.Id, notaDoAlunoEsperada);

        // Verificar se a matricula teve uma nota
        Assert.Equal(notaDoAlunoEsperada, matricula.NotaDoAluno);
    }

    [Fact]
    public void DeveNotificarQuandoMatriculaNaoEncontrada()
    {
        var notaDoAluno = 8;
        var matriculaRepositorio = new Mock<IMatriculaRepositorio>();
        var conclusaoMatricula = new ConclusaoDaMatricula(matriculaRepositorio.Object);

        Matricula matriculaInvalida = null;
        const int matriculaIdInvalida = 1;

        matriculaRepositorio.Setup(r => r.ObterPorId(It.IsAny<int>())).Returns(matriculaInvalida);

        Assert.Throws<ExcecaoDeDominio>(() =>
                conclusaoMatricula.Concluir(matriculaIdInvalida, notaDoAluno))
            .ComMensagem(Resource.MatriculaNaoEncontrada);

    }
}

public class ConclusaoDaMatricula
{
    private readonly IMatriculaRepositorio _matriculaRepositorio;

    public ConclusaoDaMatricula(IMatriculaRepositorio matriculaRepositorio)
    {
        _matriculaRepositorio = matriculaRepositorio;
    }

    public void Concluir(int matriculaId, double notaDoAluno)
    {
        var matricula = _matriculaRepositorio.ObterPorId(matriculaId);

        ValidadorDeRegra.Novo()
            .Quando(matricula == null, Resource.MatriculaNaoEncontrada)
            .DispararExcecaoSeExistir();

        matricula.InformarNota(notaDoAluno);
    }
}
