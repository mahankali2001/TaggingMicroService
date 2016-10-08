namespace Persistence.Entities.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;


    internal static class DataRowExtension
    {
        internal static T FieldWithColumnCheck<T>(this DataRow row, string columnName, T defaultValue = default(T))
        {
            return row.Table.Columns.Contains(columnName) ? row.Field<T>(columnName) : defaultValue;
        }
    }

}
