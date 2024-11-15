using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Serialization;

namespace iCloud.Dav.Calendar.Utils;

internal class SerializationUtil
{
    private static readonly ConcurrentDictionary<Type, List<MethodInfo>> _onDeserializingMethods = new ConcurrentDictionary<Type, List<MethodInfo>>();

    private static readonly ConcurrentDictionary<Type, List<MethodInfo>> _onDeserializedMethods = new ConcurrentDictionary<Type, List<MethodInfo>>();

    public static void OnDeserializing(object obj)
    {
        foreach (MethodInfo deserializingMethod in GetDeserializingMethods(obj.GetType()))
        {
            deserializingMethod.Invoke(obj, new object[1] { default(StreamingContext) });
        }
    }

    public static void OnDeserialized(object obj)
    {
        foreach (MethodInfo deserializedMethod in GetDeserializedMethods(obj.GetType()))
        {
            deserializedMethod.Invoke(obj, new object[1] { default(StreamingContext) });
        }
    }

    private static List<MethodInfo> GetDeserializingMethods(Type targetType)
    {
        return targetType == null
            ? new List<MethodInfo>()
            : _onDeserializingMethods.ContainsKey(targetType)
            ? _onDeserializingMethods[targetType]
            : _onDeserializingMethods.GetOrAdd(targetType, (tt) => (from targetTypeMethodInfo in tt.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                                                    where targetTypeMethodInfo.GetCustomAttributes(typeof(OnDeserializingAttribute), inherit: false).Any()
                                                                    select targetTypeMethodInfo).ToList());
    }

    private static List<MethodInfo> GetDeserializedMethods(Type targetType)
    {
        if (targetType == null)
        {
            return new List<MethodInfo>();
        }

        if (_onDeserializedMethods.TryGetValue(targetType, out var methodInfos))
        {
            return methodInfos;
        }

        methodInfos = (from targetTypeMethodInfo in targetType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                       select new
                       {
                           targetTypeMethodInfo,
                           attrs = targetTypeMethodInfo.GetCustomAttributes(typeof(OnDeserializedAttribute), inherit: false).ToList()
                       } into t
                       where t.attrs.Count > 0
                       select t.targetTypeMethodInfo).ToList();
        _onDeserializedMethods.AddOrUpdate(targetType, methodInfos, (type, list) => methodInfos);
        return methodInfos;
    }
}