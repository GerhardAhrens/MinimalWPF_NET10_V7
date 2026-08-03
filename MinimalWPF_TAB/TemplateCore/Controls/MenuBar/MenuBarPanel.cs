namespace System.Windows.Controls
{
    using System;

    public class MenuBarPanel : Panel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            double width = 0;
            double height = 0;

            MenuBar menuBar = ItemsControl.GetItemsOwner(this) as MenuBar;

            double spacing = menuBar?.ButtonSpacing ?? 4;
            double minWidth = menuBar?.ItemMinWidth ?? 0;

            bool firstLeft = true;
            bool firstRight = true;

            foreach (UIElement child in InternalChildren)
            {
                if (child is not FrameworkElement element)
                {
                    continue;
                }

                ApplyDefaults(element, menuBar);

                element.Measure(new Size(double.PositiveInfinity, availableSize.Height));

                Thickness margin = element.Margin;

                double itemWidth = Math.Max(minWidth, element.DesiredSize.Width) +  margin.Left + margin.Right;

                if (MenuBar.GetDock(element) == MenuBarDock.Left)
                {
                    if (firstLeft == false)
                    {
                        width += spacing;
                    }

                    firstLeft = false;
                }
                else
                {
                    if (firstRight == false)
                    {
                        width += spacing;
                    }

                    firstRight = false;
                }

                width += itemWidth;

                height = Math.Max(height, element.DesiredSize.Height + margin.Top + margin.Bottom);
            }

            return new Size(width, height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            MenuBar menuBar = ItemsControl.GetItemsOwner(this) as MenuBar;

            double spacing = menuBar?.ButtonSpacing ?? 4;
            double minWidth = menuBar?.ItemMinWidth ?? 0;

            double left = 0;
            double right = finalSize.Width;

            //--------------------------------------------------
            // Linke Seite
            //--------------------------------------------------

            bool first = true;

            foreach (UIElement child in InternalChildren)
            {
                if (child is not FrameworkElement element)
                {
                    continue;
                }

                if (MenuBar.GetDock(element) != MenuBarDock.Left)
                {
                    continue;
                }

                Thickness margin = element.Margin;

                double width = Math.Max(minWidth, element.DesiredSize.Width);

                if (!first)
                    left += spacing;

                first = false;

                left += margin.Left;

                double y =
                    (finalSize.Height -
                     element.DesiredSize.Height) / 2;

                element.Arrange(
                    new Rect(
                        left,
                        y,
                        width,
                        element.DesiredSize.Height));

                left += width + margin.Right;
            }

            //--------------------------------------------------
            // Rechte Seite
            //--------------------------------------------------

            first = true;

            for (int i = InternalChildren.Count - 1; i >= 0; i--)
            {
                if (InternalChildren[i] is not FrameworkElement element)
                    continue;

                if (MenuBar.GetDock(element) != MenuBarDock.Right)
                    continue;

                Thickness margin = element.Margin;

                double width =
                    Math.Max(minWidth,
                             element.DesiredSize.Width);

                if (!first)
                    right -= spacing;

                first = false;

                right -= margin.Right;
                right -= width;

                double y =
                    (finalSize.Height -
                     element.DesiredSize.Height) / 2;

                element.Arrange(
                    new Rect(
                        right,
                        y,
                        width,
                        element.DesiredSize.Height));

                right -= margin.Left;
            }

            return finalSize;
        }

        private static void ApplyDefaults(FrameworkElement element, MenuBar menuBar)
        {
            if (menuBar == null)
                return;

            //--------------------------------------------------
            // MinWidth
            //--------------------------------------------------

            if (double.IsNaN(element.MinWidth) ||
                element.MinWidth == 0)
            {
                element.MinWidth = menuBar.ItemMinWidth;
            }

            //--------------------------------------------------
            // Padding (nur Controls)
            //--------------------------------------------------

            if (element is Control control)
            {
                if (control.Padding == default)
                    control.Padding = menuBar.ItemPadding;
            }

            //--------------------------------------------------
            // Vertikal zentrieren
            //--------------------------------------------------

            if (element.VerticalAlignment == VerticalAlignment.Stretch)
                element.VerticalAlignment = VerticalAlignment.Center;
        }
    }
}
