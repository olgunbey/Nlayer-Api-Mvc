using Microsoft.AspNetCore.Diagnostics;
using Nlayer.Core.DTOs;
using Nlayer.Services.Exceptions;
using System.Reflection.Metadata;
using System.Text.Json;

namespace Nlayer.API.Middlewares
{
    public static class Register
    {
        public static void Middleware(this IApplicationBuilder serviceDescriptors) //throwla atılan hataları burada statuscode verebiliriz, bu middleware birtek throw algılayınca çalışır
        {
            
            
            serviceDescriptors.UseExceptionHandler(cf =>
            {
                cf.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    IExceptionHandlerFeature exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();

                    int statuscode = exceptionFeature.Error switch //burada throw'un fırlattığı hatalar hangi tipteyse ona gore bir statuscode alıyoruz.
                    {
                        ClientSideException => 400,
                        NotFoundException => 404,
                        _ => 500
                    };
                    //int statuscode = 0;
                    //switch (exceptionFeature.Error)
                    //{
                    //    case ClientSideException:
                    //        statuscode = 400;
                    //        break;
                    //    case NotFoundException:
                    //        statuscode = 404;
                    //        break;
                    //}

                    context.Response.StatusCode = statuscode; //client tarafında donen hatanın statuscode'unu ekrana verir çünkü burada customResponseController dönmedik.
                    CustomResponseDTO<CustomNoContentResponseDTO> Response = CustomResponseDTO<CustomNoContentResponseDTO>.Fail(statuscode, exceptionFeature.Error.Message);

                    await context.Response.WriteAsync(JsonSerializer.Serialize(Response)); //en son döndügün response'yi json formata çevirerek döndürdük
                    
                });
                //cf.Run(async context =>
                //{
                //    Console.WriteLine("abcxx");
                //});
            });
            
        }
        
    }
}
