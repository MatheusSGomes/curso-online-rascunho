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
}

public class Matricula
{
    public Aluno Aluno { get; set; }
    public Curso Curso { get; set; }
    public decimal ValorPago { get; set; }

    public Matricula(Aluno aluno, Curso curso, decimal valorPago)
    {
        ValidadorDeRegra.Novo()
            .Quando(aluno == null, Resource.AlunoInvalido)
            .DispararExcecaoSeExistir();

        Aluno = aluno;
        Curso = curso;
        ValorPago = valorPago;
    }
}
