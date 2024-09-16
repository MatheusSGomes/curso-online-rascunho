using ExpectedObjects;

namespace CursoOnline.DominioTest.Curso;

public class CursoTest
{
    [Fact]
    public void DeveCriarCurso()
    {
        // Arrange
        var cursoEsperado = new
        {
            Nome = "Informática",
            CargaHoraria = 80,
            PublicoAlvo = PublicoAlvo.Estudante,
            Valor = (decimal) 2950
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
        var cursoEsperado = new
        {
            Nome = "Informática",
            CargaHoraria = 80,
            PublicoAlvo = PublicoAlvo.Estudante,
            Valor = (decimal) 2950
        };

        Assert.Throws<ArgumentException>(() =>
            new Curso(nomeCursoInvalido, cursoEsperado.CargaHoraria, PublicoAlvo.Estudante, cursoEsperado.Valor));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void NaoDeveTerCursoCargaHorariaMenorQue1(int cargaHorariaInvalida)
    {
        var cursoEsperado = new
        {
            Nome = "Informática",
            CargaHoraria = 80,
            PublicoAlvo = PublicoAlvo.Estudante,
            Valor = (decimal) 2950
        };
        
        Assert.Throws<ArgumentException>(() =>
            new Curso(cursoEsperado.Nome, cargaHorariaInvalida, PublicoAlvo.Estudante, cursoEsperado.Valor));
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
            throw new ArgumentException();
        
        if (cargaHoraria < 1)
            throw new ArgumentException();

        Nome = nome;
        CargaHoraria = cargaHoraria;
        PublicoAlvo = publicoAlvo;
        Valor = valor;
    }
}
