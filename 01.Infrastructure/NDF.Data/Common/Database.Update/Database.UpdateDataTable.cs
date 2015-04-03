using NDF.Data.EnterpriseLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Common
{
    public abstract partial class Database
    {
        /// <summary>
        /// 以 <paramref name="insertCommand"/>、<paramref name="insertCommand"/>、<paramref name="insertCommand"/> 作为数据处理命令更新 <paramref name="dataTable"/> 中的数据，并返回所影响的行数。
        /// </summary>
        /// <param name="dataTable">待更新数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="insertCommand">表示用于往数据表中插入数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateCommand">表示用于往数据表中修改数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="deleteCommand">表示用于往数据表中删除数据的 <see cref="DbCommand"/> 对象。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        public int UpdateDataTable(DataTable dataTable, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand)
        {
            return DatabaseExtensions.UpdateDataTable(this.PrimitiveDatabase, dataTable, insertCommand, updateCommand, deleteCommand);
        }

        /// <summary>
        /// 以 <paramref name="insertCommand"/>、<paramref name="insertCommand"/>、<paramref name="insertCommand"/> 作为数据处理命令更新 <paramref name="dataTable"/> 中的数据，并返回所影响的行数。
        /// </summary>
        /// <param name="dataTable">待更新数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="insertCommand">表示用于往数据表中插入数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateCommand">表示用于往数据表中修改数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="deleteCommand">表示用于往数据表中删除数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateBehavior">一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.UpdateBehavior"/> 值，用于控制执行 Update 操作时当出现错误时的事务处理机制。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        public int UpdateDataTable(DataTable dataTable, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand, Microsoft.Practices.EnterpriseLibrary.Data.UpdateBehavior updateBehavior)
        {
            return DatabaseExtensions.UpdateDataTable(this.PrimitiveDatabase, dataTable, insertCommand, updateCommand, deleteCommand, updateBehavior);
        }

        /// <summary>
        /// 以 <paramref name="insertCommand"/>、<paramref name="insertCommand"/>、<paramref name="insertCommand"/> 作为数据处理命令更新 <paramref name="dataTable"/> 中的数据，并返回所影响的行数。
        /// </summary>
        /// <param name="dataTable">待更新数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="insertCommand">表示用于往数据表中插入数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateCommand">表示用于往数据表中修改数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="deleteCommand">表示用于往数据表中删除数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateBatchSize">该值启用或禁用批处理支持，并且指定可在一次批处理中执行的命令的数量。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        public int UpdateDataTable(DataTable dataTable, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand, int? updateBatchSize)
        {
            return DatabaseExtensions.UpdateDataTable(this.PrimitiveDatabase, dataTable, insertCommand, updateCommand, deleteCommand, updateBatchSize);
        }

        /// <summary>
        /// 以 <paramref name="insertCommand"/>、<paramref name="insertCommand"/>、<paramref name="insertCommand"/> 作为数据处理命令更新 <paramref name="dataTable"/> 中的数据，并返回所影响的行数。
        /// </summary>
        /// <param name="dataTable">待更新数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="insertCommand">表示用于往数据表中插入数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateCommand">表示用于往数据表中修改数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="deleteCommand">表示用于往数据表中删除数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateBehavior">一个 <see cref="Microsoft.Practices.EnterpriseLibrary.Data.UpdateBehavior"/> 值，用于控制执行 Update 操作时当出现错误时的事务处理机制。</param>
        /// <param name="updateBatchSize">该值启用或禁用批处理支持，并且指定可在一次批处理中执行的命令的数量。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        public virtual int UpdateDataTable(DataTable dataTable, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand, Microsoft.Practices.EnterpriseLibrary.Data.UpdateBehavior updateBehavior, int? updateBatchSize)
        {
            return DatabaseExtensions.UpdateDataTable(this.PrimitiveDatabase, dataTable, insertCommand, updateCommand, deleteCommand, updateBehavior, updateBatchSize);
        }


        /// <summary>
        /// 作为事务处理的一部分以 <paramref name="insertCommand"/>、<paramref name="insertCommand"/>、<paramref name="insertCommand"/> 作为数据处理命令更新 <paramref name="dataTable"/> 中的数据，并返回所影响的行数。
        /// </summary>
        /// <param name="dataTable">待更新数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="insertCommand">表示用于往数据表中插入数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateCommand">表示用于往数据表中修改数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="deleteCommand">表示用于往数据表中删除数据的 <see cref="DbCommand"/> 对象。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        public int UpdateDataTable(DataTable dataTable, DbTransaction transaction, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand)
        {
            return DatabaseExtensions.UpdateDataTable(this.PrimitiveDatabase, dataTable, transaction, insertCommand, updateCommand, deleteCommand);
        }

        /// <summary>
        /// 作为事务处理的一部分以 <paramref name="insertCommand"/>、<paramref name="insertCommand"/>、<paramref name="insertCommand"/> 作为数据处理命令更新 <paramref name="dataTable"/> 中的数据，并返回所影响的行数。
        /// </summary>
        /// <param name="dataTable">待更新数据的 <see cref="DataTable"/> 对象。</param>
        /// <param name="transaction">用于包含脚本命令的 <see cref="DbTransaction"/> 对象。</param>
        /// <param name="insertCommand">表示用于往数据表中插入数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateCommand">表示用于往数据表中修改数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="deleteCommand">表示用于往数据表中删除数据的 <see cref="DbCommand"/> 对象。</param>
        /// <param name="updateBatchSize">该值启用或禁用批处理支持，并且指定可在一次批处理中执行的命令的数量。</param>
        /// <returns>表示脚本命令执行受影响的行数。</returns>
        public virtual int UpdateDataTable(DataTable dataTable, DbTransaction transaction, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand, int? updateBatchSize)
        {
            return DatabaseExtensions.UpdateDataTable(this.PrimitiveDatabase, dataTable, transaction, insertCommand, updateCommand, deleteCommand, updateBatchSize);
        }

    }
}
