using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChildren : MonoBehaviour
{

    public List<Gene> genome = new List<Gene>();
    public float score = 0;

    public float currentGeneTime = 0.0f;
    public int geneSelect = 0;
    public bool running = true;

    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (running)
        {
            moveGene(genome[geneSelect]);
        }
        if(geneSelect >= genome.Count)
        {
            calculateScore(target.transform);
            running = false;
        }
    }

    void calculateScore(Transform targetSpot)
    {
        score = Vector3.Distance(transform.position, targetSpot.position);
    }

    void moveGene(Gene currentGene)
    {
        float xValue = Mathf.Sin(currentGene.direction);
        float zValue = Mathf.Cos(currentGene.direction);
        Vector3 movement = new Vector3(xValue, 0, zValue).normalized;
        transform.Translate(movement * currentGene.speed * Time.deltaTime);
        currentGeneTime += Time.deltaTime;
        if (currentGeneTime >= genome[geneSelect].moveLength)
        {
            currentGeneTime = 0.0f;
            geneSelect++;
        }
    }
}
