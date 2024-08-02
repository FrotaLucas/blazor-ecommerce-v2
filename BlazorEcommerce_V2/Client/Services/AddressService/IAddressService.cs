namespace BlazorEcommerce_V2.Client.Services.AddressService
{
    public interface IAddressService
    {
        Task<Address> GetAddress();
        Task<Address> AddOrUpdateAddress(Address address);
    }
}
