using DATA.UTILITIES.Log4Net;
using System;
using System.Collections;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Linq;

namespace DATA.UTILITIES.Reflection
{
    public class Ref
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="windowFullName"></param>
        /// <param name="methodName"></param>
        /// <param name="index"></param>
        /// <param name="param"></param>
        public static void GotoOthersClassLibrary(object obj, string windowFullName, string methodName, int index, params string[] param)
        {
            if (obj != null)
                try
                {
                    //string mainWindowName = windowFullName;
                    object objs = Application.Current.MainWindow.Content;

                    foreach (Window item in Application.Current.Windows)
                    {
                        TypeInfo mainObject = item.GetType() as TypeInfo;
                        if (windowFullName.Equals(mainObject.ToString()))
                        {
                            Grid grid = item.Content as Grid;
                            grid = grid.Children[1] as Grid;
                            StackPanel stackPanel = grid.Children[0] as StackPanel;
                            ToggleButton tbtn = stackPanel.Children[index] as ToggleButton;

                            //ConstructorInfo main = mainObject.GetConstructor(Type.EmptyTypes);
                            //object mainWindowObj = main.Invoke(new object[] { });

                            object mainDatacontext = item.DataContext;
                            IEnumerable imethods = mainObject.DeclaredMethods;

                            var md = imethods.Cast<MethodInfo>().ToList().FirstOrDefault(method => method.Name == methodName) ?? null;
                            if (md != null)
                            {
                                RoutedEventArgs e = new RoutedEventArgs(ToggleButton.ClickEvent);
                                e.Source = obj;
                                md.Invoke(item, new object[] { tbtn, e });
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger<Ref>.Log.Error(ex);
                }
        }
    }
}
