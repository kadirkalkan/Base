using Base.Api.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Api.Domain.Models;

public class EmailConfirmation : BaseEntity
{
    public string OldEmailAddress { get; set; }
    public string NewEmailAddress { get; set; }

    public Guid UserId { get; set; }
    public virtual User User { get; set; }
}
