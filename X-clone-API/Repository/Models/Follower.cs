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

    public int UserFollowing { get; set; }

    public int UserFollowed { get; set; }

    [ForeignKey("UserFollowed")]
    [InverseProperty("FollowerUserFollowedNavigations")]
    public virtual User UserFollowedNavigation { get; set; } = null!;

    [ForeignKey("UserFollowing")]
    [InverseProperty("FollowerUserFollowingNavigations")]
    public virtual User UserFollowingNavigation { get; set; } = null!;
}
