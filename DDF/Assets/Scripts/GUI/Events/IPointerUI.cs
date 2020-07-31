using UnityEngine.EventSystems;

namespace DDF.Events {
	public interface IPointerUI : IPointerEnterHandler, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler, IPointerExitHandler { }
}