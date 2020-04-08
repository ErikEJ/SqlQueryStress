using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SQLQueryStress
{
    [Serializable]
    [DataContract]
    public class ConnectionInfo : ICloneable
    {
        [DataMember]
        public string Database;
        [DataMember]
        public bool IntegratedAuth;
        [DataMember]
        public string Login;
        [DataMember]
        public string Password;

        [DataMember]
        public string Server;
        [DataMember]
        public ApplicationIntent ApplicationIntent;

        [DataMember]
        public readonly int ConnectTimeout;
        [DataMember]
        public readonly bool EnablePooling;
        [DataMember]
        public readonly int MaxPoolSize;


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
                SqlConnectionStringBuilder build = new SqlConnectionStringBuilder { DataSource = Server, IntegratedSecurity = IntegratedAuth, ApplicationName = "SQLQueryStress", ApplicationIntent = ApplicationIntent };
                if (!IntegratedAuth)
                {
                    build.UserID = Login;
                    build.Password = Password;
                }

                if (Database.Length > 0)
                    build.InitialCatalog = Database;

                if (ConnectTimeout != 0)
                {
                    build.ConnectTimeout = ConnectTimeout;
                }

                if (MaxPoolSize != 0)
                {
                    build.MaxPoolSize = MaxPoolSize;
                    build.MinPoolSize = MaxPoolSize;
                }

                build.Pooling = EnablePooling;

                return build.ConnectionString;
            }
        }

        #region ICloneable Members

        public object Clone()
        {
            ConnectionInfo newConnInfo = new ConnectionInfo();
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
            if (string.IsNullOrEmpty(Server) || ((IntegratedAuth == false) && (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password))))
            {
                return false;
            }

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
