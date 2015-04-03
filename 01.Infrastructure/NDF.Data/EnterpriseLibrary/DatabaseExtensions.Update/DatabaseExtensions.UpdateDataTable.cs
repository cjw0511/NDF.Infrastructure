using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace NDF.Data.EnterpriseLibrary
{
    public static partial class DatabaseExtensions
    {

        /// <summary>
        /// 以 <paramref name="insertCommand"/>、<paramref name="insertCommand"/>、<paramref name="insertCommand"/> 作为数据处理命令更新 <paramref name="dataTable"/> 中的数据，并返回所影响的行数。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="dataTable">待更新数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="insertCommand">表示用于往数据表中插入数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateCommand">表示用于往数据表中修改数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="deleteCommand">表示用于往数据表中删除数据的 <see cref="DbCommand"/> 对象。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        public static int UpdateDataTable(this Database database, DataTable dataTable, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand)
        {
            return UpdateDataTable(database, dataTable, insertCommand, updateCommand, deleteCommand, UpdateBehavior.Standard);
        }

        /// <summary>
        /// 以 <paramref name="insertCommand"/>、<paramref name="insertCommand"/>、<paramref name="insertCommand"/> 作为数据处理命令更新 <paramref name="dataTable"/> 中的数据，并返回所影响的行数。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="dataTable">待更新数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="insertCommand">表示用于往数据表中插入数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateCommand">表示用于往数据表中修改数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="deleteCommand">表示用于往数据表中删除数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateBehavior">一个 <see cref="UpdateBehavior"/> 值，用于控制执行 Update 操作时当出现错误时的事务处理机制。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        public static int UpdateDataTable(this Database database, DataTable dataTable, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand, UpdateBehavior updateBehavior)
        {
            return UpdateDataTable(database, dataTable, insertCommand, updateCommand, deleteCommand, UpdateBehavior.Standard, null);
        }

        /// <summary>
        /// 以 <paramref name="insertCommand"/>、<paramref name="insertCommand"/>、<paramref name="insertCommand"/> 作为数据处理命令更新 <paramref name="dataTable"/> 中的数据，并返回所影响的行数。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="dataTable">待更新数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="insertCommand">表示用于往数据表中插入数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateCommand">表示用于往数据表中修改数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="deleteCommand">表示用于往数据表中删除数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateBatchSize">该值启用或禁用批处理支持，并且指定可在一次批处理中执行的命令的数量。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        public static int UpdateDataTable(this Database database, DataTable dataTable, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand, int? updateBatchSize)
        {
            return UpdateDataTable(database, dataTable, insertCommand, updateCommand, deleteCommand, UpdateBehavior.Standard, updateBatchSize);
        }

        /// <summary>
        /// 以 <paramref name="insertCommand"/>、<paramref name="insertCommand"/>、<paramref name="insertCommand"/> 作为数据处理命令更新 <paramref name="dataTable"/> 中的数据，并返回所影响的行数。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="dataTable">待更新数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="insertCommand">表示用于往数据表中插入数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateCommand">表示用于往数据表中修改数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="deleteCommand">表示用于往数据表中删除数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateBehavior">一个 <see cref="UpdateBehavior"/> 值，用于控制执行 Update 操作时当出现错误时的事务处理机制。</param>
        /// <param name="updateBatchSize">该值启用或禁用批处理支持，并且指定可在一次批处理中执行的命令的数量。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        public static int UpdateDataTable(this Database database, DataTable dataTable, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand, UpdateBehavior updateBehavior, int? updateBatchSize)
        {
            using (var wrapper = GetOpenConnection(database))
            {
                if (updateBehavior == UpdateBehavior.Transactional && Transaction.Current == null)
                {
                    DbTransaction transaction = wrapper.Connection.BeginTransaction();
                    try
                    {
                        int rowsAffected = UpdateDataTable(database, dataTable, transaction, insertCommand, updateCommand, deleteCommand, updateBatchSize);
                        transaction.Commit();
                        return rowsAffected;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
                if (insertCommand != null)
                {
                    PrepareCommand(insertCommand, wrapper.Connection);
                }
                if (updateCommand != null)
                {
                    PrepareCommand(updateCommand, wrapper.Connection);
                }
                if (deleteCommand != null)
                {
                    PrepareCommand(deleteCommand, wrapper.Connection);
                }
                return DoUpdateDataTable(database, dataTable,
                                       insertCommand, updateCommand, deleteCommand, updateBehavior, updateBatchSize);
            }
        }


        /// <summary>
        /// 作为事务处理的一部分以 <paramref name="insertCommand"/>、<paramref name="insertCommand"/>、<paramref name="insertCommand"/> 作为数据处理命令更新 <paramref name="dataTable"/> 中的数据，并返回所影响的行数。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="dataTable">待更新数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="insertCommand">表示用于往数据表中插入数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateCommand">表示用于往数据表中修改数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="deleteCommand">表示用于往数据表中删除数据的 <see cref="DbCommand"/> 对象。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        public static int UpdateDataTable(this Database database, DataTable dataTable, DbTransaction transaction, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand)
        {
            return UpdateDataTable(database, dataTable, transaction, insertCommand, updateCommand, deleteCommand, null);
        }

        /// <summary>
        /// 作为事务处理的一部分以 <paramref name="insertCommand"/>、<paramref name="insertCommand"/>、<paramref name="insertCommand"/> 作为数据处理命令更新 <paramref name="dataTable"/> 中的数据，并返回所影响的行数。
        /// </summary>
        /// <param name="database">表示一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Database"/> 对象。</param>
        /// <param name="dataTable">待更新数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="insertCommand">表示用于往数据表中插入数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateCommand">表示用于往数据表中修改数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="deleteCommand">表示用于往数据表中删除数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateBatchSize">该值启用或禁用批处理支持，并且指定可在一次批处理中执行的命令的数量。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        public static int UpdateDataTable(this Database database, DataTable dataTable, DbTransaction transaction, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand, int? updateBatchSize)
        {
            if (insertCommand != null)
            {
                PrepareCommand(insertCommand, transaction);
            }
            if (updateCommand != null)
            {
                PrepareCommand(updateCommand, transaction);
            }
            if (deleteCommand != null)
            {
                PrepareCommand(deleteCommand, transaction);
            }
            return DoUpdateDataTable(database, dataTable, insertCommand, updateCommand, deleteCommand, UpdateBehavior.Transactional, updateBatchSize);
        }

    }
}
