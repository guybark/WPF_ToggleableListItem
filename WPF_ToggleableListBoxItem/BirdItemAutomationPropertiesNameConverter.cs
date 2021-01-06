using System;
using System.Globalization;
using System.Windows.Data;

namespace WPF_ToggleableListItem
{
    public class BirdItemAutomationPropertiesNameConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // All important information conveyed visually in the item must be conveyed programmatically. 
            // So all important text must be considered for here, and also any non-textual visuals that
            // convey important information. For example, if the items contains icons to convey information, 
            // then that information must be conveyed programmatically. 

            // In this demo code, all information is represented through text shown in the item. Note that
            // if the item aso displayed static text visually, that text would be included here. And all 
            // text returned from here would be as localized as the visual text. By default, all text would
            // be localized, except for some uncommon cases such as where text is used in branding.

            // The order in which the text is built up here should match the logical order in which customers 
            // will want to access it. So when the customer arrows quickly through the list to reach an item 
            // of interest, the announcement should be ordered to enable that rapid navigation through the items.

            // Note that if some text contained within the item is not typically particularly interesting to
            // customers, that might not be included in the string returned here. That said, the text must
            // still be accessible to customers, and so if it's not included here, there must be some other
            // intuitive and efficient method for all customers to access the text.

            return (values[0] as string) + ", " +
                (values[1] as string) + ", " +
                (values[2] as string);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}