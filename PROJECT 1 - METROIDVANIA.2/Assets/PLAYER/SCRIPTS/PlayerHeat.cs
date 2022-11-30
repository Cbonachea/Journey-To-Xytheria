using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeat : MonoBehaviour
{

    [SerializeField] [Range(-100, 100)] internal int heatLevel = 0;
    [SerializeField] [Range(-100, 100)] internal int maxHeat = 30;
    [SerializeField] [Range(-1, 1)]     private int insulationLevel = 1;
    [SerializeField] [Range(-1, 1)]     private int passiveCooling = -1;
    [SerializeField] [Range(0.1f, 1f)]  private float coolingPeriod = 1f;
    [SerializeField] [Range(0.1f, 5f)]  private float coolingBufferPeriod = 3f;

    [SerializeField] internal bool isHeatingUp;
    [SerializeField] internal bool isCoolingDown;
    [SerializeField] internal bool coolingBuffer = true;
    [SerializeField] internal bool canStartCooling = true;

    private void Update()
    {
        if (heatLevel < 0)
        {
            heatLevel = 0;
        }
        if (!isHeatingUp && heatLevel > 0) PassiveCooling();
    }

    public void IncrementHeat(int heatDelta)
    {
    //    heatDelta = ApplyInsulation(heatDelta);
        heatLevel = heatLevel + heatDelta;
    } 

    private int ApplyInsulation(int heatDelta)
    {
        heatDelta = heatDelta * insulationLevel;
        return heatDelta;
    }
    public void IncrementInsulation(int insulationDelta)
    {
        insulationLevel = insulationLevel + insulationDelta;
    }

    public void PassiveCooling()
    {
        if (isCoolingDown) return;
        else StartCoroutine(CoolDown(passiveCooling));
    }

    private IEnumerator CoolDown(int heatDelta)
    {
        isHeatingUp = false;
        isCoolingDown = true;
            if (coolingBuffer)
            {
                yield return new WaitForSeconds(coolingBufferPeriod);
                coolingBuffer = false;
            }
        yield return new WaitForSeconds(coolingPeriod);
        IncrementHeat(heatDelta);
        isCoolingDown = false;
    }
}
