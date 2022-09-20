namespace TaxManCoreAPI.Services;

public class ExternalSmtp : ISmtpService
{
    private string? sSmtpService { get; set; }
    public string? sEmailTo { get; set; }
    private string? sEmailFrom { get; set; }
    public string? sSubject { get; set; }
    public string? sBody { get; set; }

    public ExternalSmtp()
    {
        sSmtpService = "ExternalSmtpConn456";
        sEmailFrom = "noreply@email.com";
    }

    public void SendEmail()
    {
        // connect to smtp server using connection string
        // ensure other params aren't blank and error if so

        //if all good at this point call send method on smtp service
    }
}
