using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace WebSocketMessaging
{
    public static class ScrollViewerHelper
    {
        private static Dictionary<FlowDocumentScrollViewer, ScrollViewer> cache = new Dictionary<FlowDocumentScrollViewer, ScrollViewer>();

        public static ScrollViewer GetScrollViewer(this FlowDocumentScrollViewer element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }
            if (!cache.ContainsKey(element))
            {
                cache[element] = element.Template?.FindName("PART_ContentHost", element) as ScrollViewer;
            }
            return cache[element];
        }

        public static void ScrollToEnd(this FlowDocumentScrollViewer element)
        {
            element.GetScrollViewer().ScrollToEnd();
        }

        public static bool IsAtHome(this ScrollViewer element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return element.VerticalOffset <= 0;
        }

        public static bool IsAtEnd(this ScrollViewer element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return element.VerticalOffset >= element.ScrollableHeight;
        }


        public static ScrollViewer GetScrollViewer(this Visual visual)
        {
            var scrollViewer = visual as ScrollViewer;
            if (scrollViewer != null)
                return scrollViewer;

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
            {
                var child = VisualTreeHelper.GetChild(visual, i) as Visual;
                if (child == null) continue;

                var searchChild = GetScrollViewer(child);
                if (searchChild != null)
                {
                    return searchChild;
                }
            }

            return null;
        }
    }
}
