using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.Util
{
    internal class SSKVPFManager // SSKVPF stands for "Simple Serialized Key-Value Pair File"
    {
        private string filename;
        public Dictionary<string, SerializableObj> dict;

        public SSKVPFManager(string filename)
        {
            this.filename = filename;
            this.dict = new Dictionary<string, SerializableObj>();
        }

        public void SetObject(string key, object obj)
        {
            if (dict.TryGetValue(key, out var val))
            {
                val.val = obj;
            }
        }
        public object? GetObject(string key)
        {
            if (dict.TryGetValue(key, out var v))
            {
                return v.val;
            }
            return null;
        }

        public class SerializableObj
        {
            private object def;
            public object val;
            public Func<object, string> s;
            public Func<string, object> d;

            public SerializableObj(object val, Func<object, string> serialize, Func<string, object> deserialize)
            {
                this.def = val;
                this.val = val;
                this.s = serialize;
                this.d = deserialize;
            }

            public void Revert()
            {
                this.val = this.def;
            }

            public override string ToString()
            {
                return $"SerializableObject[default:{def},value:{val},serializer:{s},deserializer:{d}]";
            }

            public static string SeriBool(object obj)
            {
                if (obj is bool)
                {
                    return ((bool)obj).ToString();
                }
                else return "null";
            }
            public static object DeseriBool(string deseri)
            {
                try
                {
                    return bool.Parse(deseri);
                }
                catch (Exception e)
                {
                    throw new Exception($"Setting deserialization failed, read {deseri}");
                }
            }

            public static string SeriFloat(object obj)
            {
                if (obj is float)
                {
                    return ((float)obj).ToString();
                }
                else return "null";
            }
            public static object DeseriFloat(string deseri)
            {
                try
                {
                    return float.Parse(deseri);
                }
                catch (Exception e)
                {
                    throw new Exception($"Setting deserialization failed, read {deseri}");
                }
            }
        }


        public void write()
        {
            Console.WriteLine($"Writing to {filename}..");
            PrepareFileManagement(filename);
            StreamWriter writer = new StreamWriter(filename);

            foreach (var kvp in dict)
            {
                string v = kvp.Value.s.Invoke(kvp.Value.val);
                writer.WriteLine($"{kvp.Key}={v}");
            }

            Console.WriteLine($"Wrote Setting key-values to {filename}.");
            writer.Close();

        }

        public bool read()
        {
            bool b = PrepareFileManagement(filename);
            StreamReader reader = new StreamReader(filename);
            try
            {
                if (!b)
                {
                    
                    if (reader.EndOfStream)
                    {
                        throw new Exception("File was Empty!");
                    }

                    while (true)
                    {
                        string? s = reader.ReadLine();
                        if (s == null) break;
                        string[] kv = s.Split('=');
                        if (dict.ContainsKey(kv[0]))
                        {
                            dict.TryGetValue(kv[0], out var value);
                            if (value != null) {
                                value.val = value.d.Invoke(kv[1]);
                            }
                        }
                    }

                    
                }
                else RevertAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception whilst reading {filename}: {ex.Message}");
                RevertAll();
            } finally
            {
                reader.Close();
            }

            return b;
        }

        private void RevertAll()
        {
            foreach (var kvp in dict)
            {
                kvp.Value.Revert();
            }
        }

        public static bool PrepareFileManagement(string filename)
        { // returns true if a file was created

            string[] parts = filename.Split('\\');
            if (parts.Length > 1)
            {
                int l = parts.Length;
                if (!Directory.Exists(parts[l - 2]))
                {
                    Directory.CreateDirectory(parts[l - 2]);
                }
            }

            if (!File.Exists(filename))
            {
                Console.WriteLine($"Creating {filename}..");
                File.Create(filename).Close();
                return true;
            }
            return false;
        }
    }
}
