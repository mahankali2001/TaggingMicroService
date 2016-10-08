namespace Persistence.Entities.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;


    public static class DataTableExtension
    {
        public static List<T> LoadFrom<T>(this DataTable table, Func<DataRow, T> loadFromRow) where T : class
        {
            if (table == null)
            {
                return null;
            }

            var entities = new List<T>();
            foreach (DataRow row in table.Rows)
            {
                entities.Add(loadFromRow(row));
            }
            return entities;
        }
    }
}
