using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace Functions.Util.Data
{
    public class DataContext : IDisposable
    {
        public SqlConnection Connection { get; set; }
        private readonly IConfiguration _configuration;
        public string Server
        {
            get
            {
                var config = "Server";
                return _configuration[config];
            }
            set { }
        }
        public bool AutWindows
        {
            get
            {
                var config = "AutWindows";
                return string.IsNullOrWhiteSpace(_configuration[config]) ? false : Convert.ToBoolean(_configuration[config]);
            }
            set { }
        }
        public string User
        {
            get
            {
                var config = "User";
                return Cryptography.Cryptography.Decrypts(_configuration[config]);
            }
            set { }
        }
        public string Password
        { get
            {
                var config = "PassWord";
                return Cryptography.Cryptography.Decrypts(_configuration[config]);
            }
            set { }
        }
        public string DataBase
        {
            get
            {
                var config = "DataBase";
                return _configuration[config];
            }
            set { }
        }
        public DataContext(IConfiguration configuration)
        {
            SqlMapper.AddTypeMap(typeof(string), DbType.AnsiString);
            _configuration = configuration;
            Connection = new SqlConnection($"Server={Server};DataBase={DataBase};User ID={User};Password={Password}");
        }
        public async Task<IEnumerable<T>> ExecuteQueryAsync<T>(string query, List<ParametersScript> param = null)
        {
            try
            {
                await Connection.OpenAsync();
                DynamicParameters parametersScript = new ();

                if (param != null && param.Any())
                {
                    foreach (var parameter in param)
                        parametersScript.Add(parameter.Name, parameter.Value);
                }
                var result = await Connection.QueryAsync<T>(query.ToString(), parametersScript);
                await DisposeAsync();
                return result;
            }
            catch
            {
                await DisposeAsync();
                throw;
            }
        }
        public async Task ExecuteScriptAsync(string query, List<ParametersScript> param = null)
        {
            try
            {
                await Connection.OpenAsync();
                using SqlTransaction tran = (SqlTransaction)await Connection.BeginTransactionAsync();
                try
                {
                    DynamicParameters parametersScript = new();

                    if (param != null && param.Any())
                    {
                        foreach (var parameter in param)
                            parametersScript.Add(parameter.Name, parameter.Value);
                    }

                    await Connection.ExecuteAsync(query.ToString(), parametersScript, tran);
                    await tran.CommitAsync();
                    await DisposeAsync();
                }
                catch
                {
                    await tran.RollbackAsync();
                    await DisposeAsync();
                    throw;
                }
            }
            catch
            {
                throw;
            }
        }
        public async Task<SqlTransaction> ExecuteBeginTransactionAsync()
        {
            await Connection.OpenAsync();
            return (SqlTransaction)await Connection.BeginTransactionAsync();
        }
        public async Task ExecuteRollbackAsync(SqlTransaction tran)
        {
            await tran.RollbackAsync();
            await DisposeAsync();
        }
        public async Task ExecuteCommitAsync(SqlTransaction tran)
        {
            await tran.CommitAsync();
            await DisposeAsync();
        }
        public void Dispose()
        {
            if (Connection.State != ConnectionState.Closed)
                Connection.Close();
        }
        public async Task DisposeAsync()
        {
            if (Connection.State != ConnectionState.Closed)
                await Connection.CloseAsync();
        }
    }
    public class ParametersScript
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }
}
