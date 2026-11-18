using System;
using System.Collections.Generic;

namespace RsskWorkTeacher.ModelsDB;

public partial class Ktp
{
    public string Id { get; set; } = null!;

    public string Plan { get; set; } = null!;

    public string Ck { get; set; } = null!;

    public string Ktp1 { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual Rp IdNavigation { get; set; } = null!;
}
