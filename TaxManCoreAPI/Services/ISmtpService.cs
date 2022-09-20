namespace TaxManCoreAPI.Services;

public interface ISmtpService
{
    public string? sEmailTo { get; set; }
    public string? sSubject { get; set; }
    public string? sBody { get; set; }

    public void SendEmail();

}
