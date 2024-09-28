using CursoOnline.Dominio._Base;

namespace CursoOnline.DominioTest._Utils;

public static class AssertExtension
{
    public static void ComMensagem(this ExcecaoDeDominio exception, string mensagem)
    {
        if (exception.MensagensDeErro.Contains(mensagem))
            Assert.True(true);
        else
            Assert.False(true, $"Expected: {mensagem}\n Actual: {exception.Message}");
    }
}
