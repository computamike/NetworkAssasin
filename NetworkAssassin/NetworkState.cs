namespace NetworkAssassin
{
    public enum NetworkState
    {
        Disconnected = 0,
        Connecting = 1,
        Connected = 2,
        Disconnecting = 3,
        HardwareNotPresent = 4,
        HardwareDisabled = 5,
        HardwareMalfunction = 6,
        MediaDisconnected = 7,
        Authenticating = 8,
        AuthenticationSucceeded = 9,
        AuthenticationFailed = 10,
        InvalidAddress = 11,
        CredentialsRequired = 12
    };
}
