namespace CursoOnline.Dominio.Cursos;

public class CursoDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public int CargaHoraria { get; set; }
    public string PublicoAlvo { get; set; }
    public decimal Valor { get; set; }
}
