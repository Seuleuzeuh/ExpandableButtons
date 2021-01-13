using ExpandableButtons.Themes;
using Xamarin.Forms;

namespace ExpandableButtons
{
    public static class ExpandableButtonsManager
    {
        public static void Init()
        {
            var resourceDictionary = new Generic();

            if (!Application.Current.Resources.MergedDictionaries.Contains(resourceDictionary))
                Application.Current.Resources.Add(resourceDictionary);
        }
    }
}
