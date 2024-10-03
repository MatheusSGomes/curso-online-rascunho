using Bogus;
using Bogus.Extensions.Brazil;
using CursoOnline.Dominio._Base;
using CursoOnline.Dominio.Alunos;
using CursoOnline.Dominio.Cursos;
using CursoOnline.DominioTest._Builders;
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
        var alunoEsperado = new
        {
            Nome = _nome,
            Cpf = _cpf,
            Email = _email,
            PublicoAlvo = _publicoAlvo,
        };

        var aluno = new Aluno(
            alunoEsperado.Nome,
            alunoEsperado.Cpf,
            alunoEsperado.Email,
            alunoEsperado.PublicoAlvo);

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
    [InlineData("CPF inválido")]
    [InlineData("00000000000")]
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
