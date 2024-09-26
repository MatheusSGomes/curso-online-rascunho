using Microsoft.AspNetCore.Mvc.Filters;

namespace CursoOnline.Web.Filters;

public class CustomExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        base.OnException(context);
    }
}
