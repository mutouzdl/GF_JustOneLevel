using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

/// <summary>
/// 参考来源：https://github.com/EllanJiang/StarForce
/// </summary>
public class ProcedureSplash : ProcedureBase {
    protected override void OnUpdate (ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds) {
        base.OnUpdate (procedureOwner, elapseSeconds, realElapseSeconds);

        // TODO: 增加一个 Splash 动画，这里先跳过
        // 编辑器模式下，直接进入预加载流程；否则，检查一下版本
        ChangeState (procedureOwner, GameEntry.Base.EditorResourceMode ? typeof (ProcedurePreload) : typeof (ProcedureCheckVersion));
    }
}