namespace QrCommerce.Shared.DTOs;

public class DeviceRegisterDto
{
    public string DeviceCode { get; set; } = string.Empty;
    public string DeviceName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Carrier { get; set; } = string.Empty;
}
