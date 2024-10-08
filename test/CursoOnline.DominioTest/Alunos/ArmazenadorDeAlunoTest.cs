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
        _armazenadorDeAluno.Armazenar(_alunoDto);

        /*
         * Verifico se o método 'Adicionar' foi chamado.
         * Verifico se foi passado algum parâmetro do tipo 'Aluno' para ele.
         * Verifico se no parâmetro passado tem a propriedade Nome e ela é igual a Nome da DTO (posso testar outras propriedades também).
         * Verifico se o método 'Adicionar' foi chamado apenas 1 vez.
         */
        _alunoRepository.Verify(
            alunoRepository => alunoRepository.Adicionar(It.Is<Aluno>(a => a.Nome == _alunoDto.Nome)),
            Times.Once());
    }

    [Fact]
    public void NaoDeveAdicionarAlunoQuandoCpfJaCadastrado()
    {
        var alunoComMesmoCpf = AlunoBuilder.Novo().ComId(321).Build();

        // Faço o mock, como se 'aluno' já estivesse cadastrado
        _alunoRepository.Setup(alunoRepository => alunoRepository.ObterPorCpf(_alunoDto.Cpf))
            .Returns(alunoComMesmoCpf);

        // Ao tentar cadastrar novamente é lançada a exception
        Assert.Throws<ExcecaoDeDominio>(() =>
                _armazenadorDeAluno.Armazenar(_alunoDto))
            .ComMensagem(Resource.CpfJaCadastrado);
    }

    [Fact]
    public void NaoDeveCriarPublicoAlvoInvalido()
    {
        const string PUBLICO_ALVO_INVALIDO = "Invalido";

        _alunoDto.PublicoAlvo = PUBLICO_ALVO_INVALIDO;

        Assert.Throws<ExcecaoDeDominio>(() => _armazenadorDeAluno.Armazenar(_alunoDto))
            .ComMensagem(Resource.PublicoAlvoInvalido);
    }

    [Fact]
    public void DeveEditarAluno()
    {
        _alunoDto.Id = 35;
        _alunoDto.Nome = _faker.Person.FullName;

        var alunoSalvo = AlunoBuilder.Novo().Build();

        _alunoRepository.Setup(cursoRepository => cursoRepository.ObterPorId(_alunoDto.Id))
            .Returns(alunoSalvo);

        _armazenadorDeAluno.Armazenar(_alunoDto);

        Assert.Equal(_alunoDto.Nome, alunoSalvo.Nome);
    }

    [Fact]
    public void NaoDeveAdicionarQuandoForEdicao()
    {
        _alunoDto.Id = 35;
        var alunoJaSalvo = AlunoBuilder.Novo().Build();

        _alunoRepository.Setup(alunoRepository => alunoRepository.ObterPorId(_alunoDto.Id)).Returns(alunoJaSalvo);

        _armazenadorDeAluno.Armazenar(_alunoDto);

        _alunoRepository.Verify(alunoRepository => alunoRepository.Adicionar(It.IsAny<Aluno>()), Times.Never());
    }
}
