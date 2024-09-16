namespace CursoOnline.DominioTest.Curso;

public class CursoTest
{
    [Fact]
    public void DeveCriarCurso()
    {
        // Arrange
        string nome = "Meu Curso";
        int cargaHoraria = 200;
        string publicoAlvo = "Estudantes";
        decimal valor = 2500;

        // Act
        var curso = new Curso(nome, cargaHoraria, publicoAlvo, valor);
        
        // Assert
        Assert.Equal(nome, curso.Nome);
        Assert.Equal(cargaHoraria, curso.CargaHoraria);
        Assert.Equal(publicoAlvo, curso.PublicoAlvo);
        Assert.Equal(valor, curso.Valor);
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
