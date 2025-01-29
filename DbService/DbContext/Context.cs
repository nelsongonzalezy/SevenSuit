using DbService.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DbService
{
    public class Context : DbContext
    {
        public DbSet<Users> Usuarios { get; set; }
        public DbSet<SeveEstadoCivil> EstadoCivil { get; set; }
        public DbSet<SeveClie> Clientes { get; set; }

        public Context(DbContextOptions<Context> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("SevenSuiteTest"));
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Mail).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(200);
            });

            modelBuilder.Entity<SeveEstadoCivil>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<SeveClie>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Cedula).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Genero).IsRequired();
                entity.Property(e => e.Fecha_nac).IsRequired();
                entity.Property(e => e.Id_EstadoCivil).IsRequired();
      
                entity.HasOne(c => c.EstadoCivil)
                      .WithMany()
                      .HasForeignKey(c => c.Id_EstadoCivil)
                      .IsRequired();

            });
        }

        public void Seeder(ILogger logger)
        {
            if (!EstadoCivil.Any())
            {
                logger.LogInformation("Iniciando la carga de datos de EstadoCivil...");

                var estadoCivilData = new List<SeveEstadoCivil>
                {
                    new SeveEstadoCivil { Id = Guid.Parse("11294F7E-D514-4DF2-8685-5CA3293FC2F2"), Descripcion = "Casado" },
                    new SeveEstadoCivil { Id = Guid.Parse("CDA16D01-25BD-4133-9640-FC7F5A4A2B53"), Descripcion = "Soltero" }
                };

                EstadoCivil.AddRange(estadoCivilData);
                SaveChanges();
                logger.LogInformation("Datos de EstadoCivil cargados exitosamente.");
            }

            if (!Clientes.Any())
            {
                logger.LogInformation("No se encontraron clientes en la base de datos. Iniciando la carga de datos de seeder.");

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Seeder", "Clientes.json");
                var jsonData = File.ReadAllText(filepath);
                var clientesSeeder = JsonSerializer.Deserialize<List<SeveClie>>(jsonData);

                if (clientesSeeder != null && clientesSeeder.Any())
                {
                    var estadoCivilMap = EstadoCivil.ToDictionary(e => e.Descripcion, e => e.Id);

                    foreach (var cliente in clientesSeeder)
                    {
                        if (cliente.Id_EstadoCivil == Guid.Empty && estadoCivilMap.TryGetValue("Casado", out var estadoCivilId))
                        {
                            cliente.Id_EstadoCivil = estadoCivilId;
                        }
                    }

                    Clientes.AddRange(clientesSeeder);
                    SaveChanges();
                    logger.LogInformation("Datos de seeder cargados exitosamente.");
                }
                else
                {
                    logger.LogWarning("No se encontraron datos en el archivo de seeder.");
                }
            }
            else
            {
                logger.LogInformation("Ya existen clientes en la base de datos. No se necesita cargar datos de seeder.");
            }
            if (!Usuarios.Any())
            {
                logger.LogInformation("Iniciando la carga de datos de Usuarios...");

                var usuariosData = new List<Users>
        {
            new Users { Mail = "test@mail.com", Password = "Test23" } // Puedes agregar más usuarios si es necesario
        };

                Usuarios.AddRange(usuariosData);
                SaveChanges();
                logger.LogInformation("Datos de Usuarios cargados exitosamente.");
            }
            else
            {
                logger.LogInformation("Ya existen usuarios en la base de datos. No se necesita cargar datos de seeder.");
            }
        }
    }
}

