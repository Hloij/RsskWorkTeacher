using System;
using System.Collections.Generic;

namespace RsskWorkTeacher.ModelsDB;

public partial class Rp
{
    public string Id { get; set; } = null!;

    public string Ck { get; set; } = null!;

    public string Plan { get; set; } = null!;

    public string? Rp1 { get; set; }

    public string Name { get; set; } = null!;

    public virtual Ktp? Ktp { get; set; }
}
