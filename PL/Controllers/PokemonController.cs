using Microsoft.AspNetCore.Mvc;
using ML;
using Newtonsoft.Json.Linq;
using System.Reflection.Metadata;
using System.Security.Policy;

namespace PL.Controllers
{
    public class PokemonController : Controller
    {
        //Inyeccion de dependencias-- patron de diseño
        private readonly IConfiguration _configuration;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public PokemonController(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]

        public ActionResult GetPokemon()
        {
            ML.Pokemon Pokemon = new ML.Pokemon();

            using (var client = new HttpClient())
            {
                string urlApi = _configuration["urlApi"];
                client.BaseAddress = new Uri(urlApi);

                var responseTask = client.GetAsync("pokemon");
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    dynamic resultJSON = JObject.Parse(readTask.Result.ToString());
                    readTask.Wait();

                    Pokemon.Pokemones = new List<object>();//LISTA DE POKEMONES

                    foreach (var resultItem in resultJSON.results)
                    {
                        ML.Pokemon pokemon = new ML.Pokemon();

                        pokemon.Nombre = resultItem["name"];
                        string url = resultItem["url"];
                        pokemon.url = url;

                        ML.Pokemon PokemonUn = new ML.Pokemon();
                        PokemonUn = CunsumirEndPoint(url);

                        pokemon.Imagen = PokemonUn.Imagen;

                        Pokemon.Pokemones.Add(pokemon);

                    }

                }
            }
            return View(Pokemon);
        }
        [HttpGet]
        public ActionResult DetallesPokemon(string url)
        {
            ML.Pokemon pokemon = new ML.Pokemon();

            if (url != null)
            {
                pokemon = Caracteristicas(url);
                return View(pokemon);
            }
            else
            {

                return View(pokemon);
            }

        }

        [HttpGet]
        public ActionResult GetPokemonTipo(string urlTipo)
        {
            ML.Pokemon Pokemon = new ML.Pokemon();

            using (var client = new HttpClient())
            {
                string urlApi = _configuration["urlApi"];
                client.BaseAddress = new Uri(urlApi);

                var responseTask = client.GetAsync(urlTipo);
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    dynamic resultJSON = JObject.Parse(readTask.Result.ToString());
                    readTask.Wait();

                    Pokemon.Pokemones = new List<object>();//LISTA DE POKEMONES

                    var listDin = new List<dynamic>();
                    listDin.Add(resultJSON);

                    foreach (var resultItem in listDin.ToList())
                    {
                        ML.Pokemon pokemon = new ML.Pokemon();

                        foreach (var pokemonSimilar in resultItem.pokemon)
                        {
                            ML.Pokemon pokemonPorTipo = new ML.Pokemon();   

                            pokemonPorTipo.Nombre = pokemonSimilar.pokemon.name;
                            string urlPokemonPorTipo = pokemonSimilar.pokemon.url;
                            pokemonPorTipo.url = urlPokemonPorTipo;

                            ML.Pokemon PokemonUn = new ML.Pokemon();
                            PokemonUn = CunsumirEndPoint(urlPokemonPorTipo);

                            pokemonPorTipo.Imagen = PokemonUn.Imagen;



                            Pokemon.Pokemones.Add(pokemonPorTipo);
                        }

                    }

                }
            }
            return View(Pokemon);
        }

        public ML.Pokemon CunsumirEndPoint(string url)
        {
            ML.Pokemon pokemonConsumoEnd = new ML.Pokemon();

            using (var client = new HttpClient())
            {
                string urlApi = _configuration["urlApi"];
                client.BaseAddress = new Uri(urlApi);

                var responseTask = client.GetAsync(url);
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    dynamic resultJSON = JObject.Parse(readTask.Result.ToString());
                    readTask.Wait();

                    var listDin = new List<dynamic>();
                    listDin.Add(resultJSON);

                    foreach (var resultItem in listDin.ToList())
                    {
                        pokemonConsumoEnd.Imagen = resultItem.sprites.other.home.front_default;

                    }
                }
            }
            return pokemonConsumoEnd;
        }

        public ML.Pokemon GetPokemonPorTipo(string url)
        {
            ML.Pokemon pokemonConsumoEnd = new ML.Pokemon();

            using (var client = new HttpClient())
            {
                string urlApi = _configuration["urlApi"];
                client.BaseAddress = new Uri(urlApi);

                var responseTask = client.GetAsync(url);
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    dynamic resultJSON = JObject.Parse(readTask.Result.ToString());
                    readTask.Wait();

                    var listDin = new List<dynamic>();
                    listDin.Add(resultJSON);

                    foreach (var resultItem in listDin.ToList())
                    {
                        pokemonConsumoEnd.Imagen = resultItem.sprites.other.home.front_default;

                    }
                }
            }
            return pokemonConsumoEnd;
        }


        public ML.Pokemon Caracteristicas(string url)
        {
            ML.Pokemon pokemonConsumoEnd = new ML.Pokemon();

            using (var client = new HttpClient())
            {
                string urlApi = _configuration["urlApi"];
                client.BaseAddress = new Uri(urlApi);

                var responseTask = client.GetAsync(url);
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    dynamic resultJSON = JObject.Parse(readTask.Result.ToString());
                    readTask.Wait();

                    var listDin = new List<dynamic>();
                    listDin.Add(resultJSON);

                    foreach (var resultItem in listDin.ToList())
                    {
                        pokemonConsumoEnd.Imagen = resultItem.sprites.other.home.front_default;
                        pokemonConsumoEnd.Nombre = resultItem.name;
                        pokemonConsumoEnd.Experiencia = resultItem.base_experience;

                        //Listado de habilidades
                        foreach (var abilities in resultItem.abilities)
                        {
                            pokemonConsumoEnd.Habilidad = abilities.ability.name;
                        }
                        //Listado de tipos
                        foreach (var Tipos in resultItem.types)
                        {
                            pokemonConsumoEnd.Tipo = Tipos.type.name;
                            pokemonConsumoEnd.TipoUrl = Tipos.type.url;
                        }

                    }
                }
            }
            return pokemonConsumoEnd;
        }


    }
}
