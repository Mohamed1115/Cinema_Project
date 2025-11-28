using System.ComponentModel;

namespace MVC_PRJ_F.ModelView;

public class LoginVM
{
    public string UserName { get; set; }
    [type:PasswordPropertyText]
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}