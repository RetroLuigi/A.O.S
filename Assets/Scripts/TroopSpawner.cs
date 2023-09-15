using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TroopSpawner : MonoBehaviour
{
    // Start is called before the first frame update

    public List<GameObject> troops = new List<GameObject>();
    public List<GameObject> thisTroops = new List<GameObject>();
    public float minR;
    public float maxR;
    public float radius;
    public float spawnX;
    public float spawnY;
    public int maxTroops;

    [SerializeField]
    private Vector3 spawnPos;

    void Start()
    {
        StartCoroutine("spawn");
    }

    public IEnumerator spawn()
    {
        while (true)
        {
            if (thisTroops.Count < maxTroops)
            {
                yield return new WaitForSeconds(Random.Range(minR, maxR));
                spawnX = Random.Range(-radius, radius);
                spawnY = Random.Range(-radius, radius);
                spawnPos = new Vector3(spawnX, spawnY, 0);
                thisTroops.Add(
                    Instantiate(troops[0], transform.position + spawnPos, Quaternion.identity)
                );
            }
            yield return new WaitForSeconds(0.001f);
            for (int i = 0; i < thisTroops.Count; i++)
            {
                if (thisTroops[i] == null)
                {
                    thisTroops.Remove(thisTroops[i]);
                }
            }
        }
    }

    // Update is called once per frame
    void Update() { }
}
