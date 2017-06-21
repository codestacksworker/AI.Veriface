using System;
using System.Windows;
using System.Windows.Input;

namespace DATA.UTILITIES.Accessories
{
    public class KeyOpter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnClose(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers.CompareTo(ModifierKeys.Control) == 0 && e.Key == Key.Delete)
            {
                Application.Current.Shutdown();
                Environment.Exit(0);
            }
        }



    }
}
