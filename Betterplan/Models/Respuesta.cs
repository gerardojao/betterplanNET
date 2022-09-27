using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace ApiBase.Models
{
    public class Respuesta<T>
    {
        public int Ok { get; set; }
        [JsonIgnore]
        public List<T> Data { get; set; } = new List<T>();
        public string Message { get; set; }
    }

    public class Enlace
    {
        public string Url { get; set; }
    }
}