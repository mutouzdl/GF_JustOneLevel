
using UnityEngine;

/// <summary>
/// 位置工具类
/// </summary>
public static class PositionUtility {
    /// <summary>
    /// 获取地图最大坐标
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetMapMaxPosition() {
        return new Vector3(Constant.Map.MAX_X, 0, Constant.Map.MAX_Z);
    }

    /// <summary>
    /// 获取地图最小坐标
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetMapMinPosition() {
        return new Vector3(0, 0, 0);
    }

    /// <summary>
    /// 根据地图边界调整坐标
    /// </summary>
    /// <param name="pos">指定坐标</param>
    /// <returns></returns>
    public static Vector3 GetAjustPositionWithMap(Vector3 pos) {
        Vector3 maxPos = GetMapMaxPosition();
        Vector3 minPos = GetMapMinPosition();

        if (pos.x > maxPos.x) {
            pos.x = maxPos.x;
        }
        if (pos.x < minPos.x) {
            pos.x = minPos.x;
        }
        if (pos.z > maxPos.z) {
            pos.z = maxPos.z;
        }
        if (pos.z < minPos.z) {
            pos.z = minPos.z;
        }

        return pos;
    }

    /// <summary>
    /// 判断坐标是否超出地图边界
    /// </summary>
    /// <param name="pos">指定坐标</param>
    /// <returns></returns>
    public static bool IsOutOfMapBoundary(Vector3 pos) {
        Vector3 maxPos = GetMapMaxPosition();
        Vector3 minPos = GetMapMinPosition();

        if (pos.x > maxPos.x || pos.x < minPos.x
            || pos.z > maxPos.z || pos.z < minPos.z) {
            pos.x = maxPos.x;

            return true;
        }

        return false;
    }
}