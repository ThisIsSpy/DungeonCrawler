using ResourceLoaderSystem;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerSystem 
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Button playerButton;
        [SerializeField] private Image playerImage;
        private Texture2D texture;

        void Start()
        {
            if (playerButton != null) 
            {
                texture = ResourceLoader.Load<Texture2D>("Untitled");
                Rect rect = new(0, 0, texture.width, texture.height);
                Vector2 pivot = new(0.5f, 0.5f);
                float pixelsPerUnit = 100f;
                Sprite newSprite = Sprite.Create(texture, rect, pivot, pixelsPerUnit);
                playerButton.GetComponent<Image>().sprite = newSprite;
                playerButton.onClick.AddListener(DisplayImage);
            } 
        }

        void OnDestroy()
        {
            ResourceLoader.DisposeReference("Untitled");
        }

        private void DisplayImage()
        {
            if(playerImage != null)
            {
                Rect rect = new(0, 0, texture.width, texture.height);
                Vector2 pivot = new(0.5f, 0.5f);
                float pixelsPerUnit = 100f;
                Sprite newSprite = Sprite.Create(texture, rect, pivot, pixelsPerUnit);
                playerImage.sprite = newSprite;
            }
        }
    }
}
