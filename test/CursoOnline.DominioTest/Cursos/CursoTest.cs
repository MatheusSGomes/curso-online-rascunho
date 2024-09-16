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
            PublicoAlvo = "Estudantes",
            Valor = (decimal) 2950
        };

        // Act
        var curso = new Curso(cursoEsperado.Nome, cursoEsperado.CargaHoraria, cursoEsperado.PublicoAlvo, cursoEsperado.Valor);
        
        // Assert
        cursoEsperado.ToExpectedObject().ShouldMatch(curso);
    }
}

public class Curso
{
    public string Nome { get; }
    public int CargaHoraria { get; }
    public string PublicoAlvo { get; }
    public decimal Valor { get; }

    public Curso(string nome, int cargaHoraria, string publicoAlvo, decimal valor)
    {
        Nome = nome;
        CargaHoraria = cargaHoraria;
        PublicoAlvo = publicoAlvo;
        Valor = valor;
    }
        
}
