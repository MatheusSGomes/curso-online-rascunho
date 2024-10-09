using CursoOnline.Dominio.Cursos;

namespace CursoOnline.DominioTest.PublicosAlvo;

public class ConversorDePublicoAlvoTest
{
    [Fact]
    public void DeveConverterPublicoAlvo()
    {
        var publicoAlvoEsperado = PublicoAlvo.Empregado;
        var conversor = new ConversorDePublicoAlvo();
        var publicoAlvoConvertido = conversor.Converter("Empregado");
        
        Assert.Equal(publicoAlvoEsperado, publicoAlvoConvertido);
    }
}

public class ConversorDePublicoAlvo
{
    public PublicoAlvo Converter(string publicoAlvo)
    {
        Enum.TryParse<PublicoAlvo>(publicoAlvo, out var publicoAlvoConvertido);
        return publicoAlvoConvertido;
    }
}
