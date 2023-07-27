using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Game : MonoBehaviour
{
    public static Game Instance;

    int score;
    public TextMeshProUGUI scoreText;

    public GameObject destructionPrefab;
    public int destructionPoolSize = 3;
    int destructionPoolIndex;
    GameObject[] destructionPool = new GameObject[0];

    private void Awake()
    {
        Instance = this;
        destructionPool = new GameObject[destructionPoolSize];
        for (int i = 0; i < destructionPoolSize; i++)
        {
            GameObject newObject = Instantiate(destructionPrefab, transform);
            newObject.SetActive(false);
            destructionPool[i] = newObject;
        }
    }
    public void AddScore(int value)
    {
        score += value;
        scoreText.text = string.Format("${0}", score);
    }

    public void GetDestructionEffect(Vector3 position)
    {
        destructionPool[destructionPoolIndex].SetActive(false);
        destructionPool[destructionPoolIndex].transform.position = position;
        destructionPool[destructionPoolIndex].SetActive(true);
        destructionPoolIndex = destructionPoolIndex % destructionPool.Length;
    }

}
