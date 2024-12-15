using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    // Singleton
    private static HUDManager instance;
    public static HUDManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<HUDManager>();
            }
            
            return instance;
        }
    }

    [SerializeField] private RectTransform[] lives;
    [SerializeField] private RectTransform lifePrefab;
    [SerializeField] private RectTransform livesContainer;
    
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerController.Player.OnEntityHealthChanged += UpdatePlayerHealthUI;
        InitPlayerHealthUI();
    }

    private void UpdatePlayerHealthUI(int newHealth)
    {
        for (int i = 0; i < lives.Length; i++)
        {
            if (i < newHealth)
            {
                lives[i].gameObject.SetActive(true);
            }
            else
            {
                lives[i].gameObject.SetActive(false);
            }
        }
    }

    private void InitPlayerHealthUI()
    {
        lives = new RectTransform[PlayerController.Player.MaxHealth];
        for (int i = 0; i < lives.Length; i++)
        {
            lives[i] = Instantiate(lifePrefab, livesContainer);
            lives[i].anchoredPosition = new Vector2(i * lifePrefab.rect.width, 0);
        }
    }
}
