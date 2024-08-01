namespace BlazorEcommerce_V2.Server.Services.AddressService
{
    public interface IAddressService 
    {
        Task<ServiceResponse<Address>> GetAddress(Address address);

        Task<ServiceResponse<Address>> AddOrUpdateAddress(Address address);
    }

}
