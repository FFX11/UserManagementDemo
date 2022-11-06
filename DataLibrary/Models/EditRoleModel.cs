using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class EditRoleModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role Name is required")]
        public string RoleName { get; set; } = string.Empty;

        public List<string> Users { get; set; } = new List<string>();
    }
}
