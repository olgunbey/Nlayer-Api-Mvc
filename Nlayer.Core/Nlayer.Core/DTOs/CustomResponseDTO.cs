using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Nlayer.Core.DTOs
{
    public class CustomResponseDTO<T>
    {
        public T? Data { get; set; }
        [JsonIgnore]
        public int StatusCode { get; set; }
        public List<string>? Errors { get; set; }
        public static CustomResponseDTO<T> Success(int statusCode, T data)
        {
            return new CustomResponseDTO<T> { StatusCode = statusCode, Data = data };
        }
        public static CustomResponseDTO<T> Success(int statusCode)
        {
            return new CustomResponseDTO<T> { StatusCode = statusCode };
        }
        public static CustomResponseDTO<T> Fail(List<string> errors,int statusCode)
        {
            return new CustomResponseDTO<T> { StatusCode = statusCode,Errors=errors };
        }
        public static CustomResponseDTO<T> Fail(int statusCode, string error)
        {
            return new CustomResponseDTO<T> { StatusCode = statusCode, Errors =new List<string>{error }};
        }

        public readonly struct Response<T> //struct kullanma sebebimiz structlar bir value'dir ve bunlar her zaman stack'te saklanır. Bu yüzden bunu ayrı bir class'a yazmak performans kaybına sebep olacaktır
        {
            public IActionResult Fonksiyon(CustomResponseDTO<T> response) //IActionResult dönüyorum birtek.
            {
                if(response.StatusCode==204) //içeriğin var olup olmadıgını kontrol ediyor
                {
                    return new ObjectResult(null);
                }
                return new ObjectResult(response) { StatusCode=response.StatusCode};
            }
        }
    }
}
