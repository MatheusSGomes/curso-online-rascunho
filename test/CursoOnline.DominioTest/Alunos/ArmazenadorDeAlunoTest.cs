using Bogus;
using Bogus.Extensions.Brazil;
using CursoOnline.Dominio._Base;
using CursoOnline.Dominio.Alunos;
using CursoOnline.Dominio.Cursos;
using CursoOnline.DominioTest._Builders;
using CursoOnline.DominioTest._Utils;
using Moq;

namespace CursoOnline.DominioTest.Alunos;

public class ArmazenadorDeAlunoTest
{
    private readonly Faker _faker;
    private readonly AlunoDto _alunoDto;
    private readonly Mock<IAlunoRepository> _alunoRepository;
    private readonly ArmazenadorDeAluno _armazenadorDeAluno;

    public ArmazenadorDeAlunoTest()
    {
        _faker = new Faker();

        _alunoDto = new AlunoDto
        {
            Nome = _faker.Person.FullName,
            Email = _faker.Person.Email,
            Cpf = _faker.Person.Cpf(),
            PublicoAlvo = PublicoAlvo.Estudante.ToString()
        };

        _alunoRepository = new Mock<IAlunoRepository>();
        _armazenadorDeAluno = new ArmazenadorDeAluno(_alunoRepository.Object);
    }

    [Fact]
    public void DeveAdicionarAluno()
    {
        var alunoDto = new AlunoDto
        {
            Nome = "Matheus",
            Cpf = "987.654.321-12",
            Email = "matheus@gmail.com",
            PublicoAlvo = PublicoAlvo.Estudante.ToString()
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

        // Faço o mock, como se 'aluno' já estivesse cadastrado
        alunoRepositoryMock.Setup(alunoRepository => alunoRepository.ObterPorCpf("987.654.321-12"))
            .Returns(aluno);

        // Ao tentar cadastrar novamente é lançada a exception
        Assert.Throws<ExcecaoDeDominio>(() =>
                armazenadorDeAluno.Armazenar(alunoDto))
            .ComMensagem(Resource.CpfInvalido);
    }

    [Fact]
    public void DeveAlterarNomeAluno()
    {
        var alunoDto = new AlunoDto
        {
            Id = 123,
            Nome = "Matheus",
            Cpf = "987.654.321-12",
            Email = "matheus@gmail.com",
            PublicoAlvo = PublicoAlvo.Estudante.ToString()
        };

        var alunoSalvo = AlunoBuilder.Novo().Build();

        var alunoRepositoryMock = new Mock<IAlunoRepository>();
        var armazenadorDeAluno = new ArmazenadorDeAluno(alunoRepositoryMock.Object);

        alunoRepositoryMock.Setup(cursoRepository => cursoRepository.ObterPorId(alunoDto.Id))
            .Returns(alunoSalvo);

        armazenadorDeAluno.Armazenar(alunoDto);

        Assert.Equal(alunoDto.Nome, alunoSalvo.Nome);
    }
}

public interface IAlunoRepository : IRepositorio<Aluno>
{
    Aluno ObterPorCpf(string cpf);
}

public class AlunoDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public string Email { get; set; }
    public string PublicoAlvo { get; set; }
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

        if (alunoDto.Id > 0)
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
