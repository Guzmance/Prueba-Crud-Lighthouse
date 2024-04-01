using CRUD_Lighthouse.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CRUD_Lighthouse.Controllers
{
    public class HomeController : Controller
    {
        // Web API REST Service base url
        string Baseurl = "https://api.escuelajs.co/api/v1/";

        // Método para obtener todas las categorías
        public async Task<ActionResult> Index()
        {
            List<Category> categoryInfo = new List<Category>();
            using (var client = new HttpClient())
            {
                // Configurar la base url y los encabezados de la solicitud HTTP
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Realizar la solicitud HTTP para obtener todas las categorías
                HttpResponseMessage response = await client.GetAsync("categories");

                // Verificar si la solicitud fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Leer la respuesta JSON y deserializarla en una lista de categorías
                    var categoryResponse = await response.Content.ReadAsStringAsync();
                    categoryInfo = JsonConvert.DeserializeObject<List<Category>>(categoryResponse);
                }

                // Devolver la lista de categorías a la vista
                return View(categoryInfo);
            }
        }

        // Método para obtener una categoría individual por ID
        public async Task<ActionResult> Details(int id)
        {
            Category category = new Category();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"categories/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var categoryResponse = await response.Content.ReadAsStringAsync();
                    category = JsonConvert.DeserializeObject<Category>(categoryResponse);
                }

                return View(category);
            }
        }

    }
}
