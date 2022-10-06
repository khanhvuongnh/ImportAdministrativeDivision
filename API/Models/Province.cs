using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    public partial class Province
    {
        [Key]
        [StringLength(10)]
        public string ProvinceID { get; set; } = null!;
        [StringLength(100)]
        public string ProvinceName { get; set; } = null!;
    }
}
