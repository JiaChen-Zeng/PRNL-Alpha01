using UnityEngine;

public class HPIndicator : MonoBehaviour
{
    [SerializeField] private int hp;
    public int HP
    {
        get => hp;
        set
        {
            hp = value;
            for (int i = 0; i < MaxHP; i++)
            {
                transform.GetChild(i).gameObject.SetActive(i < hp);
            }
        }
    }

    public int MaxHP { get => GameManager.INSTANCE.PlayerInitialHP; }

    public void InitHPObjects()
    {
        RemovePlaceHolders();
        AddHPObjects();
    }

    private void AddHPObjects()
    {
        for (int i = 0; i < MaxHP; i++)
        {
            Instantiate(Resources.Load("prefabs/UI/HP"), transform);
        }
    }

    private void RemovePlaceHolders()
    {
        foreach (Transform HP in transform)
        {
            Destroy(HP.gameObject);
        }
    }
}
