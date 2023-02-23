using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hidayah_collage.Models.SystemMaster
{
    public class SystemMaster
    {
        [Key]
        [Column(Order =1)]
        public string Type { get; set; }
        [Key]
        [Column(Order = 2)]
        public string Code { get; set; }
        public string Value_Txt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDt { get; set; }
    }
}
