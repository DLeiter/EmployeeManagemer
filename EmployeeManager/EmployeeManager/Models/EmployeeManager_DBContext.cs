using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EmployeeManager.Models
{
	public partial class EmployeeManager_DBContext : DbContext
	{
		public EmployeeManager_DBContext(DbContextOptions<EmployeeManager_DBContext> options)
			: base(options)
		{
		}

		public void UpdateActivityLogAsync/*(string action, string details, string affectedTable)//*/(ActionEnum action, string details, Type affectedTable)
		{
			int maxNum = 0;
			try
			{
				maxNum = ActivityLog.Select(x => int.Parse(x.Id)).Max();
			}
			catch (Exception)
			{
				//Use this for logging in the future
				throw;
			}
			ActivityLog.Add(new ActivityLog()
			{
				Id = $"{++maxNum}",
				Action = action.ToString(),
				Details = details,
				AffectedTable = affectedTable.Name.Replace("Controller", ""),
				AddDate = DateTime.UtcNow
			}) ;

			SaveChanges();
			//await SaveChangesAsync();
		}

		public virtual DbSet<ActivityLog> ActivityLog { get; set; }
		public virtual DbSet<Departments> Departments { get; set; }
		public virtual DbSet<Employees> Employees { get; set; }
		public virtual DbSet<Permissions> Permissions { get; set; }
		public virtual DbSet<Photos> Photos { get; set; }
		public virtual DbSet<Positions> Positions { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured){}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ActivityLog>(entity =>
			{
				entity.Property(e => e.Id)
					.HasColumnName("ID")
					.HasMaxLength(10)
					.ValueGeneratedNever();

				entity.Property(e => e.Action)
					.IsRequired()
					.HasMaxLength(50);

				entity.Property(e => e.AddDate).HasColumnType("datetime");

				entity.Property(e => e.AffectedTable).HasMaxLength(50);

				entity.Property(e => e.Details)
					.IsRequired()
					.HasMaxLength(500);
			});

			modelBuilder.Entity<Departments>(entity =>
			{
				entity.Property(e => e.Id)
					.HasColumnName("ID")
					.HasMaxLength(10)
					.ValueGeneratedNever();

				entity.Property(e => e.AddDate).HasColumnType("datetime");

				entity.Property(e => e.Department)
					.IsRequired()
					.HasMaxLength(50);

				entity.Property(e => e.LastModified).HasColumnType("datetime");

				entity.Property(e => e.Permissions).HasMaxLength(100);
			});

			modelBuilder.Entity<Employees>(entity =>
			{
				entity.Property(e => e.Id)
					.HasColumnName("ID")
					.HasMaxLength(10)
					.ValueGeneratedNever();

				entity.Property(e => e.Address).HasMaxLength(50);

				entity.Property(e => e.DepartmentId)
					.HasColumnName("DepartmentID")
					.HasMaxLength(10);

				entity.Property(e => e.Email).HasMaxLength(50);

				entity.Property(e => e.EndDate).HasColumnType("datetime");

				entity.Property(e => e.ExtraPermissions).HasMaxLength(100);

				entity.Property(e => e.FavoriteColor).HasMaxLength(50);

				entity.Property(e => e.FirstName)
					.IsRequired()
					.HasMaxLength(50);

				entity.Property(e => e.LastName)
					.IsRequired()
					.HasMaxLength(50);

				entity.Property(e => e.ManagerId)
					.HasColumnName("ManagerID")
					.HasMaxLength(10);

				entity.Property(e => e.MiddleName).HasMaxLength(50);

				entity.Property(e => e.PhoneNumber).HasMaxLength(50);

				entity.Property(e => e.PhotoId)
					.HasColumnName("PhotoID")
					.HasMaxLength(10);

				entity.Property(e => e.PositionId)
					.HasColumnName("PositionID")
					.HasMaxLength(10);

				entity.Property(e => e.Shift)
					.IsRequired()
					.HasMaxLength(100);

				entity.Property(e => e.StartDate).HasColumnType("datetime");

				entity.Property(e => e.TerminationNotes).HasMaxLength(500);

				entity.HasOne(d => d.Department)
					.WithMany(p => p.Employees)
					.HasForeignKey(d => d.DepartmentId)
					.HasConstraintName("FK_Employees_Departments");

				entity.HasOne(d => d.Manager)
					.WithMany(p => p.InverseManager)
					.HasForeignKey(d => d.ManagerId)
					.HasConstraintName("FK_Employees_Employees1");

				entity.HasOne(d => d.Position)
					.WithMany(p => p.Employees)
					.HasForeignKey(d => d.PositionId)
					.HasConstraintName("FK_Employees_Positions");
			});

			modelBuilder.Entity<Permissions>(entity =>
			{
				entity.Property(e => e.Id)
					.HasColumnName("ID")
					.HasMaxLength(10)
					.ValueGeneratedNever();

				entity.Property(e => e.AddDate).HasColumnType("datetime");

				entity.Property(e => e.Code)
					.IsRequired()
					.HasMaxLength(10);

				entity.Property(e => e.Details).HasMaxLength(500);

				entity.Property(e => e.LastModified).HasColumnType("datetime");

				entity.Property(e => e.Permission)
					.IsRequired()
					.HasMaxLength(100);
			});

			modelBuilder.Entity<Photos>(entity =>
			{
				entity.Property(e => e.Id)
					.HasColumnName("ID")
					.HasMaxLength(10)
					.ValueGeneratedNever();

				entity.Property(e => e.Photo)
					.IsRequired()
					.HasColumnType("image");

				entity.Property(e => e.Title)
					.IsRequired()
					.HasMaxLength(50);
			});

			modelBuilder.Entity<Positions>(entity =>
			{
				entity.Property(e => e.Id)
					.HasColumnName("ID")
					.HasMaxLength(10)
					.ValueGeneratedNever();

				entity.Property(e => e.AddDate).HasColumnType("datetime");

				entity.Property(e => e.Details).HasMaxLength(50);

				entity.Property(e => e.DpeartmentId)
					.IsRequired()
					.HasColumnName("DpeartmentID")
					.HasMaxLength(10);

				entity.Property(e => e.LastModified).HasColumnType("datetime");

				entity.Property(e => e.Position)
					.IsRequired()
					.HasMaxLength(50);
			});
		}
	}
}
