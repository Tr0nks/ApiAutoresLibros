using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiAutores.Filtros
{
    public class miFiltroDeAccion : IActionFilter
    {
        private readonly ILogger<miFiltroDeAccion> logger;

        public miFiltroDeAccion(ILogger<miFiltroDeAccion> logger)
        {
            this.logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation("Antes de ejecutar la Accion");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

            logger.LogInformation("Despues de ejecutar una Accion");
            
        }

        
    }
}
