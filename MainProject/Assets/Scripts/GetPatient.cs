using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPatient : GOAPAction
{
    private GameObject resource; //in case of the patient, the cubicle will be the resource
    public override bool PrePerform()
    {
        //pre condition 
        //check if a patient is waiting
        this.target = World.Instance.RemovePatient();
        if (target == null)
            return false;

        //new code for rescource management
        this.resource = World.Instance.RemoveCubicle();
        if (this.resource != null)
        {
            this.inventory.AddItem(resource);
        }
        else
        {
            //if we cant get access to a free cubicle also release the patient back into pool
            World.Instance.AddPatient(target);
            this.target = null;
            return false; //preperform fails as no cubicle free and patient released
        }
        //end code for rescource management
        World.Instance.GetWorld().ModifyState("freeCubicle", -1); //successful rescource grab - removed a cubicle from pool
        return true;
    }

    public override bool PostPerform()
    {
        //need to update patient waiting as we have sucessfully allocated a patient to a nurse
        World.Instance.GetWorld().ModifyState("patientWaiting", -1);
        
        if(this.target) // game object of the nurse target
        //adds the cubicle to the patients inventory too
        this.target.GetComponent<Agent>().Inventory.AddItem(resource);

        //both patient and nurse have the cubicle in their inventory system
        //both can then generate a new action for treatment
        return true;
    }
}
