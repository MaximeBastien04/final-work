using System.Collections.Generic;
using UnityEngine;

public class Printer : MonoBehaviour
{
    [SerializeField] private GameObject paperPrefab;
    [SerializeField] private Transform paperSpawner;
    
    private Queue<GameObject> printedPapers = new Queue<GameObject>();
    private int maxPapers = 10;
    public void PrintPaper()
    {
        GameObject spawnedPaper = Instantiate(paperPrefab, paperSpawner.position, paperSpawner.rotation);
        spawnedPaper.transform.parent = this.transform;
        GetComponent<AudioSource>().Play();

        printedPapers.Enqueue(spawnedPaper);

        if (printedPapers.Count > maxPapers)
        {
            GameObject oldestPaper = printedPapers.Dequeue();
            if (oldestPaper != null)
            {
                Destroy(oldestPaper);
            }
        }
    }
}
