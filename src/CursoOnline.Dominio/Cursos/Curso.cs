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
        if (string.IsNullOrEmpty(nome))
            throw new ArgumentException("Nome inválido");

        if (cargaHoraria < 1)
            throw new ArgumentException("Carga horária inválida");

        if (valor < 1)
            throw new ArgumentException("Valor do curso inválido");

        Nome = nome;
        Descricao = descricao;
        CargaHoraria = cargaHoraria;
        PublicoAlvo = publicoAlvo;
        Valor = valor;
    }
}
