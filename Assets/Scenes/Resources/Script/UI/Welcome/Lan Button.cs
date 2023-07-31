using UnityEngine;
using UnityEngine.SceneManagement;

public class LanButton : MonoBehaviour
{
    public void ButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void BackButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}