
NDF（Net Dirk Framework） EntityFramework 的数据库主从读写分离服务插件发布说明：

一、版本信息和源码：
    1、版本信息
        v1.01 beta（2015-04-07），基于 EF 6.1 开发，支持 EF 6.1 之后的所有 EF6 版本。

    2、开放源码地址
        https://github.com/cjw0511/NDF.Infrastructure
        关于该组件源码位于文件夹：src/NDF.Data.Entity/MasterSlaves 文件夹中。

    3、该组件源码采用 GPL 协议进行共享；


二、功能概述：
    1、支持在基于 EF6 进行数据操作时：
        a、针对所有的数据写入操作，自动将请求转发至 主服务器（Master，即写入操作服务器）；
        b、针对所有的数据查询操作，自动将请求转发至 从服务器（Slave ，即查询操作服务器）；
        c、以上的数据库操作请求转发通过在执行命令前更改数据库连接字符串来完成，但是该数据连接字符串的更改动作，不需要业务开发人员改动任何现有代码；

    2、在将读写命令请求转发至相应数据库服务器时，支持一主多从管理
        即可以设定一台数据库服务器作为 Master 服务器，同时可以设置一台或者多台数据库服务器作为 Slave 服务器；
        注：Master 服务器和 Slave 服务器之间的需提前建立数据同步机制，该部分工作可通过配置 DBMS 系统来完成。

    3、支持自动检测服务器运行状态：
        a、可自动检测 Master 服务器的在线状态；
        b、可自动检测设定的数据库服务器列表中每台数据库服务器节点的在线状态；
        c、可自定义设定自动检测服务器状态的时间频率；
        d、可自定义在每次检测每个服务器节点可用状态时，是否排除检测已经标记为离线的数据库服务器节点

    4、支持根据自动检测服务器运行状态结果将数据查询请求在 Slave 服务器节点不可用时自动切换至 Master 节点：
        如果设置了多台 Slave 服务器节点，将在每次执行查询操作时，根据自动检测的 Slave 服务器在线状态自动选择可用的服务器节点；
        如果所有的 Slave 都不可用，则可以根据配置确定是否自动将数据查询操作切换至 Master 服务器；

    5、支持根据自动检测服务器运行状态结果将数据变更请求在 Master 服务器节点不可用时自动切换至 Slave 节点：
        在基于 EF6 的数据更改操作时，如果检测到 Master 服务器状态不可用，则可以根据配置确定是否自动将数据更改操作切换至 Slave 服务器列表中的第一个
        可用项（一般情况下不建议进行该设定，因为将 Slave 服务器作为 Master 服务器使用虽然能使在 Master 故障后应用程序不离线，但是同样也会带来
        在 Slave 服务器节点之间的数据一致性问题。）；

    6、支持多台 Slave 节点之间的负载均衡：
        如果设定了多台 Slave 服务器节点，在每次执行查询操作时，支持按照如下两种方式选择 Slave 服务器节点以执行查询命令：
        a、按设定顺序选择第一台可用的 Slave 服务器节点；
        b、从所有可用的 Slave 服务器节点中随机选择一台，这可以有效分散 Slave 服务器查询压力（默认设置）；

    7、支持面向切面的 EF 数据库主从读写分离服务动作拦截器配置：
        可通过定义和注册实现接口 IMasterSlaveInterceptor 的拦截器，以实现在 EF 数据库主从读写分离服务执行特定动作：
        扫描服务器节点可用状态前、扫描服务器节点可用状态后、修改数据库操作命令的连接字符串前、修改数据库操作命令的连接字符串后
        时以执行用户指定的附加动作（例如在扫描到服务器节点不可用时自动记录日志或发送消息通知）。

    8、支持 EF 中的多 DbContext 类型配置：
        如果项目中使用多种类型的 EF 实体上下文（System.Data.Entity.DbContext） 对象，支持为每个不同类型的 DbContext 分别配置不同的主从读写分离数据库
        连接方案；

    9、支持 Master 服务器节点和 Slave 服务器节点的热插拔配置：
        即可以不用停止项目的运行，直接通过修改配置文件 ef.masterslave.config 中的内容，来达到自动刷新相关配置连接的效果；
        在修改配置文件后并重新生效时，支持自定义的更改事件通知。


