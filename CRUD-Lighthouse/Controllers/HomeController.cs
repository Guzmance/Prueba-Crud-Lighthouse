using CRUD_Lighthouse.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_Lighthouse.Controllers
{
    public class HomeController : Controller
    {
        // URL base del servicio web API REST
        string Baseurl = "https://api.escuelajs.co/api/v1/";

        // M�todo para obtener todas las categor�as
        public async Task<ActionResult> Index()
        {
            List<Category> categoryInfo = new List<Category>();
            using (var client = new HttpClient())
            {
                // Configurar la base URL y los encabezados de la solicitud HTTP
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Realizar la solicitud HTTP para obtener todas las categor�as
                HttpResponseMessage response = await client.GetAsync("categories");

                // Verificar si la solicitud fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Leer la respuesta JSON y deserializarla en una lista de categor�as
                    var categoryResponse = await response.Content.ReadAsStringAsync();
                    categoryInfo = JsonConvert.DeserializeObject<List<Category>>(categoryResponse);
                }

                // Devolver la lista de categor�as a la vista
                return View(categoryInfo);
            }
        }

        // M�todo para obtener una categor�a individual por ID
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

        // M�todo para seleccionar eliminar una categor�a por ID
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

                    // Redirigir a la vista de confirmaci�n con los detalles de la categor�a
                    return View("Delete", category);
                }
                else
                {
                    // Manejar la situaci�n en la que la solicitud al servidor no fue exitosa
                    ViewBag.ErrorMessage = "Hubo un error al intentar obtener los detalles de la categor�a.";
                    return View("Error");
                }
            }
        }

        // M�todo para confirmar eliminaci�n de una categor�a por ID
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
                    // Redirigir a la p�gina principal u otra vista apropiada despu�s de la eliminaci�n
                    return RedirectToAction("Index");
                }
                else
                {
                    // Manejar la situaci�n en la que la eliminaci�n no fue exitosa
                    ViewBag.ErrorMessage = "La eliminaci�n de la categor�a no fue exitosa.";
                    return View("Error");
                }
            }
        }

        // M�todo para abrir vista crear una categor�a
        public ActionResult Create()
        {
            return View();
        }

        // M�todo para la creaci�n de una categor�a 
        [HttpPost]
        public ActionResult Create(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Crear un objeto JSON con los datos de la categor�a
                    var categoryData = new
                    {
                        name = category.Name,
                        image = category.Image
                    };

                    // Serializar el objeto JSON
                    var jsonCategoryData = JsonConvert.SerializeObject(categoryData);

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Baseurl);
                        // Establecer el tipo de contenido de la solicitud como JSON
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        // Crear la solicitud POST con el contenido JSON
                        var postTask = client.PostAsync("categories/", new StringContent(jsonCategoryData, Encoding.UTF8, "application/json"));
                        postTask.Wait();

                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "API Error: " + result.ReasonPhrase);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred: " + ex.Message);
            }

            return View(category);
        }

        // M�todo para seleccionar Editar o Actualizar una categor�a por ID
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Realizar la solicitud HTTP para obtener los detalles de una categor�a por su ID
                    HttpResponseMessage Res = await client.GetAsync($"categories/{id}");

                    // Verificar si la solicitud fue exitosa
                    if (Res.IsSuccessStatusCode)
                    {
                        // Leer la respuesta JSON y deserializarla en un objeto de categor�a
                        var categoryResponse = await Res.Content.ReadAsStringAsync();
                        var category = JsonConvert.DeserializeObject<Category>(categoryResponse);

                        // Devolver la vista Edit con los detalles de la categor�a
                        return View(category);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "API Error: " + Res.ReasonPhrase);
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred: " + ex.Message);
                return View();
            }
        }

        // M�todo para Editar o Actualizar una categor�a por ID
        [HttpPost]
        public async Task<ActionResult> Edit(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Crear un objeto JSON con los datos de la categor�a
                    var categoryData = new
                    {
                        id = category.Id,
                        name = category.Name,
                        image = category.Image
                    };

                    // Serializar el objeto de categor�a en formato JSON
                    var jsonCategory = JsonConvert.SerializeObject(categoryData);
                    var content = new StringContent(jsonCategory, Encoding.UTF8, "application/json");

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Baseurl);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        // Realizar la solicitud HTTP para actualizar los detalles de una categor�a
                        HttpResponseMessage Res = await client.PutAsync($"categories/{category.Id}", content);

                        // Verificar si la solicitud fue exitosa
                        if (Res.IsSuccessStatusCode)
                        {
                            // Redirigir a la p�gina Index despu�s de editar la categor�a
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "API Error: " + Res.ReasonPhrase);
                            return View(category);
                        }
                    }
                }
                else
                {
                    return View(category);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred: " + ex.Message);
                return View(category);
            }
        }
    }
}

