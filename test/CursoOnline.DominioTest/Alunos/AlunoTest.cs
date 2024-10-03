using System.Text.RegularExpressions;
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
 *
 * Validar CPF, Validar Email
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
        var nomeAlunoEsperado = _faker.Person.FullName;

        var aluno = AlunoBuilder.Novo()
            .Build();

        aluno.AlterarNome(nomeAlunoEsperado);

        Assert.Equal(nomeAlunoEsperado, aluno.Nome);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void NaoDeveCriarAlunoComNomeInvalido(string nomeInvalido)
    {
        Assert.Throws<ExcecaoDeDominio>(() => AlunoBuilder.Novo().ComNome(nomeInvalido).Build())
            .ComMensagem(Resource.NomeInvalido);
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

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void NaoDeveCriarAlunoComCpfInvalido(string cpfInvalido)
    {
        Assert.Throws<ExcecaoDeDominio>(() => AlunoBuilder.Novo().ComCpf(cpfInvalido).Build())
            .ComMensagem(Resource.CpfInvalido);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("email inválido")]
    [InlineData("email@invalido")]
    public void NaoDeveCriarAlunoComEmailInvalido(string emailInvalido)
    {
        Assert.Throws<ExcecaoDeDominio>(() => AlunoBuilder.Novo().ComEmail(emailInvalido).Build())
            .ComMensagem(Resource.EmailInvalido);
    }
}

public class Aluno
{
    public string Nome { get; protected set; }
    public string Cpf { get; protected set; }
    public string Email { get; protected set; }
    public PublicoAlvo PublicoAlvo { get; protected set; }
    
    private readonly Regex _emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
    private readonly Regex _cpfRegex = new Regex(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$");

    public Aluno(string nome, string cpf, string email, PublicoAlvo publicoAlvo)
    {
        ValidadorDeRegra.Novo()
            .Quando(string.IsNullOrEmpty(nome), Resource.NomeInvalido)
            .Quando(string.IsNullOrEmpty(cpf) /*|| !_emailRegex.Match(cpf).Success*/, Resource.CpfInvalido)
            .Quando(string.IsNullOrEmpty(email) || !_emailRegex.Match(email).Success, Resource.EmailInvalido)
            .DispararExcecaoSeExistir();
        
        Nome = nome;
        Cpf = cpf;
        Email = email;
        PublicoAlvo = publicoAlvo;
    }

    public void AlterarNome(string nome)
    {
        ValidadorDeRegra.Novo()
            .Quando(string.IsNullOrEmpty(nome), Resource.NomeInvalido)
            .DispararExcecaoSeExistir();

        Nome = nome;
    }
}

public class AlunoBuilder
{
    private readonly Faker _faker  = new Faker();
    private string _nome;
    private string _cpf;
    private string _email;
    private PublicoAlvo _publicoAlvo;

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
    
    public AlunoBuilder ComCpf(string cpf)
    {
        _cpf = cpf;
        return this;
    }
    
    public AlunoBuilder ComEmail(string email)
    {
        _email = email;
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
