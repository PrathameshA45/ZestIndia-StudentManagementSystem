using System;
using System.Collections.Generic;
using System.Text;

namespace Structure.Data.Entities;

public class Student : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public int Age { get; set; }

    public string Course { get; set; } = string.Empty;
}