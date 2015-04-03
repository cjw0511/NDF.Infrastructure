using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.SessionState;

namespace NDF.Web.SessionState
{
    /// <summary>
    /// 自定义的数据存储区会话状态提供程序所需的成员。
    /// </summary>
    public abstract class SessionStateProviderBase : System.Web.SessionState.SessionStateStoreProviderBase
    {
        SessionStateSection sessionConfig;
        //string connectionString;
        //string providerName;
        //Type providerType;


        public string ApplicationName
        {
            get;
            private set;
        }

        public SessionStateSection Config
        {
            get { return this.sessionConfig; }
        }


        public override void Initialize(string name, NameValueCollection config)
        {
            //base.Initialize(name, config);

            Check.NotNull(config, "config");

            if (String.IsNullOrEmpty(name))
                name = "NdfSessionStateStore";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "NetDirkFramework Session Store Provider");
            }
            ApplicationName = HostingEnvironment.ApplicationVirtualPath;
            base.Initialize(name, config);

            sessionConfig = WebConfigurationManager.GetWebApplicationSection("system.web/sessionState") as SessionStateSection;
            

        }


        


        public override SessionStateStoreData CreateNewStoreData(HttpContext context, int timeout)
        {
            return new SessionStateStoreData(
                new SessionStateItemCollection(),
                SessionStateUtility.GetSessionStaticObjects(context),
                timeout);
        }

        public override void CreateUninitializedItem(HttpContext context, string id, int timeout)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        public override void EndRequest(HttpContext context)
        {
            throw new NotImplementedException();
        }

        public override SessionStateStoreData GetItem(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actions)
        {
            throw new NotImplementedException();
        }

        public override SessionStateStoreData GetItemExclusive(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actions)
        {
            throw new NotImplementedException();
        }

        public override void InitializeRequest(HttpContext context)
        {
            throw new NotImplementedException();
        }

        public override void ReleaseItemExclusive(HttpContext context, string id, object lockId)
        {
            throw new NotImplementedException();
        }

        public override void RemoveItem(HttpContext context, string id, object lockId, SessionStateStoreData item)
        {
            throw new NotImplementedException();
        }

        public override void ResetItemTimeout(HttpContext context, string id)
        {
            throw new NotImplementedException();
        }

        public override void SetAndReleaseItemExclusive(HttpContext context, string id, SessionStateStoreData item, object lockId, bool newItem)
        {
            throw new NotImplementedException();
        }

        public override bool SetItemExpireCallback(SessionStateItemExpireCallback expireCallback)
        {
            throw new NotImplementedException();
        }
    }
}
