using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwaitBestPracticesWithCataas
{
    class Program
    {
        // Instancia compartida de HttpClient para eficiencia
        private static readonly HttpClient HttpClient = new HttpClient();

        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Demostración de Mejores Prácticas de Async/Await con CATAAS ===");

            // Ejemplo 1: Uso de async Task en lugar de async void
            await GetCatImageAsync();
            Console.WriteLine();

            // Ejemplo 2: Propagación de la asincronía
            await DisplayCatImageAsync();
            Console.WriteLine();

            // Ejemplo 3: Uso de Task.WhenAll para operaciones paralelas
            await FetchMultipleCatsAsync(new[] { "cute", "funny", "grumpy" });
            Console.WriteLine();

            // Ejemplo 4: Evitar bloqueos con .Result o .Wait()
            await GetCatImageDataAsync();
            Console.WriteLine();

            // Ejemplo 5: Manejo de excepciones adecuadamente
            await TryFetchCatImageAsync();
            Console.WriteLine();

            // Ejemplo 6: Uso de CancellationToken para operaciones cancelables
            await FetchCatWithTimeoutAsync();
            Console.WriteLine();

            // Ejemplo 7: Optimizar para operaciones frecuentes
            await GetCachedCatImageAsync();
            Console.WriteLine();

            // Ejemplo 8: Nombrar adecuadamente los métodos asíncronos
            await DownloadCatImageAsync();
            Console.WriteLine();

            // Ejemplo 9: Usar ConfigureAwait(false) cuando sea apropiado
            await FetchCatForLibraryAsync();
            Console.WriteLine();

            // Ejemplo 10: Evitar async/await innecesario
            await GetCatUrlBetterAsync();
            Console.WriteLine();

            // Ejemplo 11: Probar el código asíncrono adecuadamente
            await TestDownloadCatImageAsync();
            Console.WriteLine();

            Console.WriteLine("=== Fin de la Demostración ===");
        }

        // Ejemplo 1: Uso de async Task en lugar de async void
        public static async Task GetCatImageAsync()
        {
            Console.WriteLine("Ejemplo 1: Obteniendo imagen de gato...");
            var response = await HttpClient.GetAsync("https://cataas.com/cat");
            Console.WriteLine("¡Imagen de gato obtenida con éxito!");
        }

        // Ejemplo 2: Propagación de la asincronía
        public static async Task DisplayCatImageAsync()
        {
            Console.WriteLine("Ejemplo 2: Propagando asincronía...");
            await FetchCatImageAsync();
        }

        private static async Task FetchCatImageAsync()
        {
            var response = await HttpClient.GetAsync("https://cataas.com/cat");
            Console.WriteLine("¡Gato listo para mostrar!");
        }

        // Ejemplo 3: Uso de Task.WhenAll para operaciones paralelas
        public static async Task FetchMultipleCatsAsync(IEnumerable<string> catTags)
        {
            Console.WriteLine("Ejemplo 3: Obteniendo múltiples imágenes de gatos en paralelo...");
            var tasks = new List<Task>();
            foreach (var tag in catTags)
            {
                tasks.Add(FetchCatWithTagAsync(tag));
            }
            await Task.WhenAll(tasks);
            Console.WriteLine("¡Todas las imágenes de gatos obtenidas!");
        }

        private static async Task FetchCatWithTagAsync(string tag)
        {
            var response = await HttpClient.GetAsync($"https://cataas.com/cat/{tag}");
            Console.WriteLine($"Gato con tag '{tag}' obtenido");
        }

        // Ejemplo 4: Evitar bloqueos con .Result o .Wait()
        public static async Task GetCatImageDataAsync()
        {
            Console.WriteLine("Ejemplo 4: Obteniendo datos de imagen de gato...");
            var response = await HttpClient.GetAsync("https://cataas.com/cat");
            var content = await response.Content.ReadAsByteArrayAsync();
            Console.WriteLine($"Datos de imagen obtenidos: {content.Length} bytes");
        }

        // Ejemplo 5: Manejo de excepciones adecuadamente
        public static async Task TryFetchCatImageAsync()
        {
            Console.WriteLine("Ejemplo 5: Intentando obtener imagen de gato...");
            try
            {
                var response = await HttpClient.GetAsync("https://cataas.com/cat/invalid"); // URL inválida
                response.EnsureSuccessStatusCode();
                Console.WriteLine("¡Gato obtenido con éxito!");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al obtener el gato: {ex.Message}");
            }
        }

        // Ejemplo 6: Uso de CancellationToken para operaciones cancelables
        public static async Task FetchCatWithTimeoutAsync()
        {
            Console.WriteLine("Ejemplo 6: Obteniendo gato con posibilidad de cancelación...");
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2)))
            {
                try
                {
                    var response = await HttpClient.GetAsync("https://cataas.com/cat", cts.Token);
                    Console.WriteLine("¡Gato obtenido antes de que se cancele!");
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("La solicitud fue cancelada.");
                }
            }
        }

        // Ejemplo 7: Optimizar para operaciones frecuentes con caché
        private static byte[]? cachedCatImage = null;

        public static async Task GetCachedCatImageAsync()
        {
            Console.WriteLine("Ejemplo 7: Obteniendo imagen de gato desde caché o API...");
            if (cachedCatImage != null)
            {
                Console.WriteLine("Imagen obtenida desde caché.");
                return;
            }
            var response = await HttpClient.GetAsync("https://cataas.com/cat");
            cachedCatImage = await response.Content.ReadAsByteArrayAsync();
            Console.WriteLine("Imagen obtenida desde API y almacenada en caché.");
        }

        // Ejemplo 8: Nombrar adecuadamente los métodos asíncronos
        public static async Task DownloadCatImageAsync()
        {
            Console.WriteLine("Ejemplo 8: Descargando imagen de gato...");
            var response = await HttpClient.GetAsync("https://cataas.com/cat");
            Console.WriteLine("¡Imagen de gato descargada!");
        }

        // Ejemplo 9: Usar ConfigureAwait(false) cuando sea apropiado
        public static async Task FetchCatForLibraryAsync()
        {
            Console.WriteLine("Ejemplo 9: Obteniendo gato para biblioteca sin contexto...");
            var response = await HttpClient.GetAsync("https://cataas.com/cat").ConfigureAwait(false);
            var content = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            Console.WriteLine($"Imagen obtenida: {content.Length} bytes");
        }

        // Ejemplo 10: Evitar async/await innecesario
        public static Task<string> GetCatUrlBetterAsync()
        {
            Console.WriteLine("Ejemplo 10: Devolviendo URL de gato sin async/await innecesario...");
            return Task.FromResult("https://cataas.com/cat");
        }

        // Ejemplo 11: Probar el código asíncrono adecuadamente
        public static async Task TestDownloadCatImageAsync()
        {
            Console.WriteLine("Ejemplo 11: Probando la descarga de imagen de gato...");
            var response = await HttpClient.GetAsync("https://cataas.com/cat");
            var content = await response.Content.ReadAsByteArrayAsync();
            if (content.Length > 0)
            {
                Console.WriteLine("Prueba exitosa: La imagen no está vacía.");
            }
            else
            {
                Console.WriteLine("Prueba fallida: La imagen está vacía.");
            }
        }
    }
}