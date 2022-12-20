using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hidayah_collage.Models.SystemMaster
{
    public class SystemMaster
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string Value_Txt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDt { get; set; }
    }
}
