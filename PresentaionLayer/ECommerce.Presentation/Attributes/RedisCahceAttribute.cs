using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Service.Abstraction.Caches;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Presentation.Attributes
{
    public class RedisCahceAttribute(int ExpiredInMinutes = 5) : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Get CacheService from DI Container
            ICacheService cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>()!;

            // Create Cache Key
            string cacheKey = CreateCacheKey(context.HttpContext.Request);

            // Search value in cache key
            var cacheValue = await cacheService.GetAsync(cacheKey);

            // Return Value Not Null
            if (cacheValue is not null)
            {
                context.Result = new ContentResult()
                {
                    Content = cacheValue,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }

            // Value is Null
            // Invoke Next
            var ExecutedContext = await next.Invoke();

            // Set Value With Cache Key
            if (ExecutedContext.Result is OkObjectResult result)
            {
                await cacheService.SetAsync(cacheKey, result.Value!, TimeSpan.FromMinutes(ExpiredInMinutes));
            }

        }

        private static string CreateCacheKey(HttpRequest request)
        {
            StringBuilder key = new StringBuilder();
            key.Append(request.Path);
            foreach (var item in request.Query.OrderBy(X => X.Key))
            {
                key.Append($"{item.Key}={item.Value}&");
            }
            return key.ToString();
        }
    }
}
