using UnityEngine.EventSystems;

namespace DDF.Events {
	public interface IDragUI : IBeginDragHandler, IDragHandler, IEndDragHandler { }
}