三、使用说明：
    1、设置多个数据库服务器实例之间的自动同步
        首选通过数据库管理系统（DBMS）来配置多个数据库服务器实例之间的主从自动同步机制，例如：
        a、如果是用 MSSQLSERVER 数据库系统，可以配置多台数据库服务器实例之间的复制、订阅策略；
        b、如果是用 MySQL 数据库系统，可以配置多台数据库服务器实例之间的主从复制策略；
        c、其他 Oracle、DB2...

    2、在项目中添加配置文件
        在项目根目录下添加配置文件 ef.masterslave.config，并按规则修改其中的内容，以下是一份参考的配置方式：

        <?xml version="1.0" encoding="utf-8" ?>
        <configuration>
          <configSections>
            <section name="ef.masterslave" type="NDF.Data.Entity.MasterSlaves.ConfigFile.EFMasterSlaveSection, NDF.Data.Entity"
                     requirePermission="false" />
          </configSections>
          <ef.masterslave>
            <!-- 以下配置节对所有用 EF 实体上下文（DbContext）类型为 MyProject.Data.MyDbContext 的数据库操作配置主从读写分离服务  -->
            <applyItem targetContext="MyProject.Data.MyDbContext, MyProject.Data"
                       autoSwitchSlaveOnMasterFauled="false" autoSwitchMasterOnSlavesFauled="true" 
                       serverStateScanInterval="60" serverStateScanWithNonOffline="false"
                       slaveRandomization="true" >
              <master connectionString="server=192.168.0.99;port=3306;user id=root;password=123456;persistsecurityinfo=True;database=testdb;convertzerodatetime=True;allowzerodatetime=True" />
              <slaves>
                <add connectionString="server=192.168.0.101;port=3306;user id=root;password=123456;persistsecurityinfo=True;database=testdb;convertzerodatetime=True;allowzerodatetime=True" order="0" />
                <add connectionString="server=192.168.0.102;port=3306;user id=root;password=123456;persistsecurityinfo=True;database=testdb;convertzerodatetime=True;allowzerodatetime=True" order="1" />
                <add connectionString="server=192.168.0.103;port=3306;user id=root;password=123456;persistsecurityinfo=True;database=testdb;convertzerodatetime=True;allowzerodatetime=True" order="2" />
              </slaves>
            </applyItem>
            <!-- 以下配置节对另一个 EF 实体上下文（DbContext）配置主从读写分离服务  -->
            <!--<applyItem ...>
              <master ... />
              <slaves>
                <add ... />
                <add ... />
              </slaves>
            </applyItem>-->
          </ef.masterslave>
        </configuration>

    3、在项目中引入依赖的程序包
        在项目中分别引入如下 dll 库文件（或 Nuget 包）：
        a、EntityFramework 6.1 以上版本；
        b、Microsoft Enterprise Library - Data Access Application Block 6；
        c、Newtonsoft.Json.dll 6.0 以上版本；
        d、NDF.Utilities.dll；
        e、NDF.Data.dll；
        f、NDF.Data.Entity.dll；

    4、在项目中添加启动代码
        在项目的启动代码中（控制台和桌面程序一般为 Program 类型的 Main 方法、ASP.NET 程序一般为 Global.asax 文件的 Application_Start 代码块）加入如下代码段：
        NDF.Data.Entity.MasterSlaves.EFMasterSlaveConfig.Register(typeof(MyDbContext));

        其中方法中传入的类型参数应该是 ef.masterslave.config 配置文件中 applyItem 节的 targetContext 属性所示的类型，表示要为具体
        哪个类型的 EF 实体上下文（DbContext） 配置读写分离服务。


四、注意事项：
    1、关于主从数据库中相关数据内容的自动同步机制，由数据库管理系统（DBMS，如 MSSQLSERVER、Oracle、MySQL、DB2 等）来完成，该部分的功能不由本插件来提供；
        目前几乎所有的主流 DBMS 系统都提供了主从数据库自动同步机制相关功能；

    2、该 EF 数据库主从读写分离方案支持所有普通数据库事务和分布式事务操作，不过分布式事务也同样需要数据库管理系统（DBMS）的支持否则无效；

    3、在基于 EF6 和该插件的配合进行数据库主从读写分离操作，程序会自动检测所执行的数据库操作的事务状态，并自动将带有数据库事务或分布式事务的
        所有 增删改请求 和 查询请求 都转发至 Master 服务器。

    4、本篇文章只是概述性的介绍了本人编写的这个 EF 数据库主从读写分离插件，关于该插件的源码实现原理和思路，本人将会在以后的博文中展开介绍。






