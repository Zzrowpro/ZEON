using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Fuel : MonoBehaviour
{
    //Properties : 
    public float fuelAmount = 100f;
    public float FuelAmount
    {
        get{return fuelAmount;}
        set
        {
            fuelAmount = math.clamp(value, 0, 100f);
        }
    }

    public float fuelRate = 1f;
    public float fuelSpeed = 1f;
    
    private bool inrange;
    
    private PlayerController playerController;

    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (inrange)
        {
            Fueling();
        }
        else
        {
            RefillTank();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inrange = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inrange = false;
        }
    }

    private void Fueling()
    {
        if(fuelAmount > 0 && playerController.Fuel != 200f)
        {
             playerController.Fuel += fuelRate *(fuelSpeed * Time.deltaTime);
             fuelAmount -= fuelRate * (fuelSpeed * Time.deltaTime);
        }
       
    }

    private void RefillTank()
    {
        if(fuelAmount < 100)
        {
            fuelAmount += 1 *(1f * Time.deltaTime);
        }
    }
}