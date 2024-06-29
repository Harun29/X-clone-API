using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace X_clone_API.Repository.Models;

[Table("Comment")]
public partial class Comment
{
    [Key]
    [Column("CommentID")]
    public int CommentId { get; set; }

    [Column(TypeName = "text")]
    public string CommentContent { get; set; } = null!;

    public int? NoLikes { get; set; }

    public int? NoComments { get; set; }

    public int? NoReposts { get; set; }

    public int PostCommented { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string UserCommented { get; set; } = null!;

    [ForeignKey("PostCommented")]
    [InverseProperty("Comments")]
    public virtual Post PostCommentedNavigation { get; set; } = null!;

    [ForeignKey("UserCommented")]
    [InverseProperty("Comments")]
    public virtual User UserCommentedNavigation { get; set; } = null!;
}
