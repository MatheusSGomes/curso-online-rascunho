using CursoOnline.Dominio._Base;
using CursoOnline.Dominio.Alunos;
using CursoOnline.Dominio.Cursos;

namespace CursoOnline.Dominio.Matriculas;

// Classe corresponde ao Domain Service (Serviço de domínio)
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
