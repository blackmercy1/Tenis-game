using UnityEngine.SceneManagement;

public class RestartButton : UIButton
{
    protected override void OnClick() => Restart();

    private void Restart()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }
}