using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Threading;
using System.Windows;

namespace GasStationMosOil.Model
{

    class BarCode
    {
        SerialPort serialPort = new SerialPort();
        public SerialPort CreateNewScanner()
        {
            serialPort.PortName = "COM3";
            serialPort.BaudRate = 9600;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
            serialPort.Parity = Parity.None;  
            return serialPort;
        }
    }
}
