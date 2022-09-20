namespace TaxManCoreAPI.Services;

public class InternalSmtp : ISmtpService
{
    private string? sSmtpService { get; set; }
    public string? sEmailTo { get; set; }
    private string? sEmailFrom { get; set; }
    public string? sSubject { get; set; }
    public string? sBody { get; set; }

    public InternalSmtp()
    {
        sSmtpService = "InternalSMTPCon123";
        sEmailFrom = "hostname@email.com";
    }

    public void SendEmail()
    {
        // connect to smtp server using connection string
        // ensure other params aren't blank and error if so

        // add confidentially flags if files added etc

        //if all good at this point call send method on smtp service
    }
}
