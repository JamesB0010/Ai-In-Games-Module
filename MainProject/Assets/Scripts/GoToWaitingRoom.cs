using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToWaitingRoom : GOAPAction
{
    public override bool PrePerform()
    {
        return true;
    }

    public override bool PostPerform()
    {
        World.Instance.GetWorld().ModifyState("patientWaiting", 1);
        //add patient to queue
        World.Instance.AddPatient(this.gameObject);
        return true;
    }
}
