using Bogus;
using CursoOnline.Dominio._Base;
using CursoOnline.Dominio.Alunos;
using CursoOnline.Dominio.Cursos;
using CursoOnline.Dominio.Matriculas;
using CursoOnline.DominioTest._Builders;
using CursoOnline.DominioTest._Utils;
using ExpectedObjects;

namespace CursoOnline.DominioTest.Matriculas;

public class MatriculaTest
{
    [Fact]
    public void DeveCriarMatricula()
    {
        var faker = new Faker();

        var matriculaEsperada = new
        {
            Aluno = AlunoBuilder.Novo().Build(),
            Curso = CursoBuilder.Novo().Build(),
            ValorPago = faker.Finance.Amount()
        };

        var matricula = new Matricula(matriculaEsperada.Aluno, matriculaEsperada.Curso, matriculaEsperada.ValorPago);

        matriculaEsperada.ToExpectedObject().ShouldMatch(matricula);
    }

    [Fact]
    public void NaoDeveCriarMatriculaSemAluno()
    {
        // O aluno inválido será medido com "null"
        Aluno alunoInvalido = null;

        Assert.Throws<ExcecaoDeDominio>(() =>
                MatriculaBuilder.Novo().ComAluno(alunoInvalido).Build())
            .ComMensagem(Resource.AlunoInvalido);
    }

    [Fact]
    public void NaoDeveCriarMatriculaSemCurso()
    {
        Curso cursoInvalido = null!;

        Assert.Throws<ExcecaoDeDominio>(() =>
                MatriculaBuilder.Novo().ComCurso(cursoInvalido).Build())
            .ComMensagem(Resource.CursoInvalido);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-1000)]
    public void NaoDeveCriarMatriculaComValorPagoInvalido(decimal valorPagoInvalido)
    {
        Assert.Throws<ExcecaoDeDominio>(() =>
                MatriculaBuilder.Novo().ComValorPago(valorPagoInvalido).Build())
            .ComMensagem(Resource.ValorInvalido);
    }

    [Fact]
    public void NaoDeveCriarMatriculaComValorPagoMaiorQueValorDoCurso()
    {
        var curso = CursoBuilder.Novo().ComValor(1000).Build();
        decimal valorPagoMaiorQueCurso = curso.Valor + 1;

        Assert.Throws<ExcecaoDeDominio>(() =>
                MatriculaBuilder.Novo().ComCurso(curso).ComValorPago(valorPagoMaiorQueCurso).Build())
            .ComMensagem(Resource.ValorPagoMaiorQueValorCurso);
    }

    [Fact]
    public void DeveIndicarQueHouveDescontoNaMatricula()
    {
        var curso = CursoBuilder.Novo().ComValor(1000).Build();
        var valorPagoComDesconto = curso.Valor - 100;

        var matricula = MatriculaBuilder.Novo().ComCurso(curso).ComValorPago(valorPagoComDesconto).Build();

        Assert.True(matricula.TemDesconto);
    }
}
