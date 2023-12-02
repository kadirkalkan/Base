using Base.Api.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Api.Domain.Models;

public class User : BaseEntity
{
    public string Username { get; set; }
    public string EmailAddress { get; set; }
    public string Password { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool IsActive { get; set; } = true;

    public virtual ICollection<EmailConfirmation> EmailConfirmations { get; set; }
}
