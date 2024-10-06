using CursoOnline.Dominio._Base;
using CursoOnline.Dominio.Alunos;
using CursoOnline.Dominio.Cursos;
using CursoOnline.DominioTest._Builders;
using CursoOnline.DominioTest._Utils;
using Moq;

namespace CursoOnline.DominioTest.Alunos;

public class ArmazenadorDeAlunoTest
{
    [Fact]
    public void DeveAdicionarAluno()
    {
        var alunoDto = new AlunoDto
        {
            Nome = "Matheus",
            Cpf = "987.654.321-12",
            Email = "matheus@gmail.com",
            PublicoAlvo = PublicoAlvo.Estudante
        };

        var alunoRepositoryMock = new Mock<IAlunoRepository>();

        var armazenadorDeAluno = new ArmazenadorDeAluno(alunoRepositoryMock.Object);
        armazenadorDeAluno.Armazenar(alunoDto);

        /*
         * Verifico se o método 'Adicionar' foi chamado.
         * Verifico se foi passado algum parâmetro do tipo 'Aluno' para ele.
         * Verifico se no parâmetro passado tem a propriedade Nome e ela é igual a Nome da DTO (posso testar outras propriedades também).
         * Verifico se o método 'Adicionar' foi chamado apenas 1 vez.
         */
        alunoRepositoryMock.Verify(
            alunoRepository => alunoRepository.Adicionar(It.Is<Aluno>(a => a.Nome == alunoDto.Nome)),
            Times.Once());
    }

    [Fact]
    public void NaoDeveAdicionarAlunoQuandoCpfJaCadastrado()
    {
        var aluno = AlunoBuilder.Novo().ComCpf("987.654.321-12").Build();

        var alunoDto = new AlunoDto
        {
            Cpf = "987.654.321-12"
        };

        var alunoRepositoryMock = new Mock<IAlunoRepository>();
        var armazenadorDeAluno = new ArmazenadorDeAluno(alunoRepositoryMock.Object);

        alunoRepositoryMock.Setup(alunoRepository => alunoRepository.ObterPorCpf("987.654.321-12"))
            .Returns(aluno);

        Assert.Throws<ExcecaoDeDominio>(() =>
                armazenadorDeAluno.Armazenar(alunoDto))
            .ComMensagem(Resource.CpfInvalido);
    }
}

public interface IAlunoRepository : IRepositorio<Aluno>
{
    Aluno ObterPorCpf(string cpf);
}

public class AlunoDto
{
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public string Email { get; set; }
    public PublicoAlvo PublicoAlvo { get; set; }
}

public class ArmazenadorDeAluno
{
    private readonly IAlunoRepository _alunoRepository;

    public ArmazenadorDeAluno(IAlunoRepository alunoRepository)
    {
        _alunoRepository = alunoRepository;
    }

    public void Armazenar(AlunoDto alunoDto)
    {
        var alunoJaSalvo = _alunoRepository.ObterPorCpf(alunoDto.Cpf);
        var validaSeAlunoJaExiste = alunoJaSalvo != null && alunoJaSalvo.Cpf == alunoDto.Cpf;

        ValidadorDeRegra.Novo()
            .Quando(validaSeAlunoJaExiste, Resource.CpfInvalido)
            .DispararExcecaoSeExistir();

        var aluno =
            new Aluno(alunoDto.Nome, alunoDto.Cpf, alunoDto.Email, alunoDto.PublicoAlvo);

        _alunoRepository.Adicionar(aluno);
    }
}
