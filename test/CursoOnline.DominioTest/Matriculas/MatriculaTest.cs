using CursoOnline.Dominio.Alunos;
using CursoOnline.Dominio.Cursos;
using CursoOnline.DominioTest._Builders;
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
}

public class Matricula
{
    public Aluno Aluno { get; set; }
    public Curso Curso { get; set; }
    public decimal ValorPago { get; set; }

    public Matricula(Aluno aluno, Curso curso, decimal valorPago)
    {
        Aluno = aluno;
        Curso = curso;
        ValorPago = valorPago;
    }
}
