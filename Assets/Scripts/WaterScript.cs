using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScript : MonoBehaviour
{
    [SerializeField] private float waterMovingSpeed = 5;
    [SerializeField]private Material myMaterial;

    // Update is called once per frame
    void Update()
    {
        myMaterial.mainTextureOffset = new Vector2(myMaterial.mainTextureOffset.x + waterMovingSpeed * Time.deltaTime, myMaterial.mainTextureOffset.y + waterMovingSpeed/1.5f * Time.deltaTime);
    }
}
