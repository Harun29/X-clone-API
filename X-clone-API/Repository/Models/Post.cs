using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace X_clone_API.Repository.Models;

[Table("Post")]
public partial class Post
{
    [Key]
    [Column("PostID")]
    public int PostId { get; set; }

    [Column(TypeName = "text")]
    public string PostContent { get; set; } = null!;

    public int? NoLikes { get; set; }

    public int? NoComments { get; set; }

    public int? NoReposts { get; set; }

    public int UserPosted { get; set; }

    [InverseProperty("PostCommentedNavigation")]
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [InverseProperty("PostRepostedNavigation")]
    public virtual ICollection<Repost> Reposts { get; set; } = new List<Repost>();

    [InverseProperty("PostSavedNavigation")]
    public virtual ICollection<Saved> Saveds { get; set; } = new List<Saved>();

    [ForeignKey("UserPosted")]
    [InverseProperty("Posts")]
    public virtual User UserPostedNavigation { get; set; } = null!;
}
