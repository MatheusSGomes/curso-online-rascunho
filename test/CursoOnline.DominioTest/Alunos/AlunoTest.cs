using Bogus;
using Bogus.Extensions.Brazil;
using CursoOnline.Dominio._Base;
using CursoOnline.Dominio.Cursos;
using CursoOnline.DominioTest._Utils;
using ExpectedObjects;

namespace CursoOnline.DominioTest.Alunos;

/*
 * Histórias do usuário:
 * Eu, enquanto administrador do sistema, quero cadastrar um Aluno com
 * nome, cpf, email e público alvo para poder efetuar sua matrícula.
 * 
 * Eu, enquanto administrador do sistema, quero editar somente
 * o nome do Aluno para poder corrigi-lo em caso de erro.
 */
public class AlunoTest
{
    private string _nome;
    private string _cpf;
    private string _email;
    private PublicoAlvo _publicoAlvo;

    private readonly Faker _faker;

    public AlunoTest()
    {
        _faker = new Faker();

        _nome = _faker.Person.FullName;
        _cpf = _faker.Person.Cpf(true);
        _email = _faker.Person.Email;
        _publicoAlvo = PublicoAlvo.Estudante;
    }

    [Fact]
    public void DeveCadastrarUmAluno()
    {
        // Arrange
        var alunoEsperado = new
        {
            Nome = _nome,
            Cpf = _cpf,
            Email = _email,
            PublicoAlvo = _publicoAlvo,
        };

        // Act
        var aluno = new Aluno(
            alunoEsperado.Nome,
            alunoEsperado.Cpf,
            alunoEsperado.Email,
            alunoEsperado.PublicoAlvo);

        // Assert
        alunoEsperado.ToExpectedObject().ShouldMatch(aluno);
    }

    [Fact]
    public void DevePermitirEditarNomeAluno()
    {
        var nomeAluno = _faker.Person.FullName;

        var aluno = AlunoBuilder.Novo()
            .ComNome(nomeAluno)
            .Build();

        aluno.AlterarNome(nomeAluno);

        Assert.Equal(nomeAluno, aluno.Nome);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void NaoDeveAlterarNomeInvalido(string nomeInvalido)
    {
        var aluno = AlunoBuilder.Novo().Build();

        Assert.Throws<ExcecaoDeDominio>(() => aluno.AlterarNome(nomeInvalido))
            .ComMensagem(Resource.NomeInvalido);
    }
}

public class Aluno
{
    public string Nome { get; protected set; }
    public string Cpf { get; protected set; }
    public string Email { get; protected set; }
    public PublicoAlvo PublicoAlvo { get; protected set; }

    public Aluno(string nome, string cpf, string email, PublicoAlvo publicoAlvo)
    {
        Nome = nome;
        Cpf = cpf;
        Email = email;
        PublicoAlvo = publicoAlvo;
    }

    public void AlterarNome(string nome)
    {
        if (string.IsNullOrEmpty(nome))
            throw new ExcecaoDeDominio(new List<string> { Resource.NomeInvalido });

        Nome = nome;
    }
}

public class AlunoBuilder
{
    private readonly Faker _faker  = new Faker();
    private string _nome;
    private readonly string _cpf;
    private readonly string _email;
    private readonly PublicoAlvo _publicoAlvo;

    public AlunoBuilder()
    {
        _nome = _faker.Person.FullName;
        _cpf = _faker.Person.Cpf(true);
        _email = _faker.Person.Email;
        _publicoAlvo = PublicoAlvo.Estudante;
    }

    public static AlunoBuilder Novo()
    {
        return new AlunoBuilder();
    }

    public AlunoBuilder ComNome(string nome)
    {
        _nome = nome;
        return this;
    }

    public Aluno Build()
    {
        var aluno = new Aluno(
            _nome,
            _cpf,
            _email,
            _publicoAlvo);

        return aluno;
    }
}
