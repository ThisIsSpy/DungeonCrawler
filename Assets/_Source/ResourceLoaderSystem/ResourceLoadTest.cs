using UnityEngine;
using UnityEngine.UI;

namespace ResourceLoaderSystem 
{
    public class ResourceLoadTest : MonoBehaviour
    {
        [SerializeField] private Image testImage;
        [SerializeField] private AudioSource testAudioSource;
        private Texture2D texture;
        private AudioClip clip;

        void Start()
        {
            texture = ResourceLoader.Load<Texture2D>("Untitled");
            Rect rect = new(0, 0, texture.width, texture.height);
            Vector2 pivot = new(0.5f, 0.5f);
            float pixelsPerUnit = 100f;
            Sprite newSprite = Sprite.Create(texture, rect, pivot, pixelsPerUnit);
            testImage.sprite = newSprite;

            clip = ResourceLoader.Load<AudioClip>("TheVoiceSomeoneCalls");
            testAudioSource.clip = clip;
            testAudioSource.loop = true;
            testAudioSource.Play();
        }

        void OnDestroy()
        {
            ResourceLoader.DisposeReference("Untitled");
            ResourceLoader.DisposeReference("TheVoiceSomeoneCalls");
        }
    }
}