using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SourceManager : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        
    }

    public void insta()
    {
        Application.OpenURL("https://www.instagram.com/hihangeol/");
    }

    public void youtube()
    {
        Application.OpenURL("https://www.youtube.com/channel/UCFHxE7grCP07kWQqkZYuf9A/featured");
    }
    public void returntogame()
    {
        SceneManager.LoadScene("pen0");
    }

}
