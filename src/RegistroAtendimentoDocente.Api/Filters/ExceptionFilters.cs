using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RegistroAtendimentoDocente.Communication.Responses;
using RegistroAtendimentoDocente.Exception;
using RegistroAtendimentoDocente.Exception.ExceptionsBase;

namespace RegistroAtendimentoDocente.Api.Filters;

public class ExceptionFilters : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if(context.Exception is RegistroAtendimentoDocenteException)
        {
            HandleExceptionProject(context);
        }
        else
        {
            ThrowUnknowError(context);
        }
    }

    private void HandleExceptionProject(ExceptionContext context)
    {
        var registroAtendException = (RegistroAtendimentoDocenteException)context.Exception;
        var errorResponse = new ResponseAtendimentoErrorJson(registroAtendException.GetErrors());

        context.HttpContext.Response.StatusCode = registroAtendException.StatusCode;
        context.Result = new ObjectResult(errorResponse);
    }

    private void ThrowUnknowError(ExceptionContext context) {
        var errorResponse = new ResponseAtendimentoErrorJson(ResourceErrorMessages.UNKNOW_ERROR);

        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(errorResponse);
    }
}
