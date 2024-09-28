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

    private readonly Faker _faker;

    public CursoTest(ITestOutputHelper output)
    {
        _output = output;
        output.WriteLine("Setup");
        
        _faker = new Faker();
        
        _nome = _faker.Random.Words();
        _descricao = _faker.Lorem.Paragraph();
        _cargaHoraria = _faker.Random.Int(50, 1000);
        _publicoAlvo = PublicoAlvo.Estudante;
        _valor = _faker.Random.Int(100, 5000);
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
            .ComMensagem(Resource.NomeInvalido);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void NaoDeveTerCursoCargaHorariaInvalida(int cargaHorariaInvalida)
    {
        Assert.Throws<ExcecaoDeDominio>(() =>
            CursoBuilder.Novo().ComCargaHoraria(cargaHorariaInvalida).Build())
            .ComMensagem(Resource.CargaHorariaInvalida);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void NaoDeveCursoTerValorInvalido(decimal valorCursoInvalido)
    {
        Assert.Throws<ExcecaoDeDominio>(() =>
            CursoBuilder.Novo().ComValor(valorCursoInvalido).Build())
            .ComMensagem(Resource.ValorInvalido);
    }

    [Fact]
    public void DeveAlterarNome()
    {
        var nomeEsperado = _faker.Person.FullName;
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
            .ComMensagem(Resource.NomeInvalido);
    }

    [Fact]
    public void DeveAlterarCargaHoraria()
    {
        var cargaHorariaEsperada = 20;
        var curso = CursoBuilder.Novo().Build();

        curso.AlterarCargaHoraria(cargaHorariaEsperada);
        
        Assert.Equal(cargaHorariaEsperada, curso.CargaHoraria);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-2)]
    [InlineData(-20)]
    public void NaoDeveAlterarCargaHorariaInvalida(int cargaHorariaInvalida)
    {
        var curso = CursoBuilder.Novo().Build();
        
        Assert.Throws<ExcecaoDeDominio>(() => curso.AlterarCargaHoraria(cargaHorariaInvalida))
            .ComMensagem(Resource.CargaHorariaInvalida);
    }

    [Fact]
    public void DeveAlterarValor()
    {
        var valorEsperado = (decimal) 234.99;
        var curso = CursoBuilder.Novo().Build();

        curso.AlterarValor(valorEsperado);

        Assert.Equal(valorEsperado, curso.Valor);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void NaoDeveAlterarComValorInvalido(decimal valorInvalido)
    {
        var curso = CursoBuilder.Novo().Build();
        
        Assert.Throws<ExcecaoDeDominio>(() => curso.AlterarValor(valorInvalido))
            .ComMensagem(Resource.ValorInvalido);
    }
}
