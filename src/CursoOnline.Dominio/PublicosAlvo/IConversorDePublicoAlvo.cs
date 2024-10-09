using CursoOnline.Dominio.Cursos;

namespace CursoOnline.Dominio.PublicosAlvo;

public interface IConversorDePublicoAlvo
{
    PublicoAlvo Converter(string publicoAlvo);
}