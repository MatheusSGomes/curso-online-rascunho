using Bogus;
using Bogus.Extensions.Brazil;
using CursoOnline.Dominio.Alunos;
using CursoOnline.Dominio.Cursos;

namespace CursoOnline.DominioTest._Builders;

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
