using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using X_clone_API.Repository.Models;

namespace X_clone_API.Repository;

public partial class XCloneDbContext : DbContext
{
    public XCloneDbContext()
    {
    }

    public XCloneDbContext(DbContextOptions<XCloneDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Follower> Followers { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Repost> Reposts { get; set; }

    public virtual DbSet<Saved> Saveds { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-8T179V2;Initial Catalog=x_clone_db;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comment__C3B4DFAA6AA922F9");

            entity.HasOne(d => d.PostCommentedNavigation).WithMany(p => p.Comments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__PostCom__3C69FB99");

            entity.HasOne(d => d.UserCommentedNavigation).WithMany(p => p.Comments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__UserCom__3D5E1FD2");
        });

        modelBuilder.Entity<Follower>(entity =>
        {
            entity.HasKey(e => e.FollowersId).HasName("PK__Follower__35FD0298F12AC5FA");

            entity.HasOne(d => d.UserFollowedNavigation).WithMany(p => p.FollowerUserFollowedNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Followers__UserF__44FF419A");

            entity.HasOne(d => d.UserFollowingNavigation).WithMany(p => p.FollowerUserFollowingNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Followers__UserF__440B1D61");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__Post__AA12603864E4CB7E");

            entity.HasOne(d => d.UserPostedNavigation).WithMany(p => p.Posts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Post__Username__398D8EEE");
        });

        modelBuilder.Entity<Repost>(entity =>
        {
            entity.HasKey(e => e.RepostId).HasName("PK__Repost__5E7F927E11693774");

            entity.HasOne(d => d.PostRepostedNavigation).WithMany(p => p.Reposts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Repost__PostRepo__403A8C7D");

            entity.HasOne(d => d.UserRepostedNavigation).WithMany(p => p.Reposts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Repost__UserRepo__412EB0B6");
        });

        modelBuilder.Entity<Saved>(entity =>
        {
            entity.HasKey(e => e.SavedId).HasName("PK__Saved__0B058FFC28FF3A27");

            entity.HasOne(d => d.PostSavedNavigation).WithMany(p => p.Saveds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Saved__PostSaved__47DBAE45");

            entity.HasOne(d => d.UserSavedNavigation).WithMany(p => p.Saveds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Saved__UserSaved__48CFD27E");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
