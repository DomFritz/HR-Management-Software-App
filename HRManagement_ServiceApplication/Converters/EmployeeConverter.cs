﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace HRManagement_ServiceApplication
{
    public class EmployeeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Clone();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
            // this method isn't needed for us at this time.
        }
    }
}
