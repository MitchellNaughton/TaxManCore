using TaxManCoreDL.model;
using TaxManCoreDL.da;

namespace TaxManCoreDL.bo
{
    public class TaxManCoreBo
    {
        private static ISalaryReturnModel? _salaryReturn;
        private static ITaxValuesModel? _taxValues;

        public static void BoDependencies(ISalaryReturnModel oSalaryReturn, ITaxValuesModel oTaxValues)
        {
            _salaryReturn = oSalaryReturn;
            _taxValues = oTaxValues;
        }

        public static string SelectSingleQuery(string sFields, string sTable, bool bCriteria, string sClause)
        {
            string sQuery = $@"select {sFields} from {sTable}";

            if (bCriteria)
            {
                sQuery += $@" where {sClause}";
            }

            return sQuery;
        }

        public static string InsertSingleQuery(string sFields, string sTable, string sData)
        {
            string sQuery = $@"insert into {sTable} ({sFields})
                             values ({sData});";

            return sQuery;
        }

        public static ISalaryReturnModel GetSalaryReturn(decimal dcSalary, string sCountry, string sConn)
        {

            string sQuery = SelectSingleQuery("iUid", "TMDB.dbo.tmCountry", true, $@"TMDB.dbo.tmCountry.cCountry = '{sCountry}'");
            int iCountryId = DataAccess.GetCountryId(sQuery, sConn);
            sQuery = SelectSingleQuery("flTaxInc1, flTaxInc2, flTaxInc3, flTaxRate1, flTaxRate2, flTaxRate3", "TMDB.dbo.tmTax", true, $@"TMDB.dbo.tmTax.iCountryid = {iCountryId}");

            _taxValues = DataAccess.GetTaxObject(sQuery, sConn);
            _salaryReturn.dcGrossSal = dcSalary;

            var oReturn = CalculateSalaryReturn();

            return _salaryReturn;
        }

        public static ISalaryReturnModel CalculateSalaryReturn()
        {
            decimal dcTaxable1 = 0;
            decimal dcTaxable2 = 0;
            decimal dcTaxable3 = 0;

            if (_taxValues.dcTaxInc1 >= _salaryReturn.dcGrossSal)
            {

                dcTaxable1 = _salaryReturn.dcGrossSal;

            }
            else if (_taxValues.dcTaxInc1 < _salaryReturn.dcGrossSal && _taxValues.dcTaxInc2 >= _salaryReturn.dcGrossSal)
            {
                dcTaxable1 = _taxValues.dcTaxInc1;
                dcTaxable2 = _salaryReturn.dcGrossSal - _taxValues.dcTaxInc1;
            }
            else if (_taxValues.dcTaxInc2 < _salaryReturn.dcGrossSal)
            {
                dcTaxable1 = _taxValues.dcTaxInc1;
                dcTaxable2 = _taxValues.dcTaxInc2 - _taxValues.dcTaxInc1;
                dcTaxable3 = _salaryReturn.dcGrossSal - _taxValues.dcTaxInc2;
            }

            _salaryReturn.dcGrossTaxPd = (dcTaxable1 * _taxValues.dcTaxRate1)
                                       + (dcTaxable2 * _taxValues.dcTaxRate2)
                                       + (dcTaxable3 * _taxValues.dcTaxRate3);
            _salaryReturn.dcNetSal = _salaryReturn.dcGrossSal - _salaryReturn.dcGrossTaxPd;

            return _salaryReturn;
        }

        public static bool CreateTaxSetUp(TaxValuesModel ipoTaxVal, string ipsCountry, string sConn)
        {
            bool lReturn = false;
            string sQuery = SelectSingleQuery("iUid", "TMDB.dbo.tmCountry", true, $@"TMDB.dbo.tmCountry.cCountry = '{ipsCountry}'");
            int iCountryId = DataAccess.GetCountryId(sQuery, sConn);

            if (iCountryId == 0)
            {
                sQuery = InsertSingleQuery("cCountry", "TMDB.dbo.tmCountry", $"'{ipsCountry}'");
                DataAccess.InsertSingleRecord(sQuery, sConn);
                sQuery = SelectSingleQuery("iUid", "TMDB.dbo.tmCountry", true, $@"TMDB.dbo.tmCountry.cCountry = '{ipsCountry}'");
                iCountryId = DataAccess.GetCountryId(sQuery, sConn);
            }

            ipoTaxVal.iCountryId = iCountryId;
            /* I thought about transctioning this and the country addition
             * but countries only get added if they don't exist, plus these
             * are only single inserts so if they error then they aren't added 
             * there will likely be some front-end validation as well as server
             * side validation... */
            sQuery = InsertSingleQuery("iCountryId, flTaxInc1, flTaxInc2, flTaxInc3, flTaxRate1, flTaxRate2, flTaxRate3",
                                       "TMDB.dbo.tmTax",
                                       $"{ipoTaxVal.iCountryId}, {(float)ipoTaxVal.dcTaxInc1}, {(float)ipoTaxVal.dcTaxInc2}, {(float)ipoTaxVal.dcTaxInc3}, {(float)ipoTaxVal.dcTaxRate1}, {(float)ipoTaxVal.dcTaxRate2}, {(float)ipoTaxVal.dcTaxRate3}");
            DataAccess.InsertSingleRecord(sQuery, sConn);

            return lReturn;
        }
    }
}
