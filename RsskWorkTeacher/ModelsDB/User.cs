using System;
using System.Collections.Generic;

namespace RsskWorkTeacher.ModelsDB;

public partial class User
{
    public string Id { get; set; } = null!;

    public string? Login { get; set; }

    public string? Password { get; set; }

    public string? IdDostup { get; set; }

    public virtual Dostup? IdDostupNavigation { get; set; }
}
