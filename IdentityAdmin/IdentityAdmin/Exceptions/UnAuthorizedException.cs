namespace IdentityAdmin.Exceptions
{
    public class UnAuthorizedException:Exception
    {
        public UnAuthorizedException(string msj):base(msj){}
    }
}
