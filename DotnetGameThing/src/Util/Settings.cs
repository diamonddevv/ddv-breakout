using Breakout.Window;
using Raylib_cs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Breakout.Util.SSKVPFManager;
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
            KEY_MASTERVOL = "masterVol",
            KEY_BALLSPEED = "ballSpeed";
            


        public static readonly (Func<object, string> serialize, Func<string, object> deserialize)
            SERIAL_BOOL =   (   SSKVPFManager.SerializableObj.SeriBool,     SSKVPFManager.SerializableObj.DeseriBool    ),
            SERIAL_FLOAT =  (   SSKVPFManager.SerializableObj.SeriFloat,    SSKVPFManager.SerializableObj.DeseriFloat   );

        public static readonly Func<double, string>
            DELTAHANDLE_PERCENTAGE  = d => $"{Math.Round(d * 100, 2)}%",
            DELTAHANDLE_INT         = d => $"{Math.Round(d * 10, 0)}";

        public static void Default()
        {
            Add(new SettingWrapper( KEY_USEMOUSE,       false,  (20, 120),  new BooleanSetting(),                       "Use Mouse to Control Paddle:",  SERIAL_BOOL    ));
            Add(new SettingWrapper( KEY_USEEMPTY,       false,  (20, 150),  new BooleanSetting(),                       "Use Empty Blocks:",             SERIAL_BOOL    ));
            Add(new SettingWrapper( KEY_USEBALLBLOCK,   true,   (20, 180),  new BooleanSetting(),                       "Use Ball-Spawner Blocks:",      SERIAL_BOOL    ));
            Add(new SettingWrapper( KEY_MASTERVOL,      0.5f,   (20, 210),  new FloatSetting(DELTAHANDLE_PERCENTAGE),   "Master Volume:",                SERIAL_FLOAT   ));
            Add(new SettingWrapper( KEY_BALLSPEED,      0.5f,  (20, 240),  new FloatSetting(DELTAHANDLE_INT),          "Ball Speed:",                   SERIAL_FLOAT   ));
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
            object? o = settingsSSKVPF.GetObject(key);
            if (o == null)
            {
                return null;
            }
            return o;
        }

        public class SettingWrapper
        {
            public string key;
            public object defaultValue;
            public (int x, int y) position;
            public SettingType type;
            public Func<object, string> serialize;
            public Func<string, object> deserialize;
            public SerializableObj obj;

            public SettingWrapper(string key, object defaultValue, (int x, int y) position, SettingType type, string text, (Func<object, string> serialize, Func<string, object> deserialize) serial)
            {
                this.key = key;
                this.type = type;
                this.defaultValue = defaultValue;
                this.serialize = serial.serialize;
                this.deserialize = serial.deserialize;

                object? o = settingsSSKVPF.GetObject(key);

                type.text = text;
                type.settingKey = key;
                type.setting = o == null ? defaultValue : o;
                type.pos = position;

                obj = new SerializableObj(type.setting, serialize, deserialize);
            }

            public void Write()
            {
                Settings.settingsSSKVPF.SetObject(key, type.setting);
            }

            public void Tick()
            {
                type.Tick();
            }

            public void Init()
            {
                type.Initialize();
            }
        }

    }
}
