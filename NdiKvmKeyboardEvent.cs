namespace Tractus.Ndi;

public class NdiKvmKeyboardEvent
{
    public NdiKvmKeyboardEvent()
    {
    }

    public byte Keycode { get; set; }
    public bool ShiftKey { get; set; }
    public bool CtrlKey { get; set; }

}