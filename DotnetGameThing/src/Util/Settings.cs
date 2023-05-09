using Raylib_cs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Breakout.Util
{
    internal class Settings
    {
        private static readonly string filename = "settings.sskvpf";
        public static SSKVPFManager settingsSSKVPF = new SSKVPFManager(filename);

        public static readonly string
            KEY_USEMOUSE = "useMouse",
            KEY_USEEMPTY = "useEmpty",
            KEY_MASTERVOL = "masterVol";


        public static void Default()
        {
            settingsSSKVPF.dict.Add(KEY_USEMOUSE, new SSKVPFManager.SerializableObj(false));
            settingsSSKVPF.dict.Add(KEY_USEEMPTY, new SSKVPFManager.SerializableObj(false));
            settingsSSKVPF.dict.Add(KEY_MASTERVOL, new SSKVPFManager.SerializableObj(50));
        }

    }
}
