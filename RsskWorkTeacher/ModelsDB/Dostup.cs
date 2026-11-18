using System;
using System.Collections.Generic;

namespace RsskWorkTeacher.ModelsDB;

public partial class Dostup
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public int? Lvl { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
