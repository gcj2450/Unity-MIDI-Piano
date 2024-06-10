using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ComponentBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, 
                                    IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    protected bool isHovered;

    protected virtual void OnClick()
    {

    }
    protected virtual void OnEnter()
    {
        isHovered = true;
    }
    protected virtual void OnExit()
    {
        isHovered = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnExit();
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {

    }
    public virtual void OnEndDrag(PointerEventData eventData)
    {

    }
    public virtual void OnDrag(PointerEventData eventData)
    {

    }

	protected virtual void Awake()
    {

    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void Start()
    {

    }

    protected virtual void OnDisable()
    {

    }

    public virtual void Init()
    {

    }

    public virtual void OnDestroy()
	{
		
	}
}
