using CursoOnline.Dominio.Alunos;
using CursoOnline.Dominio.Cursos;
using CursoOnline.DominioTest._Builders;

namespace CursoOnline.DominioTest.Matriculas;

public class MatriculasTest
{
    [Fact]
    public void DeveCriarMatricula()
    {
        var aluno = AlunoBuilder.Novo().Build();
        var curso = CursoBuilder.Novo().Build();
        const decimal valorDaMatricula = 1000m;

        var matricula = new Matricula(aluno, curso, valorDaMatricula);

        Assert.Equal(aluno, matricula.Aluno);
        Assert.Equal(curso, matricula.Curso);
        Assert.Equal(valorDaMatricula, matricula.ValorDaMatricula);
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
