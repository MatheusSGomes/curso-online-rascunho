using CursoOnline.Dominio.Matriculas;
using CursoOnline.DominioTest._Builders;
using Moq;

namespace CursoOnline.DominioTest.Matriculas;

/*
 * Classe de testes responsável por testar Domain Service específica.
 * Antigamente, tudo era feito em uma única classe (MatriculaService),
 * Porém, isso desrespeitava o princípio da responsabilidade única
 */
public class CancelamentoDaMatriculaTest
{
    private readonly Mock<IMatriculaRepositorio> _matriculaRepositorio;
    private readonly CancelamentoDaMatricula _cancelamentoDaMatricula;

    public CancelamentoDaMatriculaTest()
    {
        _matriculaRepositorio = new Mock<IMatriculaRepositorio>();
        _cancelamentoDaMatricula = new CancelamentoDaMatricula(_matriculaRepositorio.Object);
    }

    [Fact]
    public void DeveCancelarMatricula() // Deve Cancelar matricula... Faltou o Quando...?
    {
        var matricula = MatriculaBuilder.Novo().Build();

        // Preparar a minha repository para que quando ela passar pelo método ObterPorId, ela vai retornar um objeto do tipo Matricula
        _matriculaRepositorio.Setup(r => r.ObterPorId(matricula.Id)).Returns(matricula);

        // Chamo a minha "Domain Service" (ela utiliza o mock da repository, logo, ela vai responder com o objeto do tipo Matricula)
        _cancelamentoDaMatricula.Cancelar(matricula.Id);
        
        Assert.True(matricula.Cancelada);
    }
}

public class CancelamentoDaMatricula
{
    private readonly IMatriculaRepositorio _matriculaRepositorio;

    public CancelamentoDaMatricula(IMatriculaRepositorio matriculaRepositorio)
    {
        _matriculaRepositorio = matriculaRepositorio;
    }

    public void Cancelar(int matriculaId)
    {
        Matricula matricula = _matriculaRepositorio.ObterPorId(matriculaId);

        matricula.Cancelar();
    }
}
