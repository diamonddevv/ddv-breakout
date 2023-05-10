using Breakout.Window;
using Raylib_cs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Breakout.Window.SettingsState;

namespace Breakout.Util
{
    internal class Settings
    {
        private static readonly string filename = "data\\settings.sskvpf";
        public static SSKVPFManager settingsSSKVPF = new SSKVPFManager(filename);
        public static Dictionary<string, SettingWrapper> wrapperMap = new Dictionary<string, SettingWrapper>();

        public static readonly string
            KEY_USEMOUSE = "useMouse",
            KEY_USEEMPTY = "useEmpty",
            KEY_USEBALLBLOCK = "useBallBlock",
            KEY_MASTERVOL = "masterVol";


        public static readonly (Func<object, string> serialize, Func<string, object> deserialize)
            SERIAL_BOOL = (SSKVPFManager.SerializableObj.SeriBool, SSKVPFManager.SerializableObj.DeseriBool),
            SERIAL_INT = (SSKVPFManager.SerializableObj.SeriInt, SSKVPFManager.SerializableObj.DeseriInt);

        public static void Default()
        {
            Add(new SettingWrapper( KEY_USEMOUSE,       false,  (20, 120),  new BooleanSetting(),   "Use Mouse to Control Paddle:",  SERIAL_BOOL    ));
            Add(new SettingWrapper( KEY_USEEMPTY,       false,  (20, 150),  new BooleanSetting(),   "Use Empty Blocks:",             SERIAL_BOOL    ));
            Add(new SettingWrapper( KEY_USEBALLBLOCK,   true,   (20, 180),  new BooleanSetting(),   "Use Ball-Spawner Blocks:",      SERIAL_BOOL    ));
            Add(new SettingWrapper( KEY_MASTERVOL,      50,     (20, 210),  new IntSetting(),       "Master Volume:",                SERIAL_INT     ));
        }

        public static void Add(SettingWrapper wrapper)
        {
            wrapperMap.Add(wrapper.key, wrapper);
        }

        public static void ForEachSetting(Action<SettingWrapper> action)
        {
            foreach (var wrapper in wrapperMap)
            {
                action.Invoke(wrapper.Value);
            }
        }

        public static object GetValue(string key)
        {
            if (settingsSSKVPF.dict.TryGetValue(key, out var obj))
            {
                return obj.val;
            } return null;
        }

        public class SettingWrapper
        {
            public string key;
            public object defaultValue;
            public (int x, int y) position;
            public SettingType type;
            public Func<object, string> serialize;
            public Func<string, object> deserialize;
            public string text;

            public SettingWrapper(string key, object defaultValue, (int x, int y) position, SettingType type, string text, (Func<object, string> serialize, Func<string, object> deserialize) serial)
            {
                this.key = key;
                this.type = type;
                this.serialize = serial.serialize;
                this.deserialize = serial.deserialize;

                type.text = text;
                type.setting = defaultValue;
                type.pos = position;
            }

            public void Write()
            {
                Settings.settingsSSKVPF.SetObject(key, type.setting);
            }

            public void Tick()
            {
                type.Tick();
            }
        }

    }
}
