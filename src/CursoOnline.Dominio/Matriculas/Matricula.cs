using CursoOnline.Dominio._Base;
using CursoOnline.Dominio.Alunos;
using CursoOnline.Dominio.Cursos;

namespace CursoOnline.Dominio.Matriculas;

public class Matricula
{
    public Aluno Aluno { get; private set; }
    public Curso Curso { get; private set; }
    public decimal ValorPago { get; private set; }
    public bool TemDesconto { get; private set; }
    public double NotaDoAluno { get; set; }

    public Matricula(Aluno aluno, Curso curso, decimal valorPago)
    {
        ValidadorDeRegra.Novo()
            .Quando(aluno == null!, Resource.AlunoInvalido)
            .Quando(curso == null!, Resource.CursoInvalido)
            .Quando(valorPago < 1, Resource.ValorInvalido)
            .Quando(curso != null && valorPago > curso.Valor,
                Resource.ValorPagoMaiorQueValorCurso)
            .Quando(aluno != null && curso != null && aluno.PublicoAlvo != curso.PublicoAlvo, Resource.PublicosAlvoDiferentes)
            .DispararExcecaoSeExistir();

        Aluno = aluno;
        Curso = curso;
        ValorPago = valorPago;
        TemDesconto = valorPago < Curso.Valor;
    }

    public void InformarNota(double notaDoAluno)
    {
        ValidadorDeRegra.Novo()
            .Quando(notaDoAluno < 0 || notaDoAluno > 10, Resource.NotaDoAlunoInvalida)
            .DispararExcecaoSeExistir();

        NotaDoAluno = notaDoAluno;
    }
}
