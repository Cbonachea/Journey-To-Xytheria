using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSource : MonoBehaviour
{

    [SerializeField] PlayerHeat playerHeat;

    [SerializeField] [Range(-100, 100)] private int heatIntensity = 1;
    [SerializeField] [Range(0.1f, 10f)] private float heatPeriod = 1;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (playerHeat.isHeatingUp) return;
        StartCoroutine(HeatUp());
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        playerHeat.PassiveCooling();
    }
    private IEnumerator HeatUp()
    {
        playerHeat.isCoolingDown = false;
        playerHeat.isHeatingUp = true;
        playerHeat.IncrementHeat(heatIntensity);
        yield return new WaitForSeconds(heatPeriod);
        playerHeat.isHeatingUp = false;
    }

}

