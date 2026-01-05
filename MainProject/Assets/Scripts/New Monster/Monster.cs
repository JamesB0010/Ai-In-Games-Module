using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private MeshRenderer eye;
    public float eyeColorChangeTime;
    
    public float emotionStateEnterTime { get; set; }
    
    
    public I_FoodItem currentFoodItem;
        
        
     public void ChangeEyeColour(Color color)
        {
            eyeColorChangeTime = 1.0f;
            this.eye.material.color.LerpTo(color, eyeColorChangeTime, val => this.eye.material.color = val);
        }
}
