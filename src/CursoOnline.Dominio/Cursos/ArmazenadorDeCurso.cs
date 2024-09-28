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
            .Quando(cursoJaSalvo != null, "Nome do curso já consta no banco de dados")
            .Quando(!Enum.TryParse<PublicoAlvo>(cursoDto.PublicoAlvo, out var publicoAlvo), "Público Alvo Inválido")
            .DispararExcecaoSeExistir();

        var curso = 
            new Curso(cursoDto.Nome, cursoDto.Descricao, cursoDto.CargaHoraria, publicoAlvo, cursoDto.Valor);

        _cursoRepositorio.Adicionar(curso); // testamos se esse método foi chamado
    }
}
