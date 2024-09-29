using CursoOnline.Dominio._Base;
using CursoOnline.Dominio.Cursos;

namespace CursoOnline.DominioTest._Builders;

public class CursoBuilder
{
    private int _id;
    private string _nome = "Informática";
    private string _descricao = "Descrição do curso";
    private int _cargaHoraria = 80;
    private PublicoAlvo _publicoAlvo = PublicoAlvo.Estudante;
    private decimal _valor = 2950;

    public static CursoBuilder Novo()
    {
        return new CursoBuilder();
    }

    public CursoBuilder ComId(int id)
    {
        _id = id;
        return this;
    }

    public CursoBuilder ComNome(string nome)
    {
        _nome = nome;
        return this;
    }
    
    public CursoBuilder ComDescricao(string descricao)
    {
        _descricao = descricao;
        return this;
    }
    
    public CursoBuilder ComCargaHoraria(int cargaHoraria)
    {
        _cargaHoraria = cargaHoraria;
        return this;
    }
    
    public CursoBuilder ComPublicoAlvo(PublicoAlvo publicoAlvo)
    {
        _publicoAlvo = publicoAlvo;
        return this;
    }
    
    public CursoBuilder ComValor(decimal valor)
    {
        _valor = valor;
        return this;
    }

    public Curso Build()
    {
        var curso = new Curso(_nome, _descricao, _cargaHoraria, _publicoAlvo, _valor);

        if (_id > 0)
        {
            var propertyInfo = curso.GetType().GetProperty("Id");
            propertyInfo.SetValue(curso, Convert.ChangeType(_id, propertyInfo.PropertyType), null);
        }

        return curso;
    }
}
