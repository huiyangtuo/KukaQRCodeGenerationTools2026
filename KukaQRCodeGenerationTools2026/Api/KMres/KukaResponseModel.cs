using Newtonsoft.Json;

namespace KukaQRCodeGenerationTools2026.Api.KMres
{
    internal class KukaResponseModel<T>
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("msg")]
        public string Msg { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}
