namespace Tractus.Ndi;

public enum frame_format_type_e
{
    // A progressive frame
    frame_format_type_progressive = 1,

    // A fielded frame with the field 0 being on the even lines and field 1 being
    // on the odd lines/
    frame_format_type_interleaved = 0,

    // Individual fields
    frame_format_type_field_0 = 2,
    frame_format_type_field_1 = 3,

    frame_format_type_max = 0x7fffffff,
}
