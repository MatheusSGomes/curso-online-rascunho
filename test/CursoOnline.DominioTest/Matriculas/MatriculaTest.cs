using CursoOnline.Dominio._Base;
using CursoOnline.Dominio.Alunos;
using CursoOnline.Dominio.Cursos;
using CursoOnline.DominioTest._Builders;
using CursoOnline.DominioTest._Utils;
using ExpectedObjects;

namespace CursoOnline.DominioTest.Matriculas;

public class MatriculaTest
{
    [Fact]
    public void DeveCriarMatricula()
    {
        var matriculaEsperada = new
        {
            Aluno = AlunoBuilder.Novo().Build(),
            Curso = CursoBuilder.Novo().Build(),
            ValorPago = 1000m
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
        Curso cursoInvalido = null;

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
}

public class Matricula
{
    public Aluno Aluno { get; private set; }
    public Curso Curso { get; private set; }
    public decimal ValorPago { get; private set; }

    public Matricula(Aluno aluno, Curso curso, decimal valorPago)
    {
        ValidadorDeRegra.Novo()
            .Quando(aluno == null, Resource.AlunoInvalido)
            .Quando(curso == null, Resource.CursoInvalido)
            .Quando(valorPago < 1, Resource.ValorInvalido)
            .DispararExcecaoSeExistir();

        Aluno = aluno;
        Curso = curso;
        ValorPago = valorPago;
    }
}
