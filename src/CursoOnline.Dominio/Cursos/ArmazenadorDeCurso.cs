using CursoOnline.Dominio._Base;

namespace CursoOnline.Dominio.Cursos;

public class ArmazenadorDeCurso
{
    private readonly ICursoRepositorio _cursoRepositorio;

    public ArmazenadorDeCurso(ICursoRepositorio cursoRepositorio)
    {
        _cursoRepositorio = cursoRepositorio;
    }

    public void Armazenar(CursoDto cursoDto)
    {
        var cursoJaSalvo = _cursoRepositorio.ObterPeloNome(cursoDto.Nome);
        
        ValidadorDeRegra.Novo()
            .Quando(cursoJaSalvo != null, Resource.NomeDoCursoJaExiste)
            .Quando(!Enum.TryParse<PublicoAlvo>(cursoDto.PublicoAlvo, out var publicoAlvo), Resource.PublicoAlvoInvalido)
            .DispararExcecaoSeExistir();

        var curso = 
            new Curso(cursoDto.Nome, cursoDto.Descricao, cursoDto.CargaHoraria, publicoAlvo, cursoDto.Valor);

        _cursoRepositorio.Adicionar(curso); // testamos se esse método foi chamado
    }
}
