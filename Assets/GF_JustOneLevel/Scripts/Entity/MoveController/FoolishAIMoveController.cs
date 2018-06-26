using GameFramework;
using UnityEngine;

/// <summary>
/// 简单（愚蠢）的移动控制器AI
/// </summary>
public class FoolishAIMoveController : IMoveController {
    /// <summary>
    /// 移动间隔（秒）
    /// </summary>
    private const int PER_WALK_TIME = 5;
    /// <summary>
    /// 转身间隔（秒）
    /// </summary>
    private const int PER_ROTATE_TIME = 8;
    /// <summary>
    /// 转身持续时间（秒）
    /// </summary>
    private const int ROTATE_CONTINUE_TIME = 3;
    /// <summary>
    /// 站立间隔（秒）
    /// </summary>
    private const int PER_IDLE_TIME = 5;
    /// <summary>
    /// 初始输出
    /// </summary>
    private readonly Vector3 INIT_INPUT = Vector3.zero;

    /// <summary>
    /// 下一次行走时间
    /// </summary>
    private float nextWalkTime = 0;
    /// <summary>
    /// 下一次转身时间
    /// </summary>
    private float nextRotateTime = 0;
    /// <summary>
    /// 停止转身的时间
    /// </summary>
    private float stopRotateTime = 0;

    /// <summary>
    /// 下一次站立时间
    /// </summary>
    private float nextIdleTime = 0;

    private Vector3 inputVec = Vector3.zero;
    private bool isWalk = false;
    private bool isRotate = false;


    public Vector3 GetInput () {
        inputVec = INIT_INPUT;

        // 停止转身
        if (Time.time >= stopRotateTime) {
            stopRotateTime = Time.time + ROTATE_CONTINUE_TIME;
            isRotate = false;
        }

        // 随机转身
        if (Time.time >= nextRotateTime) {
            nextRotateTime = Time.time + PER_ROTATE_TIME;
            if (Utility.Random.GetRandom (100) <= 80) {
                isRotate = true;
            }
        }

        // 随机进入行走状态
        if (Time.time >= nextWalkTime) {
            nextWalkTime = Time.time + PER_IDLE_TIME;

            if (Utility.Random.GetRandom (100) <= 80) {
                nextWalkTime = Time.time + PER_WALK_TIME;
                isWalk = true;
            }
        }

        // 随机站立
        if (Time.time >= nextIdleTime) {
            nextIdleTime = Time.time + PER_IDLE_TIME;

            if (Utility.Random.GetRandom (100) <= 30) {
                nextIdleTime = Time.time + PER_IDLE_TIME;
                isWalk = false;
            }
        }

        if (isRotate == true) {
            inputVec.x = 1;
        }

        if (isWalk == true) {
            inputVec.y = 1;
        }

        return inputVec;
    }
}