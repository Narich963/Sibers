﻿using Sibers.Core.Entities;

namespace Sibers.MVC.ViewModels.Users;

public class UserViewModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public string Email { get; set; }
    public List<Project> Projects { get; set; }
}
