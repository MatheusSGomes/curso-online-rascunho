using CursoOnline.Dados.Contextos;
using CursoOnline.Dominio.Alunos;

namespace CursoOnline.Dados.Repositorios;

public class AlunoRepositorio : RepositorioBase<Aluno>, IAlunoRepository
{
    public AlunoRepositorio(ApplicationDbContext context) : base(context)
    { }

    public Aluno ObterPorCpf(string cpf)
    {
        var entidade = Context.Set<Aluno>().Where(a => a.Cpf.Contains(cpf));

        if (entidade.Any())
            return entidade.First();

        return null;
    }
}
