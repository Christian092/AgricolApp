﻿//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProyectoV1.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class bdagricolaEntities : DbContext
    {
        public bdagricolaEntities()
            : base("name=bdagricolaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<alerta_mantencion> alerta_mantencion { get; set; }
        public DbSet<animal> animal { get; set; }
        public DbSet<comprador> comprador { get; set; }
        public DbSet<estado> estado { get; set; }
        public DbSet<fabricante> fabricante { get; set; }
        public DbSet<gps> gps { get; set; }
        public DbSet<historial_gps> historial_gps { get; set; }
        public DbSet<inseminacion> inseminacion { get; set; }
        public DbSet<inseminador> inseminador { get; set; }
        public DbSet<lecheria> lecheria { get; set; }
        public DbSet<medicamento> medicamento { get; set; }
        public DbSet<muerte> muerte { get; set; }
        public DbSet<nace_muerto> nace_muerto { get; set; }
        public DbSet<ordena> ordena { get; set; }
        public DbSet<parto> parto { get; set; }
        public DbSet<poligono> poligono { get; set; }
        public DbSet<procedencia> procedencia { get; set; }
        public DbSet<procedencia_semen> procedencia_semen { get; set; }
        public DbSet<raza> raza { get; set; }
        public DbSet<secamiento> secamiento { get; set; }
        public DbSet<tipo> tipo { get; set; }
        public DbSet<tipo_lecheria> tipo_lecheria { get; set; }
        public DbSet<tipo_tratamiento> tipo_tratamiento { get; set; }
        public DbSet<tratador> tratador { get; set; }
        public DbSet<tratamiento> tratamiento { get; set; }
        public DbSet<tratamiento_animal> tratamiento_animal { get; set; }
        public DbSet<usuario> usuario { get; set; }
        public DbSet<venta> venta { get; set; }
        public DbSet<vertice> vertice { get; set; }
    }
}
