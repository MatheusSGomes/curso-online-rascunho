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
        Nome = nome;
        CargaHoraria = cargaHoraria;
        PublicoAlvo = publicoAlvo;
        Valor = valor;
    }
}
