using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLQueryStress
{
    [Serializable]
    public class ConnectionInfo : ICloneable
    {
        public string Database;
        public bool IntegratedAuth;
        public string Login;
        public string Password;

        public string Server;
        public ApplicationIntent ApplicationIntent;

        public int ConnectTimeout;
        public bool EnablePooling;
        public int MaxPoolSize; 


        public ConnectionInfo()
        {
            Server = "";
            IntegratedAuth = true;
            ApplicationIntent = ApplicationIntent.ReadWrite;
            Login = "";
            Password = "";
            Database = "";
            ConnectTimeout = 0;
            MaxPoolSize = 0;
            EnablePooling = true; 
        }

        public ConnectionInfo(int connectTimeout, bool enablePooling, int maxPoolSize) : this()
        {
            ConnectTimeout = connectTimeout;
            EnablePooling = enablePooling;
            MaxPoolSize = maxPoolSize;
        }

        public string ConnectionString
        {
            get
            {
                var build = new SqlConnectionStringBuilder { DataSource = Server, IntegratedSecurity = IntegratedAuth, ApplicationName = "SQLQueryStress", ApplicationIntent = ApplicationIntent };
                if (!IntegratedAuth)
                {
                    build.UserID = Login;
                    build.Password = Password;
                }

                if (Database.Length > 0)
                    build.InitialCatalog = Database;

                if (ConnectTimeout != 0){
                    build.ConnectTimeout = ConnectTimeout;
                }

                if (MaxPoolSize != 0)
                {
                    build.MaxPoolSize = MaxPoolSize;
                }

                build.Pooling = EnablePooling;

                return build.ConnectionString;
            }
        }

        #region ICloneable Members

        public object Clone()
        {
            var newConnInfo = new ConnectionInfo();
            CopyTo(newConnInfo);

            return newConnInfo;
        }

        #endregion

        public void CopyTo(ConnectionInfo to)
        {
            to.Server = Server;
            to.IntegratedAuth = IntegratedAuth;
            to.Login = Login;
            to.Password = Password;
            to.Database = Database;
            to.ApplicationIntent = ApplicationIntent;
        }

        public bool TestConnection()
        {
            if ((Server == "") || ((IntegratedAuth == false) && (Login == "" || Password == "")))
                return false;

            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception exc)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
