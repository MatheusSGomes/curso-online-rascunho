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
            .ComMensagem(Resource.PublicoAlvoInvalido);
    }

    [Fact]
    public void NaoDeveCursoComMesmoNomeDeOutroJaSalvo()
    {
        var cursoJaSalvo = CursoBuilder.Novo().ComId(321).ComNome(_cursoDto.Nome).Build();

        _cursoRepositoryMock.Setup(cursoRepositoryInterface => 
                cursoRepositoryInterface.ObterPeloNome(_cursoDto.Nome)).Returns(cursoJaSalvo);

        Assert.Throws<ExcecaoDeDominio>(() => _armazenadorDeCurso.Armazenar(_cursoDto))
            .ComMensagem(Resource.NomeDoCursoJaExiste);
    }

    [Fact]
    public void DeveAlterarDadosDoCurso()
    {
        _cursoDto.Id = 123;

        var curso = CursoBuilder.Novo().Build();
        _cursoRepositoryMock.Setup(r => r.ObterPorId(_cursoDto.Id))
            .Returns(curso);

        _armazenadorDeCurso.Armazenar(_cursoDto);

        Assert.Equal(_cursoDto.Nome, curso.Nome);
        Assert.Equal(_cursoDto.Valor, curso.Valor);
        Assert.Equal(_cursoDto.CargaHoraria, curso.CargaHoraria);
    }
    
    [Fact]
    public void NaoDeveAdicionaNoRepositorioQuandoCursoJaExiste()
    {
        _cursoDto.Id = 123;
    
        var curso = CursoBuilder.Novo().Build();
        _cursoRepositoryMock.Setup(r => r.ObterPorId(_cursoDto.Id))
            .Returns(curso);
    
        _armazenadorDeCurso.Armazenar(_cursoDto);
        
        _cursoRepositoryMock.Verify(r => r.Adicionar(It.IsAny<Curso>()), Times.Never);
    }
}
