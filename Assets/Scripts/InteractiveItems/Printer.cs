using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages printing paper objects by spawning them at a specified location,
/// playing a sound on print, and limiting the number of printed papers in the scene.
/// </summary>
public class Printer : MonoBehaviour
{
    [SerializeField] private GameObject paperPrefab;
    [SerializeField] private Transform paperSpawner;

    private Queue<GameObject> printedPapers = new Queue<GameObject>();
    private int maxPapers = 10;

    /// <summary>
    /// Instantiates a paper prefab at the paperSpawner's position and rotation,
    /// plays the attached AudioSource's sound,
    /// and manages the queue to destroy the oldest paper if max count is exceeded.
    /// </summary>
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
