using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using WebMatrix.Data;
using WebMatrix.WebData;

public static class Permissions{

    public static bool User(params string[] activities) {
        IEnumerable<string> permissions = WebCache.Get(WebSecurity.CurrentUserName);
        if (permissions == null) {
            var db = Database.Open((string)HttpContext.Current.Application["Database"]);
            var commandText = @"SELECT Permissions.Name FROM Permissions 
                                INNER JOIN RolesPermissions ON Permissions.PermissionId = RolesPermissions.PermissionId
                                INNER JOIN webpages_UsersInRoles ON webpages_UsersInRoles.RoleId = RolesPermissions.RoleId
                                INNER JOIN Users ON webpages_UsersInRoles.UserId = Users.UserId 
                                WHERE lower(Users.UserName) = @0";
            permissions = db.Query(commandText, WebSecurity.CurrentUserName.ToLower()).Select(item => item.Name).Cast<string>();
            WebCache.Set(WebSecurity.CurrentUserName, permissions);
        }
        return permissions.Intersect(activities).Any();
    }
}
