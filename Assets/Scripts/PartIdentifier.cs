using UnityEngine;

/// <summary>
/// 定义部件的类型。
/// </summary>
public enum PartType
{
    Claw,
    Spike,
    Fin
    // 可根据需要添加更多类型
}

/// <summary>
/// 定义插槽或部件的侧边。
/// </summary>
public enum SocketSide
{
    Any, // 任何一侧
    Left,
    Right,
    Front,
    Back
}

/// <summary>
/// 附加在部件Prefab上的标识符，用于描述其属性。
/// </summary>
public class PartIdentifier : MonoBehaviour
{
    [Tooltip("这个部件的类型 (例如：爪子)")]
    public PartType partType;

    [Tooltip("这个部件应该安装在哪一侧")]
    public SocketSide requiredSide;
}
