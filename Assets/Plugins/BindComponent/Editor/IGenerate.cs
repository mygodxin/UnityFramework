using System.Collections.Generic;

/// <summary>
/// 生成器接口。
/// </summary>
public interface IGenerate
{
    void GenerateComponentsCode(string filePath, string codeTemplate, string nameSpace, string className, Dictionary<string, string> fieldTypeDict);
    void GenerateBehaviourCode(string filePath, string codeTemplate, string nameSpace, string className);
}
