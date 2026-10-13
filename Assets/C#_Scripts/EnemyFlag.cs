using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlag : MonoBehaviour
{
    private SpriteRenderer flagRenderer;
    // Start is called before the first frame update
    void Start()
    {
        flagRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Player.courseClear)
        {
            flagRenderer.color = new Color32(255, 255, 255, 255/2);
        }
    }
}
