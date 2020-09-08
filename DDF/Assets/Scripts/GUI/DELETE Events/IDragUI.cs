using UnityEngine.EventSystems;

namespace DDF.UI.Events {
	public interface IDragUI : IBeginDragHandler, IDragHandler, IEndDragHandler { }
}