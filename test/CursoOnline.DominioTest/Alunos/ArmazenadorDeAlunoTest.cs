using CursoOnline.Dominio._Base;
using CursoOnline.Dominio.Alunos;
using CursoOnline.Dominio.Cursos;
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
         * Verifico se o método adicionar foi chamado.
         * Verifico se foi passado algum parâmetro do tipo Aluno para ele.
         */
        alunoRepositoryMock.Verify(
            alunoRepository => alunoRepository.Adicionar(It.IsAny<Aluno>()));
    }
}

public interface IAlunoRepository : IRepositorio<Aluno>
{

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
        var aluno =
            new Aluno(alunoDto.Nome, alunoDto.Cpf, alunoDto.Email, alunoDto.PublicoAlvo);

        _alunoRepository.Adicionar(aluno);
    }
}
