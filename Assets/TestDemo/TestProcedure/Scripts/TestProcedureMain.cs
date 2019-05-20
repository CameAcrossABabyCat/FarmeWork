using SimpleGameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProcedureMain : MonoBehaviour
{

    void Start()
    {
        ProcedureManager procedureManager = FrameworkEntry.Instance.GetManager<ProcedureManager>();

        TestProcedureStart entranceProcedure = new TestProcedureStart();
        procedureManager.AddProcedure(entranceProcedure);
        procedureManager.SetEntranceProcedure(entranceProcedure);

        procedureManager.AddProcedure(new TestProcedurePlay());
        procedureManager.AddProcedure(new TestProcedureOver());

        procedureManager.CreateProceduresFsm();
    }
}
