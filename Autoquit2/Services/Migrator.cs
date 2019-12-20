using Autoquit2._App_Data;
using Autoquit2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoquit2.Services {
    internal class Migrator {
        public static void MigrateScript(ref Script script ) {
            foreach ( ScriptItem item in script.Scripts ) {
                if ( item.EventType.StartsWith("LEFT") || item.EventType.StartsWith("RIGHT") ) {
                    string[] coords = item.KeyName.Split(':');
                    if ( coords != null ) {
                        item.X = long.Parse(coords[0]);
                        item.Y = long.Parse(coords[1]);
                    }
                    item.KeyName = item.EventType.Substring(0, 1);
                    if ( item.EventType.Contains("CLICK") )
                        item.EventType = Constant.ENV_MOUSE_CLICK;
                    else if ( item.EventType.EndsWith("UP") )
                        item.EventType = Constant.ENV_MOUSE_UP;
                    else if ( item.EventType.EndsWith("DOWN") )
                        item.EventType = Constant.ENV_MOUSE_DOWN;
                }
            }
        }
    }
}
