using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Nlayer.Core.DTOs
{
    public class CustomNoContentResponseDTO //update insert gibi ifadelerde geriye data donmeye gerek olmadıgından nocontent ve generic class değil
    {
        [JsonIgnore]
        public int StatusCode { get; set; }
        public List<string>? Errors { get; set; }
    }
}
