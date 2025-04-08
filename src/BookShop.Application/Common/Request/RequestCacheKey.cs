using BookShop.Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Text;

namespace BookShop.Application.Common.Request
{
    public static class RequestCacheKey
    {
        private static JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CacheRequestContractResolver(),
        };


        private class CacheRequestContractResolver : DefaultContractResolver
        {
            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                var properties = base.CreateProperties(type, memberSerialization);

                properties = properties.Where(p => p.PropertyName.Contains("Cache") == false).ToList();

                return properties;
            }
        }

        public static string GetKey<TRequest,TResponse>(TRequest request) 
            where TRequest : CachableRequest<TResponse>
        {
            string json = JsonConvert.SerializeObject(request, settings);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            string encodedJson = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
            return $"{typeof(TRequest).Name}-{encodedJson}";
        }

        //public static string GetKey<TRequest>(TRequest request)
        //    where TRequest : ICachableRequest
        //{
        //    string json = JsonConvert.SerializeObject(request, settings);
        //    byte[] bytes = Encoding.UTF8.GetBytes(json);
        //    string encodedJson = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
        //    return $"{nameof(TRequest)}-{encodedJson}";
        //}

    }

}
