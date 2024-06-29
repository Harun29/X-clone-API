using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace X_clone_API.Repository.Models;

[Table("User")]
[Index("Username", Name = "UQ__User__536C85E45BB67E80", IsUnique = true)]
[Index("Email", Name = "UQ__User__A9D10534ABE7954E", IsUnique = true)]
public partial class User
{
    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Key]
    [StringLength(50)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [Column(TypeName = "text")]
    public string? Bio { get; set; }

    public DateOnly Birthday { get; set; }

    [Column(TypeName = "image")]
    public byte[]? ProfilePicture { get; set; }

    [Column(TypeName = "image")]
    public byte[]? CoverPicture { get; set; }

    public int? NoFollowers { get; set; }

    public int? NoFollowing { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [InverseProperty("UserCommentedNavigation")]
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [InverseProperty("UserFollowedNavigation")]
    public virtual ICollection<Follower> FollowerUserFollowedNavigations { get; set; } = new List<Follower>();

    [InverseProperty("UserFollowingNavigation")]
    public virtual ICollection<Follower> FollowerUserFollowingNavigations { get; set; } = new List<Follower>();

    [InverseProperty("UserPostedNavigation")]
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    [InverseProperty("UserRepostedNavigation")]
    public virtual ICollection<Repost> Reposts { get; set; } = new List<Repost>();

    [InverseProperty("UserSavedNavigation")]
    public virtual ICollection<Saved> Saveds { get; set; } = new List<Saved>();
}
