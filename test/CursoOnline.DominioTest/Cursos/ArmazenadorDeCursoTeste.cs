using Bogus;
using CursoOnline.Dominio.Cursos;
using CursoOnline.DominioTest._Utils;
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
            PublicoAlvo = "Estudante",
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
                    c.Valor == _cursoDto.Valor)));
    }

    [Fact]
    public void NaoDeveInformarPublicoAlvoInvalido()
    {
        var publicoAlvoInvalido = "Medico";
        _cursoDto.PublicoAlvo = publicoAlvoInvalido;

        Assert.Throws<ArgumentException>(() => _armazenadorDeCurso.Armazenar(_cursoDto))
            .ComMensagem("Público Alvo Inválido");
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
        Enum.TryParse(typeof(PublicoAlvo), cursoDto.PublicoAlvo, out var publicoAlvo);

        if (publicoAlvo == null)
            throw new ArgumentException("Público Alvo Inválido");

        var curso = 
            new Curso(cursoDto.Nome, cursoDto.Descricao, cursoDto.CargaHoraria, (PublicoAlvo) publicoAlvo, cursoDto.Valor);
        
        _cursoRepositorio.Adicionar(curso); // testamos se esse método foi chamado
    }
}

public class CursoDto
{
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public int CargaHoraria { get; set; }
    public string PublicoAlvo { get; set; }
    public decimal Valor { get; set; }
}
