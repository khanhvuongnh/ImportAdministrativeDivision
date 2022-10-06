using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    public partial class District
    {
        [Key]
        [StringLength(10)]
        public string DistrictID { get; set; } = null!;
        [StringLength(100)]
        public string DistrictName { get; set; } = null!;
        [StringLength(10)]
        public string ProvinceID { get; set; } = null!;
    }
}
