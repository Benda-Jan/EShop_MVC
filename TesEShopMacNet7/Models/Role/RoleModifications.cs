using System;
using System.ComponentModel.DataAnnotations;

namespace TestEShopMacNet7.Models.Role
{
    public class RoleModification
    {
        [Required]
        public string? Name { get; set; }

        public string? Id { get; set; }

        public string[]? AddIds { get; set; }

        public string[]? RemoveIds { get; set; }
    }
}

