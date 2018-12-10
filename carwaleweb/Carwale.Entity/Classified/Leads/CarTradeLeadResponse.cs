using System;
using System.Collections.Generic;

[Serializable]
public class CarTradeLeadResponse
{
    public List<CarTradeLead> Result { get; set; }
    public int Status { get; set; }
    public string Details { get; set; }
}