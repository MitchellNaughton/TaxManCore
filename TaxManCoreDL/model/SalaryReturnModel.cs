
namespace TaxManCoreDL.model;

public class SalaryReturnModel : ISalaryReturnModel
{
    public decimal dcGrossSal { get; set; }
    public decimal dcNetSal { get; set; }
    public decimal dcGrossTaxPd { get; set; }
}