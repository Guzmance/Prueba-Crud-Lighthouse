using CRUD_Lighthouse.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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

        // Método para eliminar una categoría por ID
        public async Task<ActionResult> Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"categories/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var categoryResponse = await response.Content.ReadAsStringAsync();
                    var category = JsonConvert.DeserializeObject<Category>(categoryResponse);

                    // Redirigir a la vista de confirmación con los detalles de la categoría
                    return View("Delete", category);
                }
                else
                {
                    // Manejar la situación en la que la solicitud al servidor no fue exitosa
                    // Puedes mostrar un mensaje de error o tomar otra acción apropiada
                    ViewBag.ErrorMessage = "Hubo un error al intentar obtener los detalles de la categoría.";
                    return View("Error");
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.DeleteAsync($"categories/{id}");

                if (response.IsSuccessStatusCode)
                {
                    // Redirigir a la página principal u otra vista apropiada después de la eliminación
                    return RedirectToAction("Index");
                }
                else
                {
                    // Manejar la situación en la que la eliminación no fue exitosa
                    // Puedes mostrar un mensaje de error o tomar otra acción apropiada
                    ViewBag.ErrorMessage = "La eliminación de la categoría no fue exitosa.";
                    return View("Error");
                }
            }
        }


    }
}
