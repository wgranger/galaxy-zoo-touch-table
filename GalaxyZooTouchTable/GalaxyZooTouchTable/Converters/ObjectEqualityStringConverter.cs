﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace GalaxyZooTouchTable.Converters
{
    public class ObjectEqualityStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return value.GetType().Name == parameter as string;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}