using CursoOnline.Dominio._Base;
using CursoOnline.Dominio.Cursos;
using CursoOnline.DominioTest._Utils;

namespace CursoOnline.DominioTest.PublicosAlvo;

public class ConversorDePublicoAlvoTest
{
    private ConversorDePublicoAlvo _conversor = new();

    [Theory]
    [InlineData(PublicoAlvo.Estudante, "Estudante")]
    [InlineData(PublicoAlvo.Universitario, "Universitario")]
    [InlineData(PublicoAlvo.Empregado, "Empregado")]
    [InlineData(PublicoAlvo.Empreendedor, "Empreendedor")]
    public void DeveConverterPublicoAlvo(PublicoAlvo publicoAlvoEsperado, string publicoAlvo)
    {
        var publicoAlvoConvertido = _conversor.Converter(publicoAlvo);

        Assert.Equal(publicoAlvoEsperado, publicoAlvoConvertido);
    }

    [Fact]
    public void NaoDeveConverterQuandoPublicoAlvoForInvalido()
    {
        const string publicoAlvoInvalido = "PublicoInvalido";

        Assert.Throws<ExcecaoDeDominio>(() => _conversor.Converter(publicoAlvoInvalido))
            .ComMensagem(Resource.PublicoAlvoInvalido);
    }

}

public class ConversorDePublicoAlvo
{
    public PublicoAlvo Converter(string publicoAlvo)
    {
        ValidadorDeRegra.Novo()
            .Quando(!Enum.TryParse<PublicoAlvo>(publicoAlvo, out var publicoAlvoConvertido),
                Resource.PublicoAlvoInvalido)
            .DispararExcecaoSeExistir();

        return publicoAlvoConvertido;
    }
}
