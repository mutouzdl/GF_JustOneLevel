using GameFramework;
using GameFramework.Event;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using System;

public class ProcedureGame : ProcedureBase {
	private float time = 0;
	protected override void OnEnter (ProcedureOwner procedureOwner) {
		base.OnEnter (procedureOwner);
	}

	protected override void OnUpdate (ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds) {
	}
}