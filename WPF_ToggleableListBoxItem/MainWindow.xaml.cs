using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF_ToggleableListItem
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Add some test items.
            List<Bird> items = new List<Bird>();
            items.Add(new Bird() { Name = "House Sparrow", Habitat = "Cities, suburbs, farms", Voice = "Repeated series of chirrup sounds" });
            items.Add(new Bird() { Name = "Golden-crowned Sparrow", Habitat = "Brushy places, including neighborhoods", Voice = "Series of long, raspy, whistled notes" });
            items.Add(new Bird() { Name = "Song Sparrow", Habitat = "Found throughout Puget Sound Region, up to mountain passes", Voice = "Song begins with several clear notes, followed by lower note, jumbled trill" });
            BirdList.ItemsSource = items;
        }

        // Whenever the checkbox in the item is checked, the containing item will raise a 
        // UIA event to convey the fact that the toggle state of the item has changed. The 
        // event must be raised regardless of whether the toggle state of the item changed 
        // in response to keyboard, mouse, or programmatic input.
        private void itemCheckBoxToggled(object sender, RoutedEventArgs e)
        {
            var itemCheckBox = sender as CheckBox;
            if (itemCheckBox != null)
            {
                var itemContainer = BirdList.ItemContainerGenerator.ContainerFromItem(itemCheckBox.DataContext) as ListViewItem;
                var isChecked = (e.RoutedEvent == CheckBox.CheckedEvent);

                AutomationPeer itemAutomationPeer =
                    UIElementAutomationPeer.FromElement(itemContainer);
                if (itemAutomationPeer != null)
                {
                    itemAutomationPeer.RaisePropertyChangedEvent(
                        TogglePatternIdentifiers.ToggleStateProperty,
                        isChecked ? ToggleState.Off : ToggleState.On, // Assume the state has actually toggled.
                        isChecked ? ToggleState.On : ToggleState.Off);
                }
            }
        }

        // Change the toggled state of the item when Space is pressed while keyboard 
        // focus is on the item.
        private void itemKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                var bird = (sender as ListViewItem).DataContext as Bird;
                bird.BirdItemIsChecked = !bird.BirdItemIsChecked;
            }
        }

        // Change the toggled state of the item when the mouse clicks on the item.
        private void itemPreviewMouseDown(object sender, MouseEventArgs e)
        {
            var bird = (sender as ListViewItem).DataContext as Bird;
            bird.BirdItemIsChecked = !bird.BirdItemIsChecked;

            e.Handled = true;
        }
    }

    public class ListViewToggleableItems : ListView
    {
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new ListViewToggleableItemsAutomationPeer(this);
        }
    }

    public class ListViewToggleableItemsAutomationPeer : ListViewAutomationPeer
    {
        public ListViewToggleableItemsAutomationPeer(ListView owner)
            : base(owner)
        {
        }

        protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
        {
            return new ToggleableItemAutomationPeer(item, this);
        }
    }

    public class ToggleableItemAutomationPeer : ListBoxItemAutomationPeer, IToggleProvider
    {
        private Bird owner;

        public ToggleableItemAutomationPeer(object item, SelectorAutomationPeer selectorAP)
            : base(item, selectorAP)
        {
            owner = item as Bird;
        }

        protected override List<AutomationPeer> GetChildrenCore()
        {
            var fullChildList = base.GetChildrenCore();

            // Remove the element associated with the CheckBox, as the item itself
            // handles all toggle-related accessibilty.
            fullChildList.RemoveAt(0);

            // Now remove the text element whose name has been set on the item.
            fullChildList.RemoveAt(0);

            return fullChildList;
        }

        public override object GetPattern(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.Toggle)
            {
                return this;
            }

            return base.GetPattern(patternInterface);
        }

        public ToggleState ToggleState 
        {
            get
            {
                return (owner.BirdItemIsChecked ? ToggleState.On : ToggleState.Off); 
            }
            set
            {
                owner.BirdItemIsChecked = (value == ToggleState.On);
            }
        }

        public void Toggle()
        {
            this.owner.BirdItemIsChecked = !this.owner.BirdItemIsChecked;
        }
    }
}
