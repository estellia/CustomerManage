using JIT.Utility.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace JIT.CPOS.BS.DataAccess.Extension
{
    public static class SqlBulkCopyExtension
    {
        public static int BulkInsert<T>(this Base.BaseCPOSDAO baseDao, IEnumerable<T> collection) where T : BaseEntity
        {
            var dataTable = collection.ToDataTable<T>();

            SqlBulkCopy bulkCopy = new SqlBulkCopy(baseDao);
        }
    }
}
