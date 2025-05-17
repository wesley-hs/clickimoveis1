using Microsoft.EntityFrameworkCore;
using click_imoveis.Models;

namespace click_imoveis.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Imovel> Imoveis { get; set; }
        public DbSet<Anuncio> Anuncios { get; set; }
        public DbSet<Mensagem> Mensagens { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Midia> Midias { get; set; }
        public DbSet<PessoaFisica> PessoasFisicas { get; set; }
        public DbSet<PessoaJuridica> PessoasJuridicas { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // CLASSE IMÓVEL

            modelBuilder.Entity<Imovel>()
               .HasMany(u => u.Anuncios)
               .WithOne(i => i.Imovel)
               .HasForeignKey(i => i.ImovelId)
               .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Imovel>()
                .HasMany(u => u.Midias)
                .WithOne(u => u.Imovel)
                .HasForeignKey(u => u.ImovelId)
                .OnDelete(DeleteBehavior.SetNull);

            
            modelBuilder.Entity<Mensagem>()
                .HasOne(e => e.Usuario)
                .WithMany(g => g.Mensagens)
                .HasForeignKey(s => s.UsuarioId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Anuncio>()
                .HasOne(e => e.Usuario)
                .WithMany(g => g.Anuncios)
                .HasForeignKey(s => s.UsuarioId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.PessoaFisica)
                .WithOne(pf => pf.Usuario)
                .HasForeignKey<PessoaFisica>(pf => pf.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.PessoaJuridica)
                .WithOne(pj => pj.Usuario)
                .HasForeignKey<PessoaJuridica>(pj => pj.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Imoveis)
                .WithOne(i => i.Usuario)
                .HasForeignKey(i => i.UsuarioId)
                .OnDelete(DeleteBehavior.SetNull);

           
        }
    }
}
