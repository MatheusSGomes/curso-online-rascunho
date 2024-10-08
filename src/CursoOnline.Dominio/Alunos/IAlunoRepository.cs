using CursoOnline.Dominio._Base;

namespace CursoOnline.Dominio.Alunos;

public interface IAlunoRepository : IRepositorio<Aluno>
{
    Aluno ObterPorCpf(string cpf);
}
