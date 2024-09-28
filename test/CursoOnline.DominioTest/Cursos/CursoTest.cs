using Bogus;
using CursoOnline.Dominio._Base;
using CursoOnline.Dominio.Cursos;
using CursoOnline.DominioTest._Builders;
using CursoOnline.DominioTest._Utils;
using ExpectedObjects;
using Xunit.Abstractions;

namespace CursoOnline.DominioTest.Cursos;

public class CursoTest : IDisposable
{
    private readonly ITestOutputHelper _output;

    private readonly string _nome;
    private readonly string _descricao;
    private readonly int _cargaHoraria;
    private readonly PublicoAlvo _publicoAlvo;
    private readonly decimal _valor;

    public CursoTest(ITestOutputHelper output)
    {
        _output = output;
        output.WriteLine("Setup");
        
        var faker = new Faker();
        
        _nome = faker.Random.Words();
        _descricao = faker.Lorem.Paragraph();
        _cargaHoraria = faker.Random.Int(50, 1000);
        _publicoAlvo = PublicoAlvo.Estudante;
        _valor = faker.Random.Int(100, 5000);
    }
    
    public void Dispose()
    {
        _output.WriteLine("Cleanup");
    }

    [Fact]
    public void DeveCriarCurso()
    {
        // Arrange
        var cursoEsperado = new
        {
            Nome = _nome,
            Descricao = _descricao,
            CargaHoraria = _cargaHoraria,
            PublicoAlvo = _publicoAlvo,
            Valor = _valor
        };

        // Act
        var curso = new Curso(
            cursoEsperado.Nome,
            cursoEsperado.Descricao,
            cursoEsperado.CargaHoraria,
            PublicoAlvo.Estudante,
            cursoEsperado.Valor);

        // Assert
        cursoEsperado.ToExpectedObject().ShouldMatch(curso);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void NaoDeveCursoTerUmNomeInvalido(string nomeCursoInvalido)
    {
        Assert.Throws<ExcecaoDeDominio>(() =>
            CursoBuilder.Novo().ComNome(nomeCursoInvalido).Build())
            .ComMensagem("Nome inválido");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void NaoDeveTerCursoCargaHorariaMenorQue1(int cargaHorariaInvalida)
    {
        Assert.Throws<ExcecaoDeDominio>(() =>
            CursoBuilder.Novo().ComCargaHoraria(cargaHorariaInvalida).Build())
            .ComMensagem("Carga horária inválida");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void NaoDeveCursoTerValorMenorQue1(decimal valorCursoInvalido)
    {
        Assert.Throws<ExcecaoDeDominio>(() =>
            CursoBuilder.Novo().ComValor(valorCursoInvalido).Build())
            .ComMensagem("Valor do curso inválido");
    }

    [Fact]
    public void DeveAlterarNome()
    {
        var nomeEsperado = "José";
        var curso = CursoBuilder.Novo().Build();
        curso.AlterarNome(nomeEsperado);
        
        Assert.Equal(nomeEsperado, curso.Nome);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void NaoDeveAlterarComNomeInvalido(string nomeInvalido)
    {
        var curso = CursoBuilder.Novo().Build();

        Assert.Throws<ExcecaoDeDominio>(() => curso.AlterarNome(nomeInvalido))
            .ComMensagem("Nome inválido");
    }
}
