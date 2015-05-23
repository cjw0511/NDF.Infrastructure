using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Entity.Interception
{
    /// <summary>
    /// 定义一个接口 <see cref="IDbConnectionInterceptor"/> 的默认空实现；
    /// <para>提供当在 EF 框架中创建或更改 <see cref="DbConnection"/> 时所引发的通知。</para>
    /// <para>详情请参见接口 <see cref="IDbConnectionInterceptor"/>。</para>
    /// </summary>
    public class NullDbConnectionInterceptor : IDbConnectionInterceptor
    {

        /// <summary>
        /// 在开启一个数据库事务动作完成后瞬间触发。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="interceptionContext"></param>
        public virtual void BeganTransaction(DbConnection connection, BeginTransactionInterceptionContext interceptionContext)
        {
        }

        /// <summary>
        /// 在开启一个数据库事务动作执行前瞬间触发。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="interceptionContext"></param>
        public virtual void BeginningTransaction(DbConnection connection, BeginTransactionInterceptionContext interceptionContext)
        {
        }


        /// <summary>
        /// 在关闭数据库连接动作完成后瞬间触发。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="interceptionContext"></param>
        public virtual void Closed(DbConnection connection, DbConnectionInterceptionContext interceptionContext)
        {
        }

        /// <summary>
        /// 在关闭数据库连接动作执行前瞬间触发。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="interceptionContext"></param>
        public virtual void Closing(DbConnection connection, DbConnectionInterceptionContext interceptionContext)
        {
        }


        /// <summary>
        /// 在获取数据库连接字符串动作执行前瞬间触发。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="interceptionContext"></param>
        public virtual void ConnectionStringGetting(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext)
        {
        }

        /// <summary>
        /// 在获取数据库连接字符串动作完成后瞬间触发。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="interceptionContext"></param>
        public virtual void ConnectionStringGot(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext)
        {
        }


        /// <summary>
        /// 在设置数据库连接字符串动作完成后瞬间触发。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="interceptionContext"></param>
        public virtual void ConnectionStringSet(DbConnection connection, DbConnectionPropertyInterceptionContext<string> interceptionContext)
        {
        }

        /// <summary>
        /// 在设置数据库连接字符串动作执行前瞬间触发。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="interceptionContext"></param>
        public virtual void ConnectionStringSetting(DbConnection connection, DbConnectionPropertyInterceptionContext<string> interceptionContext)
        {
        }


        /// <summary>
        /// 在获取数据库连接超时时间动作执行前瞬间触发。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="interceptionContext"></param>
        public virtual void ConnectionTimeoutGetting(DbConnection connection, DbConnectionInterceptionContext<int> interceptionContext)
        {
        }

        /// <summary>
        /// 在获取数据库连接超时时间动作完成后瞬间触发。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="interceptionContext"></param>
        public virtual void ConnectionTimeoutGot(DbConnection connection, DbConnectionInterceptionContext<int> interceptionContext)
        {
        }


        /// <summary>
        /// 在获取 数据源服务器名称/IP 名称动作执行前瞬间触发。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="interceptionContext"></param>
        public virtual void DataSourceGetting(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext)
        {
        }

        /// <summary>
        /// 在获取 数据源服务器名称/IP 名称动作完成后瞬间触发。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="interceptionContext"></param>
        public virtual void DataSourceGot(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext)
        {
        }


        /// <summary>
        /// 在获取数据库名称动作执行前瞬间触发。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="interceptionContext"></param>
        public virtual void DatabaseGetting(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext)
        {
        }

        /// <summary>
        /// 在获取数据库名称动作完成后瞬间触发。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="interceptionContext"></param>
        public virtual void DatabaseGot(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext)
        {
        }


        /// <summary>
        /// 在数据库连接对象 <see cref="DbConnection"/> 资源销毁动作完成后瞬间触发。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="interceptionContext"></param>
        public virtual void Disposed(DbConnection connection, DbConnectionInterceptionContext interceptionContext)
        {
        }

        /// <summary>
        /// 在数据库连接对象 <see cref="DbConnection"/> 资源销毁动作执行前瞬间触发。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="interceptionContext"></param>
        public virtual void Disposing(DbConnection connection, DbConnectionInterceptionContext interceptionContext)
        {
        }


        /// <summary>
        /// 在数据库操作事务提交动作完成后瞬间触发。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="interceptionContext"></param>
        public virtual void EnlistedTransaction(DbConnection connection, EnlistTransactionInterceptionContext interceptionContext)
        {
        }

        /// <summary>
        /// 在数据库操作事务提交动作执行前瞬间触发。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="interceptionContext"></param>
        public virtual void EnlistingTransaction(DbConnection connection, EnlistTransactionInterceptionContext interceptionContext)
        {
        }


        /// <summary>
        /// 在打开数据库连接动作完成后瞬间触发。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="interceptionContext"></param>
        public virtual void Opened(DbConnection connection, DbConnectionInterceptionContext interceptionContext)
        {
        }

        /// <summary>
        /// 在打开数据库连接动作执行前瞬间触发。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="interceptionContext"></param>
        public virtual void Opening(DbConnection connection, DbConnectionInterceptionContext interceptionContext)
        {
        }


        /// <summary>
        /// 在获取数据库版本信息动作执行前瞬间触发。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="interceptionContext"></param>
        public virtual void ServerVersionGetting(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext)
        {
        }

        /// <summary>
        /// 在获取数据库版本信息动作完成后瞬间触发。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="interceptionContext"></param>
        public virtual void ServerVersionGot(DbConnection connection, DbConnectionInterceptionContext<string> interceptionContext)
        {
        }


        /// <summary>
        /// 在获取数据库连接状态动作执行前瞬间触发。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="interceptionContext"></param>
        public virtual void StateGetting(DbConnection connection, DbConnectionInterceptionContext<ConnectionState> interceptionContext)
        {
        }

        /// <summary>
        /// 在获取数据库连接状态动作完成后瞬间触发。
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="interceptionContext"></param>
        public virtual void StateGot(DbConnection connection, DbConnectionInterceptionContext<ConnectionState> interceptionContext)
        {
        }


    }
}
