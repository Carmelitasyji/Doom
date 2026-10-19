using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public static KeyManager Instance;
    public GameObject purpleKeyImage;
    public GameObject greenKeyImage;
    public GameObject blueKeyImage;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        Keys.hasPurpleKey = false;
        Keys.hasBlueKey = false;
        Keys.hasGreenKey = false;

        purpleKeyImage.SetActive(false);
        greenKeyImage.SetActive(false);
        blueKeyImage.SetActive(false);
    }
}
