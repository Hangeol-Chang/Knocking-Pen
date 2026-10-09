using UnityEngine;
using UnityEngine.SceneManagement;

// Buttons on the developer page.
public class SourceManager : MonoBehaviour
{
    public void insta() => Application.OpenURL("https://www.instagram.com/hihangeol/");

    public void youtube() => Application.OpenURL("https://www.youtube.com/channel/UCFHxE7grCP07kWQqkZYuf9A/featured");

    public void returntogame() => SceneManager.LoadScene("Pen0");
}
