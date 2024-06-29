using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace X_clone_API.Repository.Models;

public partial class Follower
{
    [Key]
    [Column("FollowersID")]
    public int FollowersId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string UserFollowing { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string UserFollowed { get; set; } = null!;

    [ForeignKey("UserFollowed")]
    [InverseProperty("FollowerUserFollowedNavigations")]
    public virtual User UserFollowedNavigation { get; set; } = null!;

    [ForeignKey("UserFollowing")]
    [InverseProperty("FollowerUserFollowingNavigations")]
    public virtual User UserFollowingNavigation { get; set; } = null!;
}
