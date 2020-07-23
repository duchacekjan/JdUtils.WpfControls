using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace JdUtils.WpfControls.Utils
{
    public static class UIExtensions
    {
        public static LinearGradientBrush AddGradient(this LinearGradientBrush brush, Color color, double offset = 1)
        {
            if (brush != null)
            {
                var gradient = new GradientStop(color, offset);
                brush.GradientStops.Add(gradient);
            }
            return brush;
        }

        public static IntPtr GetHWND(this DependencyObject element)
        {
            var window = Window.GetWindow(element);
            var result = IntPtr.Zero;
            if (window != null)
            {
                result = new WindowInteropHelper(window).EnsureHandle();
            }
            return result;
        }

        /// <summary>
        /// Nastaví hodnotu property na jiném vlákně, aby došlo ke správnému propsání zpět do modelu.
        /// Hlavní využití je pro coerce nastavované hodnoty z modelu
        /// </summary>
        /// <typeparam name="TClass">Třída, na které se nachází DependencyProperty</typeparam>
        /// <typeparam name="TProperty">DependencyProperty (její hodnota)</typeparam>
        /// <param name="source"></param>
        /// <param name="expr"></param>
        /// <param name="value"></param>
        public static void SetPropertyBackToModel<TClass, TProperty>(this TClass source, Expression<Func<TClass, TProperty>> expr, TProperty value)
            where TClass : DependencyObject
        {
            source?.Dispatcher?.InvokeOnBackground(() =>
            {
                if (expr.Body is MemberExpression member)
                {
                    if (member.Member is PropertyInfo propInfo)
                    {
                        propInfo.SetValue(source, value);
                    }
                }
            });
        }

        /// <summary>
        /// Vyvolání akce na UI vlákně
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <param name="action"></param>
        private static void InvokeOnBackground(this Dispatcher dispatcher, Action action)
        {
            Task.Run(async () =>
            {
                await Task.CompletedTask;
                dispatcher?.Invoke(action);
            });
        }

        /// <summary>
        /// Nahradí první nalezený substring
        /// </summary>
        /// <param name="text"></param>
        /// <param name="search"></param>
        /// <param name="replace"></param>
        /// <returns></returns>
        public static string ReplaceFirst(this string text, string search, string replace)
        {
            return text.ReplaceFirst(search, replace, 0);
        }

        /// <summary>
        /// Nahradí první nalezený substring
        /// </summary>
        /// <param name="text"></param>
        /// <param name="search"></param>
        /// <param name="replace"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static string ReplaceFirst(this string text, string search, string replace, int offset)
        {
            var result = text;
            const string pattern = "^(.*?){0}";
            if (!string.IsNullOrEmpty(replace))
            {
                var regex = new System.Text.RegularExpressions.Regex(string.Format(pattern, search));
                result = regex.Replace(text, replace, 1, offset);
            }

            return result;
        }
    }
}
