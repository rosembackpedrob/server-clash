using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodoiMenuManager : MonoBehaviour
{
    public static GodoiMenuManager Instance;

    [SerializeField] GodoiMenu[] menus;

    private void Awake()
    {
        Instance = this;
    }
    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName)
            {
                menus[i].Open();
            }
            else if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
    }
    public void OpenMenu(GodoiMenu menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
        menu.Open();
    }
    public void CloseMenu(GodoiMenu menu)
    {
        menu.Close();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
