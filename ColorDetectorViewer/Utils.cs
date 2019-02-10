using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDetectorViewer
{
    public static class Utils
    {
        public static bool IsArduinoDetected(SerialPort currentPort)
        {
            try
            {
                if (currentPort.PortName == "COM1")
                    return false;
                currentPort.Open();
                System.Threading.Thread.Sleep(1000);
                // небольшая пауза, ведь SerialPort не терпит суеты

                string returnMessage = currentPort.ReadLine();
                currentPort.Close();

                // необходимо чтобы void loop() в скетче содержал код Serial.println("Info from Arduino");
                return returnMessage.Contains("$#$");
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
