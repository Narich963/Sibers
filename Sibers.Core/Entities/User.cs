﻿using Microsoft.AspNetCore.Identity;

namespace Sibers.Core.Entities;

public class User : IdentityUser<int>
{
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }

    public List<Project> Projects { get; set; }
    public User()
    {
        Projects = new();
    }
}
