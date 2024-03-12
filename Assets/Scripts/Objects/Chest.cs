using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
This Class is for a chest object. All a chest does is store copper and tin ore, nothing else
 */

public class Chest : MonoBehaviour
{
    public int copperOre = 0;
    public int tinOre = 0;
    public int BronzeBar = 0;

    //adds to the copperOre and tinOre by whatever amount the person delivering the ore has
    public void DeliverOre(int copperAdd, int tinAdd)
    {
        copperOre += copperAdd;
        tinOre += tinAdd;
    }

    public void DeliverBars(int barAdd)
    {
        BronzeBar += barAdd;
    }

    public void TakeOre(int copperTake, int tinTake)
    {
        copperOre -= copperTake;
        tinOre -= tinTake;
    }

    public void TakeBar(int barTake)
    {
        BronzeBar -= barTake;
    }
}
