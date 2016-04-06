using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Security.Cryptography;

namespace FutureSight.lib
{
    static class Utilities
    {
        public static object DeepCopy(this object target)
        {
            object result;
            BinaryFormatter b = new BinaryFormatter();
            MemoryStream mem = new MemoryStream();

            try
            {
                b.Serialize(mem, target);
                mem.Position = 0;
                result = b.Deserialize(mem);
#if NULL
                System.Diagnostics.Debug.Print("CopyEnd, Size(bytes):{0}", mem.Length);
#endif
            }
            finally
            {
                mem.Close();
            }
            return result;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static string Join(this List<int> list)
        {
            return string.Join(",", list.Select(item => item.ToString()).ToArray());
        }
    }

    static class DictionaryExtensions
    {
        /// <summary>
        /// 値を取得、keyがなければデフォルト値を設定し、デフォルト値を取得
        /// </summary>
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue defaultValue)
        {

            //Dictionary自体がnullの場合はインスタンス作成
            if (source == null)
            {
                source = new Dictionary<TKey, TValue>();
            }

            //keyが存在しない場合はデフォルト値を設定
            if (!source.ContainsKey(key))
            {
                source[key] = defaultValue;
            }

            return source[key];
        }
    }
}
