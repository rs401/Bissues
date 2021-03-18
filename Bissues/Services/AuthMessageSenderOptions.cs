
using System;

public class AuthMessageSenderOptions
{
    public string SendGridUser 
    { 
        get
        {
            return Environment.GetEnvironmentVariable("SEND_GRID_USER");
        }
    }
    public string SendGridEmail 
    { 
        get
        {
            return Environment.GetEnvironmentVariable("SEND_GRID_EMAIL");
        }
    }
    public string SendGridKey 
    { 
        get
        {
            return Environment.GetEnvironmentVariable("SEND_GRID_KEY");
        }
    }
}