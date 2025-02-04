using System;
using Elixir.DATA.DTOs.Store;

namespace Elixir.Entities;

public class UserStore : BaseEntity<Guid>
{
    public Guid StoreId { get; set; }
    public Store? Store { get; set; }
    public Guid UserId { get; set; }
    public AppUser? User { get; set; }
    public EmployeeRole Role { get; set; }
}

public enum EmployeeRole
{
    Owner,
    Employee,
}