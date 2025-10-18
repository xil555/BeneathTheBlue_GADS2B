using UnityEngine;
using UnityEngine.UI;

public class SceneFadeManager : MonoBehaviour
{
    /*  public static SceneFadeManager instance;

      [SerializeField] private Image _fadeOutImage;
      [Range(0f, 1f), SerializeField] private float _fadeOutSpeed = 5f;
      [Range(0f, 1f), SerializeField] private float _fadeInSpeed = 5f;

      [SerializeField] private Color _fadeOutStartColor;

      public bool IsFadingOut { get; private set; }
      public bool IsFadingIn { get; private set; }

      private void Awake()
      {
          if (instance == null)
          {
              instance = this;
          }

          _fadeOutStartColor.a = 0f;
      }


      // Update is called once per frame
      void Update()
      {
          if (!IsFadingOut)
          {
              if (_fadeOutImage.color.a < 1f)
              {
                  _fadeOutStartColor.a += Time.deltaTime * _fadeInSpeed;
                  _fadeOutImage.color = _fadeOutStartColor;
              }
              else
              {
                  IsFadingIn = false;
              }
          }

          if (IsFadingIn)
          {
              if (_fadeOutImage.color.a > 0f)
              {
                  _fadeOutStartColor.a -= Time.deltaTime * _fadeInSpeed;
                  _fadeOutImage.color = _fadeOutStartColor;
              }
              else
              {
                  IsFadingIn = false;
              }
          }
      }

      public void StartFadeOut()
      {
          _fadeOutImage.color = _fadeOutStartColor;
          IsFadingOut = true;
      }

      public void StartFadeIn()
      {
          if (_fadeOutImage.color.a >= 1f)
          {
              _fadeOutImage.color = _fadeOutStartColor;
              IsFadingIn = true;
          }
      }
    */


    public static SceneFadeManager instance;

    [SerializeField] private Image _fadeOutImage;
    [Range(0.5f, 5f)][SerializeField] private float _fadeOutSpeed = 2f;
    [Range(0.5f, 5f)][SerializeField] private float _fadeInSpeed = 2f;

    [SerializeField] private Color _fadeOutStartColor = Color.black;

    public bool IsFadingOut { get; private set; }
    public bool IsFadingIn { get; private set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        _fadeOutStartColor.a = 0f;
        _fadeOutImage.color = _fadeOutStartColor;
    }

    private void Update()
    {
        // Fade Out
        if (IsFadingOut)
        {
            if (_fadeOutImage.color.a < 1f)
            {
                _fadeOutStartColor.a += Time.deltaTime * _fadeOutSpeed;
                _fadeOutImage.color = _fadeOutStartColor;
            }
            else
            {
                IsFadingOut = false;
            }
        }

        // Fade In
        if (IsFadingIn)
        {
            if (_fadeOutImage.color.a > 0f)
            {
                _fadeOutStartColor.a -= Time.deltaTime * _fadeInSpeed;
                _fadeOutImage.color = _fadeOutStartColor;
            }
            else
            {
                IsFadingIn = false;
            }
        }
    }

    public void StartFadeOut()
    {
        _fadeOutStartColor.a = 0f;
        _fadeOutImage.color = _fadeOutStartColor;
        IsFadingOut = true;
        IsFadingIn = false;
        Debug.Log("[Fade] Fade Out Started");
    }

    public void StartFadeIn()
    {
        _fadeOutStartColor.a = 1f;
        _fadeOutImage.color = _fadeOutStartColor;
        IsFadingIn = true;
        IsFadingOut = false;
        Debug.Log("[Fade] Fade In Started");
    }

}
