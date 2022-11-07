using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public int copperOre = 0;
    public int tinOre = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeliverOre(int copperAdd, int tinAdd)
    {
        copperOre += copperAdd;
        tinOre += tinAdd;
    }
}
