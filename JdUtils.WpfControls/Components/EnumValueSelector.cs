﻿using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using JdUtils.MarkupExtensions;

namespace JdUtils.WpfControls.Components
{
    public class EnumValueSelector : ComboBox
    {
        public static readonly DependencyProperty EnumTypeProperty;

        static EnumValueSelector()
        {
            var owner = typeof(EnumValueSelector);
            EnumTypeProperty = DependencyProperty.Register(nameof(EnumType), typeof(Type), owner, new FrameworkPropertyMetadata(OnEnumTypeChangedCallback));
        }

        public EnumValueSelector()
        {
            DisplayMemberPath = "Value";
            SelectedValuePath = "Key";
        }

        public Type EnumType
        {
            get => (Type)GetValue(EnumTypeProperty);
            set => SetValue(EnumTypeProperty, value);
        }

        private static void OnEnumTypeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is EnumValueSelector control)
            {
                if (!Equals(e.OldValue, e.NewValue))
                {
                    control.OnEnumTypeChanged();
                }
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            OnEnumTypeChanged(true);
        }

        private void OnEnumTypeChanged(bool fromApplyTemplate = false)
        {
            System.Diagnostics.Debug.WriteLine($"{GetType().Name}: OnEnumTypeChanged ({fromApplyTemplate})");
            if (EnumType != null)
            {
                var markup = new EnumValues
                {
                    EnumType = EnumType
                };

                ItemsSource = (IEnumerable)markup.ProvideValue(null);
            }
            else
            {
                ItemsSource = null;
            }

            System.Diagnostics.Debug.WriteLine($"{GetType().Name}: OnEnumTypeChanged.ItemsSourceGenerated ({fromApplyTemplate}) Count:{ItemsSource.Cast<object>().Count()}");
        }
    }
}
