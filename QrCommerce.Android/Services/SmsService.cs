namespace QrCommerce.Android.Services;

public class SmsService
{
    public async Task<bool> SendSmsAsync(string phoneNumber, string message)
    {
        try
        {
#if ANDROID
            var smsManager = global::Android.Telephony.SmsManager.Default;
            if (smsManager == null) return false;

            var parts = smsManager.DivideMessage(message);
            if (parts.Count > 1)
            {
                smsManager.SendMultipartTextMessage(phoneNumber, null, parts, null, null);
            }
            else
            {
                smsManager.SendTextMessage(phoneNumber, null, message, null, null);
            }
            return true;
#else
            await Task.CompletedTask;
            return false;
#endif
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"SMS Error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RequestPermissionsAsync()
    {
#if ANDROID
        var status = await Permissions.RequestAsync<Permissions.Sms>();
        return status == PermissionStatus.Granted;
#else
        await Task.CompletedTask;
        return false;
#endif
    }
}
