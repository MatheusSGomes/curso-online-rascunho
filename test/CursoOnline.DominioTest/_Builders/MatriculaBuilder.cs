using CursoOnline.Dominio.Alunos;
using CursoOnline.Dominio.Cursos;
using CursoOnline.Dominio.Matriculas;
using CursoOnline.DominioTest.Matriculas;

namespace CursoOnline.DominioTest._Builders;

public class MatriculaBuilder
{
    private Aluno _aluno { get; set; }
    private Curso _curso { get; set; }
    private decimal _valorPago { get; set; }

    public static MatriculaBuilder Novo()
    {
        var aluno = AlunoBuilder.Novo().ComPublicoAlvo(PublicoAlvo.Estudante).Build();
        var curso = CursoBuilder.Novo().ComPublicoAlvo(PublicoAlvo.Estudante).Build();

        return new MatriculaBuilder
        {
            // Implemento valores default (poderiam ser fakes)
            _aluno = aluno,
            _curso = curso,
            _valorPago = curso.Valor
        };
    }

    public MatriculaBuilder ComAluno(Aluno aluno)
    {
        _aluno = aluno;
        return this;
    }

    public MatriculaBuilder ComCurso(Curso curso)
    {
        _curso = curso;
        return this;
    }

    public MatriculaBuilder ComValorPago(decimal valorPago)
    {
        _valorPago = valorPago;
        return this;
    }

    public Matricula Build()
    {
        return new Matricula(
            aluno: _aluno,
            curso: _curso,
            valorPago: _valorPago);
    }
}
