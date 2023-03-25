using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Requests
{
    
}

public class Register
{
    //public string Email { get; set; }
    //public string Password { get; set; }
    //public string password_confirmation { get; set; }
    
    //public string username;
    public string Email;
    public string Password;
    public string password_confirmation;
}

public class LogIn
{
    public string access_token;
    public string token_type;
    public int expires_in;
    public string userName;

    public string issued;

    public string expires;
}



