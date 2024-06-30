using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace X_clone_API.Repository.Models;

[Table("Saved")]
public partial class Saved
{
    [Key]
    [Column("SavedID")]
    public int SavedId { get; set; }

    public int PostSaved { get; set; }

    public int UserSaved { get; set; }

    [ForeignKey("PostSaved")]
    [InverseProperty("Saveds")]
    public virtual Post PostSavedNavigation { get; set; } = null!;

    [ForeignKey("UserSaved")]
    [InverseProperty("Saveds")]
    public virtual User UserSavedNavigation { get; set; } = null!;
}
