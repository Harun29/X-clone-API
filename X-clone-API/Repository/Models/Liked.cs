using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace X_clone_API.Repository.Models;

[Table("Liked")]
public partial class Liked
{
    [Key]
    [Column("LikedID")]
    public int LikedId { get; set; }

    public int UserLiked { get; set; }

    public int PostLiked { get; set; }

    [ForeignKey("PostLiked")]
    [InverseProperty("Likeds")]
    public virtual Post PostLikedNavigation { get; set; } = null!;

    [ForeignKey("UserLiked")]
    [InverseProperty("Likeds")]
    public virtual User UserLikedNavigation { get; set; } = null!;
}
