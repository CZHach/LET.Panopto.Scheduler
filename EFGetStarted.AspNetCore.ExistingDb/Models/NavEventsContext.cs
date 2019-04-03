using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using LET.Panopto.Scheduler.Models;

namespace LET.Panopto.Scheduler.Models
{
    public partial class NavEventsContext : DbContext
    {
        public NavEventsContext()
        {
        }

        public NavEventsContext(DbContextOptions<NavEventsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<FolderList> FolderList { get; set; }
        public virtual DbSet<ModuleList> ModuleList { get; set; }
        public virtual DbSet<PageList> PageList { get; set; }
        public virtual DbSet<Groups> Groups { get; set; }
        public virtual DbSet<ModuleCurricula> ModuleCurricula { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=NavEvents;Integrated Security=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FolderList>(entity =>
            {
                entity.HasKey(e => e.FolderId);

                entity.ToTable("folderList");

                entity.Property(e => e.FolderId).HasColumnName("folderID");

                entity.Property(e => e.FolderDateTimeStart)
                    .HasColumnName("folderDateTimeStart")
                    .HasColumnType("datetime2(0)");

                entity.Property(e => e.FolderDisplayName)
                    .HasColumnName("folderDisplayName")
                    .HasMaxLength(255);

                entity.Property(e => e.FolderSequence).HasColumnName("folderSequence");

                entity.Property(e => e.HasCustomAcl).HasColumnName("hasCustomAcl");

                entity.Property(e => e.Hidden).HasColumnName("hidden");

                entity.Property(e => e.ModuleId).HasColumnName("moduleID");

                entity.Property(e => e.ShowEndDate)
                    .HasColumnName("showEndDate")
                    .HasColumnType("datetime2(0)");

                entity.Property(e => e.ShowStartDate)
                    .HasColumnName("showStartDate")
                    .HasColumnType("datetime2(0)");

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.FolderList)
                    .HasForeignKey(d => d.ModuleId)
                    .HasConstraintName("fk_moduleList_folderList_moduleID");
            });

            modelBuilder.Entity<ModuleList>(entity =>
            {
                entity.HasKey(e => e.ModuleId);

                entity.ToTable("moduleList");

                entity.Property(e => e.ModuleId).HasColumnName("moduleID");

                entity.Property(e => e.AcademicYear).HasColumnName("academicYear");

                entity.Property(e => e.IsPlaceholder).HasColumnName("isPlaceholder");

                entity.Property(e => e.PublishingStatus).HasColumnName("publishingStatus");

                entity.Property(e => e.MediasiteCatalogId)
                    .HasColumnName("mediasiteCatalogId")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ModuleDisplayName)
                    .IsRequired()
                    .HasColumnName("moduleDisplayName")
                    .HasMaxLength(255);

            });

            modelBuilder.Entity<Groups>(entity =>
            {
                entity.HasKey(e => e.GroupId);
                entity.ToTable("groups");
                entity.Property(e => e.GroupId).HasColumnName("groupID");
                entity.Property(e => e.Discriminator).HasColumnName("discriminator");
                entity.Property(e => e.ClassYear).HasColumnName("classYear");
            });

            modelBuilder.Entity<ModuleCurricula>(entity =>
            {
                entity.HasKey(e => e.ModuleId);
                entity.HasKey(e => e.GroupId);
                entity.ToTable("moduleCurricula");
                entity.Property(e => e.ModuleId).HasColumnName("moduleId");
                entity.Property(e => e.GroupId).HasColumnName("groupId");
            });

            modelBuilder.Entity<PageList>(entity =>
            {
                entity.HasKey(e => e.PageId);

                entity.ToTable("pageList");

                entity.Property(e => e.PageId).HasColumnName("pageID");

                entity.Property(e => e.BodyContent).HasColumnName("bodyContent");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("dateCreated")
                    .HasColumnType("datetime2(2)");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("dateUpdated")
                    .HasColumnType("datetime2(2)");

                entity.Property(e => e.FolderId).HasColumnName("folderID");

                entity.Property(e => e.HasCustomAcl).HasColumnName("hasCustomAcl");

                entity.Property(e => e.Hidden).HasColumnName("hidden");

                entity.Property(e => e.LinkLibraryId).HasColumnName("linkLibraryID");

                entity.Property(e => e.ObjectiveId).HasColumnName("objectiveId");

                entity.Property(e => e.PageDisplayName)
                    .IsRequired()
                    .HasColumnName("pageDisplayName")
                    .HasMaxLength(255);

                entity.Property(e => e.PageEndTime)
                    .HasColumnName("pageEndTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.PageKeywords).HasColumnName("pageKeywords");

                entity.Property(e => e.PageSequence).HasColumnName("pageSequence");

                entity.Property(e => e.PageStartTime)
                    .HasColumnName("pageStartTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.PageTypeId).HasColumnName("pageTypeId");

                entity.Property(e => e.ShowEndDate)
                    .HasColumnName("showEndDate")
                    .HasColumnType("datetime2(2)");

                entity.Property(e => e.ShowStartDate)
                    .HasColumnName("showStartDate")
                    .HasColumnType("datetime2(2)");

                entity.HasOne(d => d.Folder)
                    .WithMany(p => p.PageList)
                    .HasForeignKey(d => d.FolderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_pageList_folderList");
            });
        }

        public DbSet<LET.Panopto.Scheduler.Models.EventSession> EventSession { get; set; }

        public DbSet<LET.Panopto.Scheduler.Models.ScheduleBlockResult> ScheduleBlockResult { get; set; }
    }
}
