using ApplicationWPF.Models;
using Microsoft.EntityFrameworkCore;


namespace ApplicationWPF
{
	public class MyDbContext : DbContext
	{
		public DbSet<LOGPAS> LOGPAS { get; set; }
		public DbSet<CLIENTS> CLIENTS { get; set; }
		public DbSet<EMPLOYEES> EMPLOYEES { get; set; }
		public DbSet<ABONEMENTS> ABONEMENTS { get; set; }
		public DbSet<SHEDULE_TABLE> SHEDULE_TABLE { get; set; }
		public DbSet<SALEOFABONEMENTS> SALEOFABONEMENTS { get; set; }
		public DbSet<ABONEMENT_REQUEST> ABONEMENT_REQUEST { get; set; }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Data Source=DESKTOP-6EJJG7F;Initial Catalog=publ;Integrated Security=True");
		}
	}
}
