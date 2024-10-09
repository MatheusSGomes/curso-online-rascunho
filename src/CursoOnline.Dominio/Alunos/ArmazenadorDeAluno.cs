using CursoOnline.Dominio._Base;
using CursoOnline.Dominio.Cursos;
using CursoOnline.Dominio.PublicosAlvo;

namespace CursoOnline.Dominio.Alunos;

public class ArmazenadorDeAluno
{
    private readonly IAlunoRepository _alunoRepository;
    private readonly IConversorDePublicoAlvo _conversorDePublicoAlvo;

    public ArmazenadorDeAluno(IAlunoRepository alunoRepository, IConversorDePublicoAlvo conversorDePublicoAlvo)
    {
        _alunoRepository = alunoRepository;
        _conversorDePublicoAlvo = conversorDePublicoAlvo;
    }

    public void Armazenar(AlunoDto alunoDto)
    {
        var alunoMesmoCpf = _alunoRepository.ObterPorCpf(alunoDto.Cpf);

        var validaSeAlunoJaExiste = alunoMesmoCpf != null && alunoMesmoCpf.Id != alunoDto.Id;

        ValidadorDeRegra.Novo()
            .Quando(validaSeAlunoJaExiste, Resource.CpfJaCadastrado)
            // .Quando(!Enum.TryParse<PublicoAlvo>(alunoDto.PublicoAlvo, out var publicoAlvoConvertido), Resource.PublicoAlvoInvalido)
            .DispararExcecaoSeExistir();

        if (alunoDto.Id == 0)
        {
            var publicoAlvoConvertido = _conversorDePublicoAlvo.Converter(alunoDto.PublicoAlvo);
            var aluno = new Aluno(alunoDto.Nome, alunoDto.Cpf, alunoDto.Email, publicoAlvoConvertido);
            _alunoRepository.Adicionar(aluno);
        }
        else
        {
            var aluno = _alunoRepository.ObterPorId(alunoDto.Id);
            aluno.AlterarNome(alunoDto.Nome);
        }
    }
}
