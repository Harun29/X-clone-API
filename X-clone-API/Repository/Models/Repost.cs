using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace X_clone_API.Repository.Models;

[Table("Repost")]
public partial class Repost
{
    [Key]
    [Column("RepostID")]
    public int RepostId { get; set; }

    public int PostReposted { get; set; }

    public int UserReposted { get; set; }

    [ForeignKey("PostReposted")]
    [InverseProperty("Reposts")]
    public virtual Post PostRepostedNavigation { get; set; } = null!;

    [ForeignKey("UserReposted")]
    [InverseProperty("Reposts")]
    public virtual User UserRepostedNavigation { get; set; } = null!;
}
