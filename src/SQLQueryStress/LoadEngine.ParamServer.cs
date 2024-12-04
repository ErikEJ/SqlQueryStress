using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using Microsoft.Data.SqlClient;

namespace SQLQueryStress;

public partial class LoadEngine
{
    private static class ParamServer
    {
        private static int _currentRow;
        private static int _numRows;

        //The actual params that will be filled
        private static SqlParameter[] _outputParams;
        //Map the param columns to ordinals in the data table
        private static int[] _paramDtMappings;
        private static DataTable _theParams;

        public static void GetNextRow_Values(SqlParameterCollection newParam)
        {
            var rowNum = Interlocked.Increment(ref _currentRow);
            var dr = _theParams.Rows[rowNum % _numRows];

            for (var i = 0; i < _outputParams.Length; i++)
            {
                newParam[i].Value = dr[_paramDtMappings[i]];
            }
        }

        public static SqlParameter[] GetParams()
        {
            var newParam = new SqlParameter[_outputParams.Length];

            for (var i = 0; i < _outputParams.Length; i++)
            {
                newParam[i] = (SqlParameter)((ICloneable)_outputParams[i]).Clone();
            }

            return newParam;
        }

        public static void Initialize(string paramQuery, string connString, Dictionary<string, string> paramMappings)
        {
#pragma warning disable CA2100
            using var sqlDataAdapter = new SqlDataAdapter(paramQuery, connString);
#pragma warning restore CA2100
            _theParams = new DataTable();
            sqlDataAdapter.Fill(_theParams);

            _numRows = _theParams.Rows.Count;

            _outputParams = new SqlParameter[paramMappings.Keys.Count];
            _paramDtMappings = new int[paramMappings.Keys.Count];

            //Populate the array of parameters that will be cloned and filled
            //on each request
            var i = 0;
            foreach (var parameterName in paramMappings.Keys)
            {
                _outputParams[i] = new SqlParameter { ParameterName = parameterName };
                var paramColumn = paramMappings[parameterName];

                //if there is a param mapped to this column
                if (paramColumn != null)
                    _paramDtMappings[i] = _theParams.Columns[paramColumn].Ordinal;

                i++;
            }
        }
    }
}