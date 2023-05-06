﻿namespace BudgetRequests.Models.Organizations;

public class Officer
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
}