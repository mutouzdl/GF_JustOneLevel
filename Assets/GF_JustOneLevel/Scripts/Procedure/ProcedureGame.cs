using GameFramework;
using GameFramework.Event;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using System;

public class ProcedureGame : ProcedureBase {
	private SurvivalGame m_SurvivalGame = null;
	private float time = 0;

    protected override void OnInit (ProcedureOwner procedureOwner) {
        base.OnInit (procedureOwner);

        m_SurvivalGame = new SurvivalGame ();
    }

	protected override void OnEnter (ProcedureOwner procedureOwner) {
		base.OnEnter (procedureOwner);

		m_SurvivalGame.Initialize();
	}

	protected override void OnUpdate (ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds) {
        if (m_SurvivalGame != null && !m_SurvivalGame.GameOver) {
            m_SurvivalGame.Update (elapseSeconds, realElapseSeconds);
            return;
        }
	}
}