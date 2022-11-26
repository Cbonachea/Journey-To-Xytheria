using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeat : MonoBehaviour
{

    [SerializeField] [Range(-100, 100)] private int heatLevel = 0;
    [SerializeField] [Range(-1, 1)]     private int insulationLevel = 1;
    [SerializeField] [Range(-1, 1)]     private int passiveCooling = -1;
    [SerializeField] [Range(0.1f, 1f)]  private float coolingPeriod = 1f;
    [SerializeField] [Range(0.1f, 5f)]  private float coolingBufferPeriod = 3f;
   
    internal bool isHeatingUp;
    internal bool isCoolingDown;
    internal bool coolingBuffer = true;

    private void Update()
    {
        if (!isHeatingUp) PassiveCooling();
    }

    public void IncrementHeat(int heatDelta)
    {
        heatDelta = ApplyInsulation(heatDelta);
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
        StartCoroutine(CoolDown(passiveCooling));
    }

    private IEnumerator CoolDown(int heatDelta)
    {
        isHeatingUp = false;
        isCoolingDown = true;
        while (heatLevel > 0)
        {
            if (coolingBuffer)
            {
                yield return new WaitForSeconds(coolingBufferPeriod);
                coolingBuffer = false;
            }
            IncrementHeat(heatDelta);
            yield return new WaitForSeconds(coolingPeriod);
        }
        if (heatLevel <= 0)
        {
            heatLevel = 0;
            coolingBuffer = true;
        }
        isCoolingDown = false;
    }

}
