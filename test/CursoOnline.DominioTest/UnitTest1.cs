namespace CursoOnline.DominioTest;

public class UnitTest1
{
    [Fact(DisplayName = "As variáveis devem ter o mesmo valor")]
    public void DeveAsVariaveisTeremOMesmoValor()
    {
        // Organização do código
        var variavel1 = 1;
        var variavel2 = 1;

        // Ação
        variavel2 = variavel1;
        
        // Assert
        Assert.Equal(variavel1, variavel2);
    }
}
