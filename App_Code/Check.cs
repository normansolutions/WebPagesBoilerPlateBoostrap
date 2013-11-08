using System.Linq;
using System.Net;
using System.Web;

public static class Check
{
    public static void User(params string[] actions) {
        if (actions.Any(a => Permissions.User(a))) {
            return;
        }
        HttpContext.Current.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
    }
}