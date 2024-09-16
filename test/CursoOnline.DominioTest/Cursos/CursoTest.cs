using CursoOnline.DominioTest._Utils;
using ExpectedObjects;
using Xunit.Abstractions;

namespace CursoOnline.DominioTest.Curso;

public class CursoTest
{
    private readonly string _nome;
    private readonly int _cargaHoraria;
    private readonly PublicoAlvo _publicoAlvo;
    private readonly decimal _valor;

    public CursoTest()
    {
        _nome = "Informática";
        _cargaHoraria = 80;
        _publicoAlvo = PublicoAlvo.Estudante;
        _valor = 2950;
    }
    
    [Fact]
    public void DeveCriarCurso()
    {
        // Arrange
        var cursoEsperado = new
        {
            Nome = _nome,
            CargaHoraria = _cargaHoraria,
            PublicoAlvo = _publicoAlvo,
            Valor = _valor
        };

        // Act
        var curso = new Curso(cursoEsperado.Nome, cursoEsperado.CargaHoraria, PublicoAlvo.Estudante, cursoEsperado.Valor);
        
        // Assert
        cursoEsperado.ToExpectedObject().ShouldMatch(curso);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void NaoDeveCursoTerUmNomeInvalido(string nomeCursoInvalido)
    {
        Assert.Throws<ArgumentException>(() =>
            new Curso(nomeCursoInvalido, _cargaHoraria, _publicoAlvo, _valor))
            .ComMensagem("Nome inválido");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void NaoDeveTerCursoCargaHorariaMenorQue1(int cargaHorariaInvalida)
    {
        Assert.Throws<ArgumentException>(() =>
            new Curso(_nome, cargaHorariaInvalida, _publicoAlvo, _valor))
            .ComMensagem("Carga horária inválida");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void NaoDeveCursoTerValorMenorQue1(decimal valorCursoInvalido)
    {
        Assert.Throws<ArgumentException>(() =>
            new Curso(_nome, _cargaHoraria, _publicoAlvo, valorCursoInvalido))
            .ComMensagem("Valor do curso inválido");
    }
}

public enum PublicoAlvo
{
    Estudante,
    Universitario,
    Empregado,
    Empreendedor
}

public class Curso
{
    public string Nome { get; }
    public int CargaHoraria { get; }
    public PublicoAlvo PublicoAlvo { get; }
    public decimal Valor { get; }

    public Curso(string nome, int cargaHoraria, PublicoAlvo publicoAlvo, decimal valor)
    {
        if (string.IsNullOrEmpty(nome))
            throw new ArgumentException("Nome inválido");

        if (cargaHoraria < 1)
            throw new ArgumentException("Carga horária inválida");

        if (valor < 1)
            throw new ArgumentException("Valor do curso inválido");

        Nome = nome;
        CargaHoraria = cargaHoraria;
        PublicoAlvo = publicoAlvo;
        Valor = valor;
    }
}
