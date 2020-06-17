using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {

        }

        public DbSet<University> Universities { get; set; }
        public DbSet<EducationalDivision> Divisions { get; set; }
        public DbSet<EducationalDirection> Directions { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SubjectScore> SubjectScores { get; set; }
        public DbSet<IdentityUserClaim<int>> UserClaims { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<IdentityRole<int>> Roles { get; set; }
        public DbSet<IdentityRoleClaim<int>> RoleClaims { get; set; }
        public DbSet<IdentityUserRole<int>> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
             modelBuilder.Entity<IdentityUserRole<int>>()
                .HasKey(new string[]{ "RoleId", "UserId"});

            modelBuilder.Entity<University>()
                .HasMany(u=>u.Divisions)
                .WithOne(d=>d.University)
                .HasForeignKey(d=>d.UniversityId);

            modelBuilder.Entity<EducationalDivision>()
                .HasOne(d=>d.University)
                .WithMany(u=>u.Divisions)
                .HasForeignKey(d=>d.UniversityId);
            
            modelBuilder.Entity<EducationalDivision>()
                .HasMany(d=>d.EducationalDirections)
                .WithOne(l=>l.EducationalDivision)
                .HasForeignKey(l=>l.EducationalDivisionId);

            modelBuilder.Entity<EducationalDirection>()
                .HasOne(dir=>dir.EducationalDivision)
                .WithMany(d=>d.EducationalDirections)
                .HasForeignKey(dir=>dir.EducationalDivisionId);
            
            modelBuilder.Entity<EducationalDirection>()
                .HasMany(dir=>dir.SubjectScores)
                .WithOne(s=>s.EducationalDirection)
                .HasForeignKey(s=>s.EducationalDirectionId);

            modelBuilder.Entity<Subject>()
                .HasMany(s=>s.SubjectScores)
                .WithOne(ss=>ss.Subject)
                .HasForeignKey(ss=>ss.SubjectId);

            modelBuilder.Entity<SubjectScore>()
                .HasOne(ss=>ss.Subject)
                .WithMany(s=>s.SubjectScores)
                .HasForeignKey(ss=>ss.SubjectId);

            modelBuilder.Entity<SubjectScore>()
                .HasOne(ss=>ss.EducationalDirection)
                .WithMany(l=>l.SubjectScores)
                .HasForeignKey(ss=>ss.EducationalDirectionId);

            var date = DateTime.Now;
            modelBuilder.Entity<Subject>().HasData(
            new Subject[] 
            {
                new Subject {Id = 1, Name="Физика", CreateDateTime = date, ModifiedDateTime = date},
                new Subject {Id = 2, Name="Химия", CreateDateTime = date, ModifiedDateTime = date},
                new Subject {Id = 3, Name="Русский язык", CreateDateTime = date, ModifiedDateTime = date},
                new Subject {Id = 4, Name="Математика(профильная)", CreateDateTime = date, ModifiedDateTime = date},
                new Subject {Id = 5, Name="Биология", CreateDateTime = date, ModifiedDateTime = date},
                new Subject {Id = 6, Name="Творческий конкурс", CreateDateTime = date, ModifiedDateTime = date},
                new Subject {Id = 7, Name="Спортивная дисциплина", CreateDateTime = date, ModifiedDateTime = date},
                new Subject {Id = 8, Name="География", CreateDateTime = date, ModifiedDateTime = date},
                new Subject {Id = 9, Name="Обществознание", CreateDateTime = date, ModifiedDateTime = date},
                new Subject {Id = 10, Name="Литература", CreateDateTime = date, ModifiedDateTime = date},
                new Subject {Id = 11, Name="История", CreateDateTime = date, ModifiedDateTime = date},
                new Subject {Id = 12, Name="Информатика", CreateDateTime = date, ModifiedDateTime = date},
                new Subject {Id = 13, Name="Иностранный язык", CreateDateTime = date, ModifiedDateTime = date}
            });
        }
    }
}
