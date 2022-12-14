using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gene
{
    public float direction;
    public float moveLength;
    public float speed;

    public Gene()
    {
        Initialize();
    }

    private void Initialize()
    {
        direction = Random.Range(0.0f, 360.0f);
        moveLength = Random.Range(0.0f, 1.0f);
        speed = Random.Range(1.0f, 10.0f);
    }
}
