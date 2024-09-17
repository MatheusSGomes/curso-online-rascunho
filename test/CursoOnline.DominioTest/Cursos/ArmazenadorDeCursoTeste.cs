using Bogus;
using CursoOnline.Dominio.Cursos;
using Moq;

namespace CursoOnline.DominioTest.Cursos;

public class ArmazenadorDeCursoTeste
{
    private readonly CursoDto _cursoDto;
    private readonly ArmazenadorDeCurso _armazenadorDeCurso;
    private readonly Mock<ICursoRepositorio> _cursoRepositoryMock;

    public ArmazenadorDeCursoTeste()
    {
        var fake = new Faker();

        _cursoDto = new CursoDto
        {
            Nome = fake.Random.Words(),
            Descricao = fake.Lorem.Paragraphs(),
            CargaHoraria = fake.Random.Int(1, 100),
            PublicoAlvo = PublicoAlvo.Estudante,
            Valor = fake.Random.Int(100, 5000)
        };

        _cursoRepositoryMock = new Mock<ICursoRepositorio>();
        _armazenadorDeCurso = new ArmazenadorDeCurso(_cursoRepositoryMock.Object);
    }

    [Fact]
    public void DeveAdicionarCurso()
    {
        _armazenadorDeCurso.Armazenar(_cursoDto);

        _cursoRepositoryMock.Verify(r => 
            r.Adicionar(
                It.Is<Curso>(c => 
                    c.Nome == _cursoDto.Nome && 
                    c.Descricao == _cursoDto.Descricao && 
                    c.CargaHoraria == _cursoDto.CargaHoraria &&
                    c.PublicoAlvo == _cursoDto.PublicoAlvo &&
                    c.Valor == _cursoDto.Valor)));
    }
}

public interface ICursoRepositorio
{
    void Adicionar(Curso curso);
    void Atualizar(Curso curso);
}

public class ArmazenadorDeCurso
{
    private readonly ICursoRepositorio _cursoRepositorio;

    public ArmazenadorDeCurso(ICursoRepositorio cursoRepositorio)
    {
        _cursoRepositorio = cursoRepositorio;
    }

    public void Armazenar(CursoDto cursoDto)
    {
        var curso = 
            new Curso(cursoDto.Nome, cursoDto.Descricao, cursoDto.CargaHoraria, PublicoAlvo.Estudante, cursoDto.Valor);
        
        _cursoRepositorio.Adicionar(curso); // testamos se esse método foi chamado
    }
}

public class CursoDto
{
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public int CargaHoraria { get; set; }
    public PublicoAlvo PublicoAlvo { get; set; }
    public decimal Valor { get; set; }
}
