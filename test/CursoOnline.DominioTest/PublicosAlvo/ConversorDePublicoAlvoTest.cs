using CursoOnline.Dominio.Cursos;

namespace CursoOnline.DominioTest.PublicosAlvo;

public class ConversorDePublicoAlvoTest
{
    [Theory]
    [InlineData(PublicoAlvo.Estudante, "Estudante")]
    [InlineData(PublicoAlvo.Universitario, "Universitario")]
    [InlineData(PublicoAlvo.Empregado, "Empregado")]
    [InlineData(PublicoAlvo.Empreendedor, "Empreendedor")]
    public void DeveConverterPublicoAlvo(PublicoAlvo publicoAlvoEsperado, string publicoAlvo)
    {
        var conversor = new ConversorDePublicoAlvo();
        var publicoAlvoConvertido = conversor.Converter(publicoAlvo);

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
