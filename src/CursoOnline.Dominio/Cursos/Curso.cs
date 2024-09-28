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
            .Quando(string.IsNullOrEmpty(nome), "Nome inválido")
            .Quando(cargaHoraria < 1, "Carga horária inválida")
            .Quando(valor < 1, "Valor do curso inválido")
            .DispararExcecaoSeExistir();

        Nome = nome;
        Descricao = descricao;
        CargaHoraria = cargaHoraria;
        PublicoAlvo = publicoAlvo;
        Valor = valor;
    }
}
