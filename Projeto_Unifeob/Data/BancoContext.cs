using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_Unifeob.Models;
using System.Collections.Generic;

namespace Projeto_Unifeob.Data {
    public class BancoContext : DbContext {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options) {
        }
        public DbSet<Contribuinte_Model> Contribuinte { get; set; }
        public DbSet<Beneficio_Model> Beneficio { get; set; }
        public DbSet<ContribuinteBeneficio_Model> ContribuinteBeneficio { get; set; }
    }
}
