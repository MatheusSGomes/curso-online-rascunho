using CursoOnline.Dominio._Base;
using CursoOnline.Dominio.Cursos;

namespace CursoOnline.Dominio.PublicosAlvo;

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