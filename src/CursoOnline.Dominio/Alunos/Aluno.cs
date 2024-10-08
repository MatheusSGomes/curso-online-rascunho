using System.Text.RegularExpressions;
using CursoOnline.Dominio._Base;
using CursoOnline.Dominio.Cursos;

namespace CursoOnline.Dominio.Alunos;

public class Aluno : Entidade
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
            .Quando(string.IsNullOrEmpty(cpf) || !_cpfRegex.Match(cpf).Success, Resource.CpfInvalido)
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
