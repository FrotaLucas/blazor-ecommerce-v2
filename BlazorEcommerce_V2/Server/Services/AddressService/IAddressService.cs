namespace BlazorEcommerce_V2.Server.Services.AddressService
{
    public interface IAddressService 
    {
        Task<ServiceResponse<Address>> GetAddress();

        Task<ServiceResponse<Address>> AddOrUpdateAddress(Address address);
    }

}
