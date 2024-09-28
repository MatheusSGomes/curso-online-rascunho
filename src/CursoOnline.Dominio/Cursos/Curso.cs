using CursoOnline.Dominio._Base;

namespace CursoOnline.Dominio.Cursos;

public class Curso : Entidade
{
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public int CargaHoraria { get; set; }
    public PublicoAlvo PublicoAlvo { get; set; }
    public decimal Valor { get; set; }

    public Curso(string nome, string descricao, int cargaHoraria, PublicoAlvo publicoAlvo, decimal valor)
    {
        ValidadorDeRegra.Novo()
            .Quando(string.IsNullOrEmpty(nome), Resource.NomeInvalido)
            .Quando(cargaHoraria < 1, Resource.CargaHorariaInvalida)
            .Quando(valor < 1, Resource.ValorInvalido)
            .DispararExcecaoSeExistir();
    
        Nome = nome;
        Descricao = descricao;
        CargaHoraria = cargaHoraria;
        PublicoAlvo = publicoAlvo;
        Valor = valor;
    }

    public void AlterarNome(string nome)
    {
        ValidadorDeRegra.Novo()
            .Quando(string.IsNullOrEmpty(nome), Resource.NomeInvalido)
            .DispararExcecaoSeExistir();

        Nome = nome;
    }

    public void AlterarCargaHoraria(int cargaHoraria)
    {
        ValidadorDeRegra.Novo()
            .Quando(cargaHoraria < 1, Resource.CargaHorariaInvalida)
            .DispararExcecaoSeExistir();

        CargaHoraria = cargaHoraria;
    }

    public void AlterarValor(decimal valor)
    {
        ValidadorDeRegra.Novo()
            .Quando(valor < 1, Resource.ValorInvalido)
            .DispararExcecaoSeExistir();

        Valor = valor;
    }
}
