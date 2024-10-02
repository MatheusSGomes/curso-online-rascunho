using Bogus;
using Bogus.Extensions.Brazil;
using CursoOnline.Dominio.Cursos;
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
}

public class Aluno
{
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public string Email { get; set; }
    public PublicoAlvo PublicoAlvo { get; set; }

    public Aluno(string nome, string cpf, string email, PublicoAlvo publicoAlvo)
    {
        Nome = nome;
        Cpf = cpf;
        Email = email;
        PublicoAlvo = publicoAlvo;
    }
}
