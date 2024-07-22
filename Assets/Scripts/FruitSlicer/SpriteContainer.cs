using System.Collections.Generic;
using UnityEngine;
using YG;

public class SpriteContainer : MonoBehaviour
{
    [SerializeField] private List<Sprite> _sprites;

    private void Start()
    {
        InitSpriteContainer();
    }

    private void OnEnable() => YandexGame.GetDataEvent += InitSpriteContainer;
    private void OnDisable() => YandexGame.GetDataEvent -= InitSpriteContainer;

    private void InitSpriteContainer()
    {
        YandexGame.savesData.LevelsCount = _sprites.Count;
    }

    public Sprite GetSprite(int spriteIndex)
    {
        if (spriteIndex >= _sprites.Count)
        {
            Debug.Log("��������� ��������� ������� !");
            return _sprites[^1];
        }

        Debug.Log("�������� ����� ������ " + spriteIndex);
        return _sprites[spriteIndex];
    }
}