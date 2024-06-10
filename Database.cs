using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CST_EMI_Shield_Wizard
{
    // Existing classes
    public class Project
    {
        public string ProjectName { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }

    public class LayerData
    {
        public string LayerName { get; set; }
        public string Material { get; set; }
        public string MinCoordinates { get; set; }
        public string MaxCoordinates { get; set; }
    }

    // New Entity Framework classes
    public class Экран
    {
        [Key]
        public int НомерЭкрана { get; set; }
        public string Название { get; set; }
    }

    public class Слои
    {
        [Key, Column(Order = 0)]
        public int НомерСлоя { get; set; }

        [Key, Column(Order = 1)]
        public int НомерЭкрана { get; set; }

        public string НазваниеСлоя { get; set; }
        public double XMin { get; set; }
        public double XMax { get; set; }
        public double YMax { get; set; }
        public double YMin { get; set; }
        public double ZMax { get; set; }
        public double ZMin { get; set; }
        public int Материал { get; set; }
    }

    public class Проект
    {
        [Key]
        public int НомерПроекта { get; set; }

        [Required]
        public DateTime ДатаСоздания { get; set; }

        [Required]
        public DateTime ДатаРедактирования { get; set; }

        public string Название { get; set; }
        public int НомерЭкрана { get; set; }
        public int НомерВоздействия { get; set; }
    }

    public class Результаты
    {
        [Key]
        public int Номер { get; set; }
        public string Название { get; set; }
        public double Результат { get; set; }
    }

    public class Воздействие
    {
        [Key]
        public int НомерВоздействия { get; set; }
        public string НазваниеВоздействия { get; set; }
        public double ЧастотаЭМПоля { get; set; }
        public bool ГраничноеУсловиеXMin { get; set; }
        public bool ГраничноеУсловиеXMax { get; set; }
        public bool ГраничноеУсловиеYMin { get; set; }
        public bool ГраничноеУсловиеYMax { get; set; }
        public bool ГраничноеУсловиеZMax { get; set; }
        public bool ГраничноеУсловиеZMin { get; set; }
        public double НормальРаспространенияX { get; set; }
        public double НормальРаспространенияY { get; set; }
        public double НормальРаспространенияZ { get; set; }
        public double ВекторЭлектрическогоПоляX { get; set; }
        public double ВекторЭлектрическогоПоляY { get; set; }
        public double ВекторЭлектрическогоПоляZ { get; set; }
        public int ОриентацияДатчикаHE { get; set; }
        public double ПозицияДатчикаX { get; set; }
        public double ПозицияДатчикаY { get; set; }
        public double ПозицияДатчикаZ { get; set; }
    }

    public class MyDbContext : DbContext
    {
        public DbSet<Экран> Экраны { get; set; }
        public DbSet<Слои> Слои { get; set; }
        public DbSet<Проект> Проекты { get; set; }
        public DbSet<Результаты> Результаты { get; set; }
        public DbSet<Воздействие> Воздействия { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string relativePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "БазаДанных.db");
            optionsBuilder.UseSqlite($"Data Source={relativePath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure composite key for Слои table
            modelBuilder.Entity<Слои>()
                .HasKey(s => new { s.НомерСлоя, s.НомерЭкрана });
        }
    }
}