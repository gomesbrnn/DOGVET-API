using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Clinica> Clinicas { get; set; }
        public DbSet<Tutor> Tutores { get; set; }
        public DbSet<Animal> Animais { get; set; }
        public DbSet<Veterinario> Veterinarios { get; set; }
        public DbSet<Atendimento> Atendimentos { get; set; }
        public DbSet<Autenticacao> Autenticacoes { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

    }
}