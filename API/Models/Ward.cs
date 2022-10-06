using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    public partial class Ward
    {
        [Key]
        [StringLength(10)]
        public string WardID { get; set; } = null!;
        [StringLength(100)]
        public string WardName { get; set; } = null!;
        [StringLength(25)]
        public string? WardLevel { get; set; }
        [StringLength(100)]
        public string? WardEnglishName { get; set; }
        [StringLength(10)]
        public string DistrictID { get; set; } = null!;
    }
}
