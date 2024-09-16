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

        var message = Assert.Throws<ArgumentException>(() =>
            new Curso(nomeCursoInvalido, cursoEsperado.CargaHoraria, PublicoAlvo.Estudante, cursoEsperado.Valor)).Message;

        Assert.Equal("Nome inválido", message);
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
        
        var message = Assert.Throws<ArgumentException>(() =>
            new Curso(cursoEsperado.Nome, cargaHorariaInvalida, PublicoAlvo.Estudante, cursoEsperado.Valor)).Message;
        
        Assert.Equal("Carga horária inválida", message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void NaoDeveCursoTerValorMenorQue1(decimal valorCursoInvalido)
    {
        var cursoEsperado = new
        {
            Nome = "Informática",
            CargaHoraria = 80,
            PublicoAlvo = PublicoAlvo.Estudante,
            Valor = (decimal) 2950
        };

        var message = Assert.Throws<ArgumentException>(() => 
                new Curso(cursoEsperado.Nome, cursoEsperado.CargaHoraria, PublicoAlvo.Estudante, valorCursoInvalido)).Message;

        Assert.Equal("Valor do curso inválido", message);
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
