using CursoOnline.Dominio.Cursos;
using Moq;

namespace CursoOnline.DominioTest.Cursos;

public class ArmazenadorDeCursoTeste
{
    [Fact]
    public void DeveAdicionarCurso()
    {
        var cursoDto = new CursoDto
        {
            Nome = "Nome curso",
            Descricao = "Descrição do curso",
            CargaHoraria = 80,
            PublicoAlvo = 1,
            Valor = 980
        };

        var cursoRepositoryMock = new Mock<ICursoRepositorio>();

        var armazenadorDeCurso = new ArmazenadorDeCurso(cursoRepositoryMock.Object);
        armazenadorDeCurso.Armazenar(cursoDto);

        cursoRepositoryMock.Verify(r => 
            r.Adicionar(It.IsAny<Curso>()));
    }
}

public interface ICursoRepositorio
{
    void Adicionar(Curso curso);
    void Atualizar(Curso curso);
}

public class ArmazenadorDeCurso
{
    private readonly ICursoRepositorio _cursoRepositorio;

    public ArmazenadorDeCurso(ICursoRepositorio cursoRepositorio)
    {
        _cursoRepositorio = cursoRepositorio;
    }

    public void Armazenar(CursoDto cursoDto)
    {
        var curso = 
            new Curso(cursoDto.Nome, cursoDto.Descricao, cursoDto.CargaHoraria, PublicoAlvo.Estudante, cursoDto.Valor);
        
        _cursoRepositorio.Adicionar(curso); // testamos se esse método foi chamado
    }
}

public class CursoDto
{
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public int CargaHoraria { get; set; }
    public int PublicoAlvo { get; set; }
    public decimal Valor { get; set; }
}
