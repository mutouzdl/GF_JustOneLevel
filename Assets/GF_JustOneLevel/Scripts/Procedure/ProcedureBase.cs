using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

public abstract class ProcedureBase : GameFramework.Procedure.ProcedureBase {
    protected ProcedureOwner m_ProcedureOwner = null;

    protected override void OnInit (ProcedureOwner procedureOwner) {
        base.OnInit (procedureOwner);

        this.m_ProcedureOwner = procedureOwner;
    }
}