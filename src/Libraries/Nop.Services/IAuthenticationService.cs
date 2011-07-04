using Nop.Core.Domain.Customers;

namespace Nop.Services
{
    /// <summary>
    /// Authentication service interface
    /// </summary>
    public partial interface IAuthenticationService 
    {
        void SignIn(Customer customer, bool createPersistentCookie);
        void SignOut();
        Customer GetAuthenticatedCustomer();
    }
}