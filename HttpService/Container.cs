using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpService {
    public static class Container {
        public static T GetController<T>() where T: Controller {
            foreach (var item in HttpService.Mapping ) {
                if ( item.Value.target is T )
                    return (T) item.Value.target;
            }
            return null;
        }
    }
}
