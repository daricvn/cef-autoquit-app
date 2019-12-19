using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoquit2.Models {
    public class AppProcess {
        public decimal Pid { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Icon { get; set; }

        public override bool Equals( object obj ) {
            if ( obj is AppProcess )
                return this.Pid.Equals((obj as AppProcess).Pid);
            return base.Equals(obj);
        }

        public override int GetHashCode() {
            return Pid.GetHashCode();
        }
    }
}
