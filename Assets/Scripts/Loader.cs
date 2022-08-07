using UnityEngine;

public class Loader : MonoBehaviour
{
    [SerializeField]
    private GameObject gameManagerPrefab;

    private void OnEnable()
    {
        if(GameManager.instance == null)
        {
            GameObject newManager = Instantiate(gameManagerPrefab);
        }
    }
}
