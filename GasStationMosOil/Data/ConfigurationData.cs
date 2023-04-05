using System.Collections.Generic;
using System.Xml.Serialization;

namespace GasolineMosOil.Data;

[XmlRoot("Configuration")]
public class ConfigurationData
{
    [XmlElement("NozzlePostCount")] public int NozzlePostCount { get; set; }

    [XmlArray("PaymentTypes")]
    [XmlArrayItem("PaymentType")]
    public List<PaymentTypeData>? PaymentTypes { get; set; }
}

public class PaymentTypeData
{
    [XmlElement("Name")] public string? Name { get; set; }

    [XmlElement("IsActive")] public bool IsActive { get; set; }

    [XmlElement("IsCash")] public bool IsCash { get; set; }
}