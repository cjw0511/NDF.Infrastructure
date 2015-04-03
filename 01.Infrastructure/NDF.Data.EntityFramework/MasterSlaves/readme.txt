
基于 EntityFramework 的数据库主从读写分离服务插件发布说明：

一、版本信息和源码：v1.0 beta（2015-04-02），基于 EF 6.1 开发，支持 EF 6.1 之后的所有 EF6 版本。

二、功能概述：
    1、支持在基于 EF6 进行数据操作时：
        a、针对所有的数据写入操作，自动将请求转发至 主服务器（Master，即写入操作服务器）；
        b、针对所有的数据查询操作，自动将请求转发至 从服务器（Slave ，即查询操作服务器）；
        c、以上的数据库操作请求转发通过在执行命令前更改数据库连接字符串来完成，但是该数据连接字符串的更改动作，不需要业务开发人员改动任何现有代码；

    2、在将读写命令请求转发至相应数据库服务器时，支持一主多从管理；
        即可以设定一台数据库服务器作为 Master 服务器，同时可以设置一台或者多台数据库服务器作为 Slave 服务器；
        （注：Master 服务器和 Slave 服务器之间的需提前建立数据同步机制，该部分工作可通过配置 DBMS 系统来完成）

    3、支持自动检测 Master 服务器的在线状态、自动检测设定的 Slave 服务器列表中每台 Slave 服务器节点的在线状态，支持自定义设定自动检测服务器状态的时间频率；

    4、如果设置了多台 Slave 服务器节点，将在每次执行查询操作时，根据自动检测的 Slave 服务器在线状态自动选择可用的服务器节点；
        如果所有的 Slave 都不可用，则可以根据配置确定是否自动将数据查询操作切换至 Master 服务器；

    5、如果设定了多台 Slave 服务器节点，在每次执行查询操作时，支持按照设定顺序选择第一台可用的 Slave 服务器，也支持随机选择所有可用的 Slave 服务器中
        任意一台（该设置可以有效分散 Slave 服务器查询压力）以执行查询命令。

    6、支持在基于 EF6 的数据更改操作时，如果检测到 Master 服务器状态不可用，则可以根据配置确定是否自动将数据更改操作切换至 Slave 服务器列表中的
        第一个可用项（一般情况下不建议进行该设定，因为将 Slave 服务器作为 Master 服务器使用虽然能使在 Master 故障后应用程序不离线，但是同样
        也会带来在 Slave 服务器节点之间的数据一致性问题。）；

    7、如果项目中使用多种类型的  EF 实体上下文（System.Data.Entity.DbContext） 对象，支持为每个不同类型的 DbContext 分别配置不同的主从读写分离
        数据库连接方案；

    8、支持在程序正常运行时，以热插拔的方式配置读写分离数据库连接：
        即可以不用停止项目的运行，直接通过修改配置文件 ef.masterslave.config 中的内容，来达到自动刷新相关配置连接的效果；


三、使用说明：
    1、首选通过数据库管理系统（DBMS）来配置多个数据库服务器实例之间的主从自动同步机制，例如：
        a、如果是用 MSSQLSERVER 数据库系统，可以配置多台数据库服务器实例之间的复制、订阅策略；
        b、如果是用 MySQL 数据库系统，可以配置多台数据库服务器实例之间的主从复制策略；
        c、其他 Oracle、DB2...

    2、在项目根目录下添加配置文件 ef.masterslave.config，并按规则修改其中的内容，以下是一份参考的配置方式：
        <?xml version="1.0" encoding="utf-8" ?>
        <configuration>
          <configSections>
            <section name="ef.masterslave" type="NDF.Data.EntityFramework.MasterSlaves.ConfigFile.EFMasterSlaveSection, NDF.Data.EntityFramework"
                     requirePermission="false" />
          </configSections>
          <ef.masterslave>
            <!-- 以下是为所有用 EF 实体上下文（DbContext）类型为 MyProject.Data.MyDbContext 的数据库操作配置主从读写分离服务  -->
            <applyItem targetContext="MyProject.Data.MyDbContext, MyProject.Data"
                       autoSwitchSlaveOnMasterFauled="false" autoSwitchMasterOnSlavesFauled="true" 
                       slaveRandomization="true" slaveScanInterval="60" >
              <master connectionString="server=192.168.0.99;port=3306;user id=root;password=123456;persistsecurityinfo=True;database=testdb;convertzerodatetime=True;allowzerodatetime=True" />
              <slaves>
                <add connectionString="server=192.168.0.101;port=3306;user id=root;password=123456;persistsecurityinfo=True;database=testdb;convertzerodatetime=True;allowzerodatetime=True" order="0" />
                <add connectionString="server=192.168.0.102;port=3306;user id=root;password=123456;persistsecurityinfo=True;database=testdb;convertzerodatetime=True;allowzerodatetime=True" order="1" />
                <add connectionString="server=192.168.0.103;port=3306;user id=root;password=123456;persistsecurityinfo=True;database=testdb;convertzerodatetime=True;allowzerodatetime=True" order="2" />
              </slaves>
            </applyItem>
            <!-- 以下是为另一个 EF 实体上下文（DbContext）配置主从读写分离服务  -->
            <!--<applyItem ...>
              <master ... />
              <slaves>
                <add ... />
                <add ... />
              </slaves>
            </applyItem>-->
          </ef.masterslave>
        </configuration>

    3、在项目中分别引入如下 dll 库文件（或 Nuget 包）：
        a、EntityFramework 6.1 以上版本；
        b、Microsoft Enterprise Library - Data Access Application Block 6；
        c、Newtonsoft.Json.dll 6.0 以上版本；
        d、NDF.Utilities.dll；
        e、NDF.Data.dll；
        f、NDF.Data.EntityFramework.dll；

    4、在项目的启动代码中（控制台和桌面程序一般为 Program 类型的 Main 方法、ASP.NET 程序一般为 Global.asax 文件的 Application_Start 代码块）加入如下代码段：
        NDF.Data.EntityFramework.MasterSlaves.EFMasterSlaveConfig.Register(typeof(MyDbContext));

        其中方法中传入的类型参数应该是 ef.masterslave.config 配置文件中 applyItem 节的 targetContext 属性所示的类型，表示要为具体
        哪个类型的 EF 实体上下文（DbContext） 配置读写分离服务。


四、注意事项：
    1、关于主从数据库中相关数据内容的自动同步机制，由数据库管理系统（DBMS，如 MSSQLSERVER、Oracle、MySQL、DB2 等）来完成，该部分的功能不由本插件来提供；
        目前几乎所有的主流 DBMS 系统都提供了主从数据库自动同步机制相关功能；

    2、该 EF 数据库主从读写分离方案支持所有普通数据库事务和分布式事务操作，不过分布式事务也同样需要数据库管理系统（DBMS）的支持否则无效；

    3、在基于 EF6 和该插件的配合进行数据库主从读写分离操作，程序会自动检测所执行的数据库操作的事务状态，并自动将带有数据库事务或分布式事务的
        所有 增删改请求 和 查询请求 都转发至 Master 服务器。






