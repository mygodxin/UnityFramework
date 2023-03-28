using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 收集器接口。
/// </summary>
public interface ICollect
{
    Dictionary<string, Component> CollectComponentFields(Transform target, string fieldNamePrefix, bool fieldNameByType, Dictionary<string, string> componentMapDict);
}
