﻿using JdUtils.Extensions;
using JdUtils.WpfControls.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace JdUtils.WpfControls.Components
{
    [ContentProperty(nameof(Items))]
    public class TagControl : Control
    {
        public const string PartItemsSource = "PART_ItemsSource";
        public const string PartItems = "PART_Items";

        public static readonly DependencyProperty ItemsSourceProperty;
        public static readonly DependencyProperty ItemsProperty;
        private ItemsControl m_itemsSourceControl;
        private ItemsControl m_itemsControl;

        static TagControl()
        {
            var owner = typeof(TagControl);
            ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable<ITag>), owner, new FrameworkPropertyMetadata(OnItemsSourcePropertyChangedCallback));
            ItemsProperty = DependencyProperty.Register(nameof(Items), typeof(ObservableCollection<Tag>), owner, new UIPropertyMetadata(OnItemsSourcePropertyChangedCallback));
            DefaultStyleKeyProperty.OverrideMetadata(owner, new FrameworkPropertyMetadata(owner));
        }

        public TagControl()
        {
            Items = new ObservableCollection<Tag>();
        }

        public ObservableCollection<Tag> Items
        {
            get => (ObservableCollection<Tag>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public IEnumerable<ITag> ItemsSource
        {
            get => (IEnumerable<ITag>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            m_itemsSourceControl = this.FindTemplatePart<ItemsControl>(PartItemsSource);
            m_itemsControl = this.FindTemplatePart<ItemsControl>(PartItems);
        }

        private static void OnItemsSourcePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TagControl control)
            {
                if (!Equals(e.OldValue, e.NewValue))
                {
                    control.OnItemsSourceChanged(e.Property.Name);
                }
            }

        }

        private void OnItemsSourceChanged(string name)
        {
            switch (name)
            {
                case nameof(ItemsSource):
                    if (Items.Count > 0 && ItemsSource != null)
                    {
                        //TODO
                        throw new InvalidOperationException("Kolekce Items musi byt prazdna");
                    }
                    m_itemsSourceControl.SetValueSafe(s => s.Visibility, Visibility.Visible);
                    m_itemsControl.SetValueSafe(s => s.Visibility, Visibility.Collapsed);
                    break;
                case nameof(Items):
                    if (Items.Count > 0 && ItemsSource != null)
                    {
                        //TODO
                        throw new InvalidOperationException("Kolekce ItemsSource nesmi byt pouzita");
                    }
                    m_itemsControl.SetValueSafe(s => s.Visibility, Visibility.Visible);
                    m_itemsSourceControl.SetValueSafe(s => s.Visibility, Visibility.Collapsed);
                    break;
                default:
                    break;
            }
        }
    }
}
