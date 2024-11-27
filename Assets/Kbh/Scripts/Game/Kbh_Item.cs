using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Kbh_Item : MonoBehaviour
{
   private bool _IsActive = false;
   private ItemSO _itemInfo;
   private TagHandle _playerTag;

   [SerializeField] private Collider _collider;
   [SerializeField] private SpriteRenderer _renderer;

   [SerializeField] private UnityEvent OnInit;
   [SerializeField] public UnityEvent<Kbh_Item> OnDestroy;

   private void Awake()
   {
      _collider = GetComponent<Collider>();
      _renderer = transform.Find("Visual").GetComponent<SpriteRenderer>();

      _playerTag = TagHandle.GetExistingTag("Player");
   }

   public void Update()
   {
      transform.Rotate(0, 50 * Time.deltaTime, 0);
   }

   public void Initialize(ItemSO itemSO)
   {
      _itemInfo = itemSO;
      _IsActive = true;
      _collider.enabled = true;

      _renderer.sprite = itemSO.sprite;
      // _renderer.material.mainTexture = _itemInfo.texture;
      // _renderer.material.color = Color.white;
      // _renderer.material.SetFloat("_Mode", 3);
      // _renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
      // _renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
      // _renderer.material.SetInt("_ZWrite", 0);
      // _renderer.material.DisableKeyword("_ALPHATEST_ON");
      // _renderer.material.EnableKeyword("_ALPHABLEND_ON");
      // _renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
      // _renderer.material.SetFloat("_Surface", 1);
      // _renderer.material.SetFloat("_AlphaClip", 0);
      // _renderer.material.SetFloat("_Blend", 0.0f);
      // _renderer.material.SetFloat("_Cull", 0.0f);
      // _renderer.material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
      // _renderer.material.SetFloat("_SurfaceType", 0.0f);
      // _renderer.material.SetFloat("_RenderQueueType", 1.0f);
      

      OnInit?.Invoke();
   }

   public ItemSO GetItemInfo() => _itemInfo;

   private void OnTriggerEnter(Collider other)
   {
      if (_IsActive && other.CompareTag(_playerTag))
      {
         _collider.enabled = false;
         _renderer.enabled = false;
         _IsActive = false;
      }

      OnDestroy?.Invoke(this);
   }

}
