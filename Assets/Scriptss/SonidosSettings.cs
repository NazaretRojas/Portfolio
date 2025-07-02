using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SonidosSettings : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    
    [SerializeField] private Image musicFillImage; 
    [SerializeField] private Image sfxFillImage;  

    private RectTransform musicRectTransform;
    private RectTransform sfxRectTransform;

    private enum BarraActiva { Ninguna, Musica, Efectos }
    private BarraActiva barraActiva = BarraActiva.Ninguna;

    private void Awake()
    {
        if (musicFillImage == null)
        {
            Debug.LogError("musicFillImage ES NULL");
        }
        else
        {
            Debug.Log("musicFillImage asignado correctamente");
            musicRectTransform = musicFillImage.GetComponent<RectTransform>();
            Debug.Log("musicRectTransform obtenido: " + (musicRectTransform != null));
        }

        if (sfxFillImage == null)
        {
            Debug.LogError("sfxFillImage ES NULL");
        }
        else
        {
            Debug.Log("sfxFillImage asignado correctamente");
            sfxRectTransform = sfxFillImage.GetComponent<RectTransform>();
            Debug.Log("sfxRectTransform obtenido: " + (sfxRectTransform != null));
        }
    }

    private void Start()
    {
        ActualizarBarrasyVolumen();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        DeterminarBarraActiva(eventData);
        if (barraActiva != BarraActiva.Ninguna)
        {
            ActualizarVolumen(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (barraActiva != BarraActiva.Ninguna)
        {
            ActualizarVolumen(eventData);
        }
    }

    private void DeterminarBarraActiva(PointerEventData eventData)
    {
        
        if (RectTransformUtility.RectangleContainsScreenPoint(musicRectTransform, eventData.position, eventData.pressEventCamera))
        {
            barraActiva = BarraActiva.Musica;
        }
        
        else if (RectTransformUtility.RectangleContainsScreenPoint(sfxRectTransform, eventData.position, eventData.pressEventCamera))
        {
            barraActiva = BarraActiva.Efectos;
        }
        else
        {
            barraActiva = BarraActiva.Ninguna;
        }
    }

    private void ActualizarVolumen(PointerEventData eventData)
    {
        RectTransform barraRect;
        Image fillImage;

        if (barraActiva == BarraActiva.Musica)
        {
            barraRect = musicRectTransform;
            fillImage = musicFillImage;
        }
        else if (barraActiva == BarraActiva.Efectos)
        {
            barraRect = sfxRectTransform;
            fillImage = sfxFillImage;
        }
        else
        {
            return;
        }

        Vector2 localPoint;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(barraRect, eventData.position, eventData.pressEventCamera, out localPoint))
        {
            float width = barraRect.rect.width;
            float x = Mathf.Clamp(localPoint.x, -width / 2, width / 2);

            
            float normalized = 1 - ((x + width / 2) / width);

            
            fillImage.fillAmount = normalized;

            if (SoundManager.Instance != null)
            {
                if (barraActiva == BarraActiva.Musica)
                    SoundManager.Instance.SetMusicVolume(normalized);
                else
                    SoundManager.Instance.SetSfxVolume(normalized);
            }
        }
    }

    public void ActualizarBarrasyVolumen()
    {
        if (SoundManager.Instance != null)
        {
            musicFillImage.fillAmount = SoundManager.Instance.MusicVolume;
            sfxFillImage.fillAmount = SoundManager.Instance.SfxVolume;
        }
    }
}