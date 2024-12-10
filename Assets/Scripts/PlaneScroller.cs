using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneScroller : MonoBehaviour
{
    private const string SpeedSettingKey = "_ScrollSpeed";

    private float scrollPrefSpeed;
    public float scrollSpeed = 0.5f; //Speed of the texture movement
    private Renderer rend;
    public string currentUserID;

    void Start()
    {
        currentUserID = Singleton.Instance.userID;

        scrollPrefSpeed = PlayerPrefs.GetFloat(currentUserID + SpeedSettingKey, 5f);
        scrollSpeed = scrollPrefSpeed / 10;
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        scrollPrefSpeed = PlayerPrefs.GetFloat(currentUserID + SpeedSettingKey, 5f);
        scrollSpeed = scrollPrefSpeed / 10;
        rend = GetComponent<Renderer>();

        float offset = Time.time * scrollSpeed;
        rend.material.mainTextureOffset = new Vector2(0, -offset); //Adjust the direction as needed
    }
}
