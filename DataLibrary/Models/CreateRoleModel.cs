using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class CreateRoleModel
    {
        [Required]
        public string RoleName { get; set; } = string.Empty;
    }
}
