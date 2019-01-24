using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using todo;
using todo.Models;

namespace quickstartcore
{
    public class RemoteRepository<T> : IRemoteRepository<T> where T : class
    {
        public string BaseAPIUri { get; set; }
        public RemoteRepository(string baseUrl)
        {
            BaseAPIUri = baseUrl;
        }

        public async Task<string> CreateItemAsync(T item)
        {
            HttpContent requestContent = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8,"application/json");
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage res = await client.PostAsync(BaseAPIUri + "items", requestContent))
            using (HttpContent content = res.Content)
            {
                if (res.IsSuccessStatusCode)
                {
                    string id = await content.ReadAsStringAsync();
                    return id;
                }
                else
                    return "";
            }
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage res = await client.DeleteAsync(BaseAPIUri + "items/" + id))
            {
                return res.IsSuccessStatusCode;
            }
        }

        public async Task<T> GetItemAsync(string id)
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage res = await client.GetAsync(BaseAPIUri + "items/" + id))
            using (HttpContent content = res.Content)
            {
                string data = await content.ReadAsStringAsync();
                var item = JsonConvert.DeserializeObject<T>(data);
                return item;
            }
        }

        public async Task<IEnumerable<T>> GetItemsAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage res = await client.GetAsync(BaseAPIUri + "items"))
            using (HttpContent content = res.Content)
            {
                string data = await content.ReadAsStringAsync();
                var items = JsonConvert.DeserializeObject<IEnumerable<T>>(data);
                return items;
            }
        }

        public async Task<bool> UpdateItemAsync(string id, T item)
        {
            HttpContent requestContent = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage res = await client.PutAsync(BaseAPIUri + "items/" + id, requestContent))
            {
                return res.IsSuccessStatusCode;
            }
        }

       
    }
}



