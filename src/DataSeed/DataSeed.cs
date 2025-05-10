using click_imoveis.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

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

                var jsonFileUsuarios = Path.Combine(AppContext.BaseDirectory, "DataSeed", "usuarios.json");
                var jsonDataUsuarios = await File.ReadAllTextAsync(jsonFileUsuarios);
                List<Usuario>? usuarios = JsonSerializer.Deserialize<List<Usuario>>(jsonDataUsuarios);

                var jsonFileImoveis = Path.Combine(AppContext.BaseDirectory, "DataSeed", "imoveis.json");
                var jsonDataImoveis = await File.ReadAllTextAsync(jsonFileImoveis);
                List<Imovel>? imoveis = JsonSerializer.Deserialize<List<Imovel>>(jsonDataImoveis);

                var jsonFileAnuncios = Path.Combine(AppContext.BaseDirectory, "DataSeed", "anuncios.json");
                var jsonDataAnuncios = await File.ReadAllTextAsync(jsonFileAnuncios);
                List<Anuncio>? anuncios = JsonSerializer.Deserialize<List<Anuncio>>(jsonDataAnuncios);

                Models.DataSeed entities = new Models.DataSeed { Imoveis = imoveis, Usuarios = usuarios, Anuncios = anuncios };

                if ((entities.Usuarios != null) && (entities.Imoveis != null) && (entities.Anuncios != null))
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
