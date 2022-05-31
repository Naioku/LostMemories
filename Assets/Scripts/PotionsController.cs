using UnityEngine;

public class PotionsController : MonoBehaviour
{
    [SerializeField] private float chargeValue;

    public float GetChargeValue()
    {
        return chargeValue;
    }
}
