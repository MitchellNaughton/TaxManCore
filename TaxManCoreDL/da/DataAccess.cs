using System.Data;
using Dapper;
using System.Data.SqlClient;
using TaxManCoreDL.model;

namespace TaxManCoreDL.da
{
    public class DataAccess
    {
        private static ITaxValuesModel _taxValues;
        private static ISqlTaxModel _sqlTaxReturn;

        public static void DaDependencies(ITaxValuesModel oTaxValues, ISqlTaxModel oSqlTaxReturn)
        {
            _taxValues = oTaxValues;
            _sqlTaxReturn = oSqlTaxReturn;
        }
        public static bool InsertSingleRecord(string ipsQuery, string ipsConn)
        {
            using (IDbConnection cnn = new SqlConnection(ipsConn))
            {
                int iRowNo = cnn.Execute(ipsQuery);
                bool lReturn = iRowNo > 0;
                return lReturn;
            }
        }

        public static ITaxValuesModel GetTaxObject(string query, string ipsConn)
        {
            using (IDbConnection cnn = new SqlConnection(ipsConn))
            {
                _sqlTaxReturn = cnn.QuerySingle<SqlTaxModel>(query);

                _taxValues.dcTaxInc1 = (decimal)_sqlTaxReturn.flTaxInc1;
                _taxValues.dcTaxInc2 = (decimal)_sqlTaxReturn.flTaxInc2;
                _taxValues.dcTaxInc3 = (decimal)_sqlTaxReturn.flTaxInc3;
                _taxValues.dcTaxRate1 = (decimal)_sqlTaxReturn.flTaxRate1;
                _taxValues.dcTaxRate2 = (decimal)_sqlTaxReturn.flTaxRate2;
                _taxValues.dcTaxRate3 = (decimal)_sqlTaxReturn.flTaxRate3;

                return _taxValues;
            }
        }

        public static int GetCountryId(string query, string ipsConn)
        {
            using (IDbConnection cnn = new SqlConnection(ipsConn))
            {
                int iReturn;
                Nullable<int> iResponse = cnn.QuerySingleOrDefault<int>(query);

                if (iResponse.HasValue)
                {
                    iReturn = iResponse.Value;
                }
                else
                {
                    iReturn = 0;
                }

                return iReturn;
            }
        }
    }
}
