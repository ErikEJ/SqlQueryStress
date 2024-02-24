using Microsoft.Data.SqlClient;
using System;
using System.Runtime.Serialization;

namespace SQLQueryStress
{
    [Serializable]
    [DataContract]
    public class ConnectionInfo : ICloneable
    {
        [DataMember]
        public string Database { get; set; }

        [DataMember]
        public bool IntegratedAuth { get; set; }

        [DataMember]
        public bool AzureMFA { get; set; }

        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string Server { get; set; }

        [DataMember]
        public  SqlConnectionEncryptOption EncryptOption { get; set; }

        [DataMember]
        public ApplicationIntent ApplicationIntent { get; set; }

        [DataMember]
        public int ConnectTimeout { get; set; }

        [DataMember]
        public bool EnablePooling { get; set; }

        [DataMember]
        public int MaxPoolSize { get; set; }

        [DataMember]
        public string AdditionalParameters { get; set; }

        public bool RequiresPassword
        { 
            get
            {
                return !IntegratedAuth && !AzureMFA;
            }
        }

        public ConnectionInfo()
        {
            Server = "(local)";
            IntegratedAuth = true;
            ApplicationIntent = ApplicationIntent.ReadWrite;
            Login = string.Empty;
            Password = string.Empty;
            Database = string.Empty;
            ConnectTimeout = 0;
            MaxPoolSize = 0;
            EnablePooling = true;
            EncryptOption = SqlConnectionEncryptOption.Optional;
            AdditionalParameters = String.Empty;
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
                if (!IntegratedAuth && !AzureMFA)
                {
                    build.UserID = Login;
                    build.Password = Password;
                }

                if (AzureMFA)
                {
                    build.UserID = Login;
                }

                if (AzureMFA)
                {
                    build.Authentication = SqlAuthenticationMethod.ActiveDirectoryInteractive;
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
                build.Encrypt = EncryptOption;

                string connString = build.ConnectionString;

                if (!string.IsNullOrEmpty(AdditionalParameters))
                {
                    connString = $"{connString};{AdditionalParameters}";
                }

                return connString;
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
            if (to is null)
            {
                throw new ArgumentNullException(nameof(to));
            }

            to.Server = Server;
            to.IntegratedAuth = IntegratedAuth;
            to.AzureMFA = AzureMFA;
            to.Login = Login;
            to.Password = Password;
            to.Database = Database;
            to.ApplicationIntent = ApplicationIntent;
            to.EncryptOption = EncryptOption;
            to.AdditionalParameters = AdditionalParameters;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types")]
        public bool TestConnection()
        {
            if ((string.IsNullOrEmpty(Server)) 
                || ((IntegratedAuth == false && AzureMFA == false) && (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password))))
            return false;

            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open(SqlConnectionOverrides.OpenWithoutRetry);
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
