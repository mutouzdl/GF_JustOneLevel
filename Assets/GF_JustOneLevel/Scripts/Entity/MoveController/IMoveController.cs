using UnityEngine;

/// <summary>
/// 移动控制器接口
/// </summary>
public interface IMoveController {
    /// <summary>
    /// 获取移动参数： x为旋转、y为移动、z暂时无用
    /// </summary>
    /// <returns></returns>
    Vector3 GetInput();
}