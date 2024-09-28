using Bogus;
using CursoOnline.Dominio._Base;
using CursoOnline.Dominio.Cursos;
using CursoOnline.DominioTest._Builders;
using CursoOnline.DominioTest._Utils;
using Moq;

namespace CursoOnline.DominioTest.Cursos;

public class ArmazenadorDeCursoTest
{
    private readonly CursoDto _cursoDto;
    private readonly ArmazenadorDeCurso _armazenadorDeCurso;
    private readonly Mock<ICursoRepositorio> _cursoRepositoryMock;

    public ArmazenadorDeCursoTest()
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

        Assert.Throws<ExcecaoDeDominio>(() => _armazenadorDeCurso.Armazenar(_cursoDto))
            .ComMensagem("Público Alvo Inválido");
    }

    [Fact]
    public void NaoDeveCursoComMesmoNomeDeOutroJaSalvo()
    {
        var cursoJaSalvo = CursoBuilder.Novo().ComNome(_cursoDto.Nome).Build();

        _cursoRepositoryMock.Setup(cursoRepositoryInterface => 
                cursoRepositoryInterface.ObterPeloNome(_cursoDto.Nome)).Returns(cursoJaSalvo);
        
        Assert.Throws<ExcecaoDeDominio>(() => _armazenadorDeCurso.Armazenar(_cursoDto))
            .ComMensagem("Nome do curso já consta no banco de dados");
    }
}
