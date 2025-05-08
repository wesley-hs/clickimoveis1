using click_imoveis.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace click_imoveis.DataSeed
{
    public class DataSeedImporter
    {
        public static async Task ImportDataAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Como estou testando os dados iniciais preciso que o banco seja destruído e recriado em toda reinicialização
            //context.Database.EnsureDeleted();

            // Garantir que o banco de dados já foi criado
            context.Database.EnsureCreated();


            if (!context.Usuarios.Any() && !context.Imoveis.Any() && !context.Anuncios.Any())
            {
                var jsonFilePath = Path.Combine(AppContext.BaseDirectory, "DataSeed", "DataSeed.json");
                var jsonData = await File.ReadAllTextAsync(jsonFilePath);

                Models.DataSeed? entities = JsonSerializer.Deserialize<Models.DataSeed>(jsonData);

                


                if (entities != null)
                {
                    await context.Usuarios.AddRangeAsync(entities.Usuarios);
                    await context.SaveChangesAsync();
                    await context.Imoveis.AddRangeAsync(entities.Imoveis);
                    await context.SaveChangesAsync();
                    await context.Anuncios.AddRangeAsync(entities.Anuncios);
                    await context.SaveChangesAsync();
                }
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("#############################################################");
                Console.WriteLine("DataSeed: Dados seed inseridos no banco de dados com sucesso.");
                Console.WriteLine("#############################################################");
                Console.WriteLine("");
                Console.WriteLine("");
            }
        }
    }
    
}
