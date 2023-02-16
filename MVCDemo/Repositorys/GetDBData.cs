using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MVCDemo.Repositorys
{
	public class StudentData: DbContext
	{
		public StudentData(DbContextOptions<StudentData> options): base(options)
		{
		}
		/*
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Models.DBDataModel.Student>()
				.HasKey(e => new { e.UniqueID });
		}
		*/
		public DbSet<Models.DBDataModel.Student> Student { get; set; }
	}

	public class ClassData : DbContext
	{
        public ClassData(DbContextOptions<ClassData> options) : base(options)
        {
        }
        /*
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Models.DBDataModel.Student>()
				.HasKey(e => new { e.UniqueID });
		}
		*/
        public DbSet<Models.DBDataModel.Class> Class { get; set; }
    }

    public class SelectClassData : DbContext
    {
        public SelectClassData(DbContextOptions<SelectClassData> options) : base(options)
        {
        }
        
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Models.DBDataModel.SelectClass>()
				.HasKey(e => new { e.StudentNum });
		}
		
        public DbSet<Models.DBDataModel.SelectClass> SelectClass { get; set; }
    }
}

