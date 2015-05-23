using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Entity.Migrations
{
    /// <summary>
    /// 表示与对给定模型使用迁移相关的配置。
    /// </summary>
    /// <typeparam name="TContext">表示一个 <see cref="System.Data.Entity.DbContext"/> 类型。</typeparam>
    public class MigrationConfiguration<TContext> : DbMigrationsConfiguration<TContext> where TContext : System.Data.Entity.DbContext
    {
        /// <summary>
        /// 初始化 <see cref="MigrationConfiguration&lt;TContext&gt;"/> 类型的一个新实例。
        /// </summary>
        public MigrationConfiguration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }
    }
}
