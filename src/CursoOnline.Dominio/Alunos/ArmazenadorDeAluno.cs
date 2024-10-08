using CursoOnline.Dominio._Base;
using CursoOnline.Dominio.Cursos;

namespace CursoOnline.Dominio.Alunos;

public class ArmazenadorDeAluno
{
    private readonly IAlunoRepository _alunoRepository;

    public ArmazenadorDeAluno(IAlunoRepository alunoRepository)
    {
        _alunoRepository = alunoRepository;
    }

    public void Armazenar(AlunoDto alunoDto)
    {
        var alunoMesmoCpf = _alunoRepository.ObterPorCpf(alunoDto.Cpf);

        var validaSeAlunoJaExiste = alunoMesmoCpf != null && alunoMesmoCpf.Id != alunoDto.Id;

        ValidadorDeRegra.Novo()
            .Quando(validaSeAlunoJaExiste, Resource.CpfJaCadastrado)
            .DispararExcecaoSeExistir();

        if (alunoDto.Id == 0)
        {
            var aluno = new Aluno(alunoDto.Nome, alunoDto.Cpf, alunoDto.Email, PublicoAlvo.Estudante);
            _alunoRepository.Adicionar(aluno);
        }
        else
        {
            var aluno = _alunoRepository.ObterPorId(alunoDto.Id);
            aluno.AlterarNome(alunoDto.Nome);
        }
    }
}
