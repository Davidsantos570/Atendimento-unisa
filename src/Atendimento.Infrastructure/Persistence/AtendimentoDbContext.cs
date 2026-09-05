using Atendimento.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Atendimento.Infrastructure.Persistence;

public class AtendimentoDbContext : DbContext
{
    public AtendimentoDbContext(DbContextOptions<AtendimentoDbContext> options) : base(options) { }

    public DbSet<Aluno> Alunos => Set<Aluno>();
    public DbSet<Atendente> Atendentes => Set<Atendente>();
    public DbSet<Chamado> Chamados => Set<Chamado>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aluno>(builder =>
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Nome).IsRequired().HasMaxLength(200);
            builder.Property(a => a.Email).IsRequired().HasMaxLength(200);
            builder.Property(a => a.Matricula).IsRequired().HasMaxLength(50);
            builder.Property(a => a.SenhaHash).IsRequired().HasMaxLength(200);
            builder.HasIndex(a => a.Email).IsUnique();
        });

        modelBuilder.Entity<Atendente>(builder =>
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Nome).IsRequired().HasMaxLength(200);
            builder.Property(a => a.Email).IsRequired().HasMaxLength(200);
            builder.Property(a => a.Setor).IsRequired().HasMaxLength(100);
            builder.Property(a => a.SenhaHash).IsRequired().HasMaxLength(200);
            builder.HasIndex(a => a.Email).IsUnique();
        });

        modelBuilder.Entity<Chamado>(builder =>
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Titulo).IsRequired().HasMaxLength(200);
            builder.Property(c => c.Descricao).IsRequired().HasMaxLength(2000);

            builder.OwnsMany(c => c.Mensagens, mensagem =>
            {
                mensagem.ToTable("ChamadoMensagens");
                mensagem.WithOwner().HasForeignKey("ChamadoId");
                mensagem.Property(m => m.Autor).IsRequired().HasMaxLength(100);
                mensagem.Property(m => m.Texto).IsRequired().HasMaxLength(4000);
                mensagem.HasKey("Id");
            });
        });
    }
}
