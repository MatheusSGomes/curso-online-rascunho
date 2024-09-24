namespace CursoOnline.Dominio._Base;

public interface IUnitOfWork
{
    Task Commit();
}
