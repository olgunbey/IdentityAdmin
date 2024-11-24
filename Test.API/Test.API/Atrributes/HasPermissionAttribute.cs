using Microsoft.AspNetCore.Mvc;

namespace Test.API.Atrributes
{
    public class HasPermissionAttribute : TypeFilterAttribute
    {
        public HasPermissionAttribute(string permission) : base(typeof(HasPermissionFilter))
        {
            Arguments=new object[] {permission};
        }
    }
}
