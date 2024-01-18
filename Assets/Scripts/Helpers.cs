using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace
{
    public class Helpers
    {
        private static LayerMask UILayer = 5;
        //Returns 'true' if we touched or hovering on Unity UI element.
        public static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
        {
            for (int index = 0; index < eventSystemRaysastResults.Count; index++)
            {
                RaycastResult curRaysastResult = eventSystemRaysastResults[index];
                if (curRaysastResult.gameObject.layer == UILayer)
                    return true;
            }
            return false;
        }
    }
}