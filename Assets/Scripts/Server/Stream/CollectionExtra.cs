
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace KHCore.Utils
//{
//    public static class CollectionExtra
//    {
//        public static T RandomElement<T>(this IList<T> arr)
//        {
//            return arr[new Random().Next(0, arr.Count)];
//        }
//        public static int RandomIndex<T>(this IList<T> arr)
//        {
//            return new Random().Next(0, arr.Count-1);//(0, arr.Count - 1);
//        }
//        public static void RandomElements<T>(this IList<T> arr, IList<T> pool, int count)
//        {
//            count += pool.Count;
//            foreach (var item in arr.RandomSort())
//            {
//                if (pool.Count >= count)
//                {
//                    break;
//                }
//                pool.Add(item);
//            }
//        }
//        public static List<T> WithOut<T>(this IList<T> arr, Func<T, bool> trueOut)
//        {
//            var lis = new List<T>();
//            foreach (var item in arr)
//            {
//                if (!trueOut(item))
//                {
//                    lis.Add(item);
//                    continue;
//                }
//            }
//            return lis;
//        }
//        public static List<T> RandomElements<T>(this IList<T> arr, int count)
//        {
//            var pool = new List<T>();
//            foreach (var item in arr.RandomSort())
//            {
//                if (pool.Count >= count)
//                {
//                    break;
//                }
//                pool.Add(item);
//            }
//            return pool;
//        }
//        public static List<T> RandomSort<T>(this IList<T> lis)
//        {
//            return lis.OrderBy(c => Guid.NewGuid()).ToList<T>();
//        }
//    }
//    public static class DicExtra
//    {
//        public static void AddOrUpdate<Key, Value>(this IDictionary<Key, Value> dic, Key key, Value value)
//        {
//            if (dic.ContainsKey(key))
//            {
//                dic[key] = value;
//            }
//            else
//            {
//                dic.Add(key, value);
//            }
//        }
//        public static SDictionary<Key, Value> ToSerializable<Key, Value>(this IDictionary<Key, Value> dic)
//        {
//            var ret = new SDictionary<Key, Value>();
//            foreach (var item in dic)
//            {
//                ret.Add(item.Key, item.Value);
//            }
//            return ret;
//        }
//        public static Value GetOrCreat<Key, Value>(this IDictionary<Key, Value> dic, Key key) where Value : new()
//        {
//            if (dic.TryGetValue(key, out var value))
//            {
//                return value;
//            }
//            value=Activator.CreateInstance<Value>();
//            dic.Add(key, value);
//            return value;
//        }
//        public static void AddOrPlus<Key>(this IDictionary<Key, int> dic, Key key, int value)
//        {
//            if (!dic.ContainsKey(key))
//            {
//                dic.Add(key, value);
//            }
//            else
//            {
//                dic[key] += value;
//            }
//        }
//        public static void AddOrPlus<Key>(this IDictionary<Key, double> dic, Key key, double value)
//        {
//            if (!dic.ContainsKey(key))
//            {
//                dic.Add(key, value);
//            }
//            else
//            {
//                dic[key] += value;
//            }
//        }
//        public static void AddOrPlus<Key>(this IDictionary<Key, double> dic, IDictionary<Key, double> other)
//        {
//            if (other == null)
//                return;
//            foreach (var item in other)
//            {
//                if (!dic.ContainsKey(item.Key))
//                {
//                    dic.Add(item.Key, item.Value);
//                }
//                else
//                {
//                    dic[(item.Key)] += item.Value;
//                }
//            }
//        }
//        public static void Cheng<Key>(this IDictionary<Key, double> dic, double coffence)
//        {
//            foreach (var item in dic)
//            {
//                dic[item.Key] *= coffence;
//            }
//        }
//        public static void Round<Key>(this IDictionary<Key, double> dic, int dis)
//        {
//            foreach (var item in dic)
//            {
//                dic[item.Key] =Math.Round( item.Value, dis);
//            }
//        }
//        public static void AddOrPlus<Key>(this IDictionary<Key, uint> dic, Key key, uint value)
//        {
//            if (!dic.ContainsKey(key))
//            {
//                dic.Add(key, value);
//            }
//            else
//            {
//                dic[key] += value;
//            }
//        }
//        public static void AddOrPlus<Key>(this IDictionary<Key, byte> dic, Key key, byte value)
//        {
//            if (!dic.ContainsKey(key))
//            {
//                dic.Add(key, value);
//            }
//            else
//            {
//                dic[key] += value;
//            }
//        }
//        public static void AddOrPlus<Key>(this IDictionary<Key, float> dic, Key key, float value)
//        {
//            if (!dic.ContainsKey(key))
//            {
//                dic.Add(key, value);
//            }
//            else
//            {
//                dic[key] += value;
//            }
//        }
//        public static void AddOrPlus<Key>(this IDictionary<Key, ushort> dic, Key key, ushort value)
//        {
//            if (!dic.ContainsKey(key))
//            {
//                dic.Add(key, value);
//            }
//            else
//            {
//                dic[key] += value;
//            }
//        }
//    }



//}
