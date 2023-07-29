using UnityEngine;

public class BoostScript : MonoBehaviour
{
    public int boostId;
    [SerializeField]private Transform meshTransform;
    private float s = 0;

    private void Update()
    {
        meshTransform.eulerAngles = new Vector3(90, meshTransform.eulerAngles.y+Time.deltaTime*75, 0);
        s += Time.deltaTime;
        if (s > 5) Destroy(gameObject);
    }
}
