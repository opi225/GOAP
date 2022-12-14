using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenomeManager : MonoBehaviour
{
    public List<GameObject> creatures = new List<GameObject>();

    public int generationNum = 1;
    public int popSize = 50;
    public int genomeSize = 12;

    public GameObject target;
    public GameObject creaturePrefab;

    public Material success;
    public Material diedOff;
    public Material passedOn;

    private void Start()
    {
        creatures = firstGen();
        Time.timeScale = 3.0f;
    }

    private void Update()
    {
        bool reset = true;
        foreach(GameObject c in creatures)
        {
            if(Vector3.Distance(c.transform.position, target.transform.position)<= 0.5f)
            {
                c.GetComponent<MeshRenderer>().material = success;
                for(int i = 0; i < popSize; i++)
                {
                    creatures[i].GetComponent<TestChildren>().running = false;
                }
                this.enabled = false;
            }
            else
            {
                if (c.GetComponent<TestChildren>().running == true)
                {
                    reset = false;
                }
            }
        }

        if (reset)
        {
            creatures = nextGen();
            generationNum++;
        }
    }

    public List<GameObject> firstGen()
    {
        List<GameObject> genOne = new List<GameObject>();
        for(int i = 0; i < popSize; i++)
        {
            genOne.Add(Instantiate(creaturePrefab));
            genOne[i].GetComponent<TestChildren>().genome = newGenome();
            genOne[i].GetComponent<TestChildren>().target = target;
        }
        return genOne;
    }

    public List<Gene> newGenome()
    {
        List<Gene> genome = new List<Gene>();
        for(int i = 0; i < genomeSize; i++)
        {
            genome.Add(new Gene());
        }
        return genome;
    }

    public List<GameObject> nextGen()
    {
        List<GameObject> newGen = new List<GameObject>();
        rankCreatures();
        creatures = reducedList();
        for(int i = 0; i < popSize; i++)
        {
            int mom = Random.Range(0, creatures.Count);
            int dad = Random.Range(0, creatures.Count);

            newGen.Add(Instantiate(creaturePrefab));
            newGen[i].GetComponent<TestChildren>().genome = GeneticCrossover(creatures[mom].GetComponent<TestChildren>().genome, creatures[dad].GetComponent<TestChildren>().genome);
            newGen[i].GetComponent<TestChildren>().target = target;

            int mutationChance = Random.Range(1, 101);
            if (mutationChance > 95)
            {
                mutateGene(newGen[i]);
                Debug.Log("Mutation Occured");
            }
        }
        creatures.Clear();
        return newGen;
    }

    public List<GameObject> reducedList()
    {
        //int separate = (popSize / 2) + Random.Range(-10, 10);
        List<GameObject> smaller = new List<GameObject>();
        for(int i = 0; i < popSize; i++)
        {
            if((popSize - i) > Random.Range(1, 50))
            {
                creatures[i].GetComponent<MeshRenderer>().material = passedOn;
                smaller.Add(creatures[i]);
            }
            else
            {
                creatures[i].GetComponent<MeshRenderer>().material = diedOff;
            }
        }
        Debug.Log("The amount of surviving creatures is " + smaller.Count);
        return smaller;
    }

    public void rankCreatures()
    {
        creatures.Sort(SortByScore);
    }

    static int SortByScore(GameObject one, GameObject two)
    {
        return one.GetComponent<TestChildren>().score.CompareTo(two.GetComponent<TestChildren>().score);
    }

    public List<Gene> GeneticCrossover(List<Gene> parentOne, List<Gene> parentTwo)
    {
        List<Gene> newBaby = new List<Gene>();
        List<int> fromMom = new List<int>(); 
        //int geneSeparation = Random.Range(0, genomeSize);
        for (int i = 0; i < genomeSize; i++)
        {
            fromMom.Add(Random.Range(0, 2));
        }
        for(int i = 0; i < genomeSize; i++)
        {
            if(fromMom[i] == 0)
            {
                newBaby.Add(parentOne[i]);
            }
            else
            {
                newBaby.Add(parentTwo[i]);
            }
        }

        return newBaby;
    }

    public void mutateGene(GameObject mutatedCreature)
    {
        int geneToMutate = Random.Range(0, genomeSize);
        int valueToMutate = Random.Range(0, 3);

        switch (valueToMutate)
        {
            case 0:
                mutatedCreature.GetComponent<TestChildren>().genome[geneToMutate].direction = Random.Range(0, 360);
                break;
            case 1:
                mutatedCreature.GetComponent<TestChildren>().genome[geneToMutate].moveLength = Random.Range(0.0f, 1.0f);
                break;
            case 2:
                mutatedCreature.GetComponent<TestChildren>().genome[geneToMutate].speed = Random.Range(1.0f, 10.0f);
                break;
        }
    }
}
