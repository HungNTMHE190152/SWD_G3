namespace EduNexus.Configuration;

public class MailSettings
{
    public string SenderName { get; set; } = "";

    public string SenderEmail { get; set; } = "";

    public string Password { get; set; } = "";

    public string Host { get; set; } = "";

    public int Port { get; set; }
}