using UnityEngine.EventSystems;

namespace DDF.UI.Events {
	public interface IPointerUI : IPointerEnterHandler, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler, IPointerExitHandler { }
}