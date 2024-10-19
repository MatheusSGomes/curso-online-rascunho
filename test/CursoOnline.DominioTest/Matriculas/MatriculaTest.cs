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
            ValorDaMatricula = 1000m
        };

        var matricula = new Matricula(matriculaEsperada.Aluno, matriculaEsperada.Curso, matriculaEsperada.ValorDaMatricula);

        matriculaEsperada.ToExpectedObject().ShouldMatch(matricula);
    }
}

public class Matricula
{
    public Aluno Aluno { get; set; }
    public Curso Curso { get; set; }
    public decimal ValorDaMatricula { get; set; }

    public Matricula(Aluno aluno, Curso curso, decimal valorDaMatricula)
    {
        Aluno = aluno;
        Curso = curso;
        ValorDaMatricula = valorDaMatricula;
    }
}
