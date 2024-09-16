namespace CursoOnline.DominioTest._Utils;

public static class AssertExtension
{
    public static void ComMensagem(this ArgumentException exception, string mensagem)
    {
        if (exception.Message == mensagem)
            Assert.True(true);
        else
            Assert.False(true, $"Expected: {mensagem}\n Actual: {exception.Message}");
    }
}
