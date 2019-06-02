using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.Net.Sockets;
using System.Xml.Serialization;
using System.Management;
using Microsoft.Win32;


namespace dcontUploader
{

    [XmlRoot("DbMeasurementRecord")]
    public class DbMeasurementRecord
    {
        private int measurment_IdField;
        private float valueField;
        private DateTime dateField;
        private bool isDcontField;
        private bool markerField;
        private bool isBloodPlasmaField;
        private int isLoHiField;
        private DateTime dcontDateField;
        private bool isVisibleField;
        private bool? isHypoField;
        private bool? isSportField;
        private bool? isPostmealField;
        private bool? isPremealField;
        private bool? isFastingField;

        [XmlElement("Measurment_Id")]
        public int Measurment_Id
        {
            get
            {
                return this.measurment_IdField;
            }
            set
            {
                this.measurment_IdField = value;
            }
        }

        [XmlElement("Value")]
        public float Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        [XmlElement("Date")]
        public DateTime Date
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
            }
        }

        [XmlElement("IsDcont")]
        public bool IsDcont
        {
            get
            {
                return this.isDcontField;
            }
            set
            {
                this.isDcontField = value;
            }
        }

        [XmlElement("Marker")]
        public bool Marker
        {
            get
            {
                return this.markerField;
            }
            set
            {
                this.markerField = value;
            }
        }

        [XmlElement("IsBloodPlasma")]
        public bool IsBloodPlasma
        {
            get
            {
                return this.isBloodPlasmaField;
            }
            set
            {
                this.isBloodPlasmaField = value;
            }
        }

        [XmlElement("IsLoHi")]
        public int IsLoHi
        {
            get
            {
                return this.isLoHiField;
            }
            set
            {
                this.isLoHiField = value;
            }
        }

        [XmlElement("DcontDate")]
        public DateTime DcontDate
        {
            get
            {
                return this.dcontDateField;
            }
            set
            {
                this.dcontDateField = value;
            }
        }

        [XmlElement("IsVisible")]
        public bool IsVisible
        {
            get
            {
                return this.isVisibleField;
            }
            set
            {
                this.isVisibleField = value;
            }
        }

        [XmlElement(IsNullable = true)]
        public bool? IsHypo
        {
            get
            {
                return this.isHypoField;
            }
            set
            {
                this.isHypoField = value;
            }
        }

       [XmlElement(IsNullable = true)]
        public bool? IsSport
        {
            get
            {
                return this.isSportField;
            }
            set
            {
                this.isSportField = value;
            }
        }

        [XmlElement(IsNullable = true)]
        public bool? IsPostmeal
        {
            get
            {
                return this.isPostmealField;
            }
            set
            {
                this.isPostmealField = value;
            }
        }

        [XmlElement(IsNullable = true)]
        public bool? IsPremeal
        {
            get
            {
                return this.isPremealField;
            }
            set
            {
                this.isPremealField = value;
            }
        }

       [XmlElement(IsNullable = true)]
        public bool? IsFasting
        {
            get
            {
                return this.isFastingField;
            }
            set
            {
                this.isFastingField = value;
            }
        }
    }



    class Program
    {

        public enum GlucoOperationResultEnum
        {
            Unknown,
            Success,
            Failed,
        }

        public class GlucoResponse
        {
            private GlucoOperationResultEnum operationResultField;
            private string errorField;
            private object returnValueField;

            public GlucoOperationResultEnum operationResult
            {
                get
                {
                    return this.operationResultField;
                }
                set
                {
                    this.operationResultField = value;
                }
            }

            public string error
            {
                get
                {
                    return this.errorField;
                }
                set
                {
                    this.errorField = value;
                }
            }

            public object returnValue
            {
                get
                {
                    return this.returnValueField;
                }
                set
                {
                    this.returnValueField = value;
                }
            }
        }

        public class DbGlucoTransferRecord
        {
            private int transfer_IDField;
            private uint statusField;
            private uint codeField;
            private uint dcont_IDField;
            private uint versionField;

            public int Transfer_ID
            {
                get
                {
                    return this.transfer_IDField;
                }
                set
                {
                    this.transfer_IDField = value;
                }
            }

            public uint Status
            {
                get
                {
                    return this.statusField;
                }
                set
                {
                    this.statusField = value;
                }
            }

            public uint Code
            {
                get
                {
                    return this.codeField;
                }
                set
                {
                    this.codeField = value;
                }
            }

            public uint Dcont_ID
            {
                get
                {
                    return this.dcont_IDField;
                }
                set
                {
                    this.dcont_IDField = value;
                }
            }

            public uint Version
            {
                get
                {
                    return this.versionField;
                }
                set
                {
                    this.versionField = value;
                }
            }
        }

        static uint getSoftVer(List<byte> data)
        {
            DbGlucoTransferRecord glucoTransferRecord = new DbGlucoTransferRecord();
            uint SoftwareVersion = 0;
            try
            {
                if (data == null || data.Count == 0)
                {
                    //  return glucoResponse;
                    Console.WriteLine("data is null!");
                    return 0;
                }
                int count = data.Count;
                uint[] numArray = new uint[data.Count];
                for (int index = 0; index < data.Count; ++index)
                    numArray[index] = (uint)data[index];
                glucoTransferRecord.Status = (uint)((int)numArray[0] | (int)numArray[1] << 8 | (int)numArray[2] << 16 | (int)numArray[3] << 24);
                glucoTransferRecord.Code = numArray[8] | numArray[9] << 8;
                glucoTransferRecord.Version = (uint)((int)numArray[count - 6] | (int)numArray[count - 7] << 8 | (int)numArray[count - 8] << 16 | (int)numArray[count - 9] << 24);
                glucoTransferRecord.Dcont_ID = (uint)((int)numArray[count - 5] | (int)numArray[count - 4] << 8 | (int)numArray[count - 3] << 16 | (int)numArray[count - 2] << 24);
                SoftwareVersion = numArray[count - 9];
                //     glucoTransferRecord.returnValue = (object)data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception4: " + ex);
            }
            return SoftwareVersion;
        }

        static bool ValidateCheckSum(byte[] data)
        {
            int num = 0;
            for (int index = 0; index < data.Length; ++index)
                num += (int)data[index];
            if ((byte)num != (byte)127) return (byte)num == byte.MaxValue;
            return true;
        }

        static byte CalculateCheckSum(byte[] data)
        {
            int num = 0;
            for (int index = 0; index < data.Length - 1; ++index)
                num -= (int)data[index];
            return (byte)num;
        }

        static void SendCurrentDateTime(SerialPort ComPort)
        {

            while (!ComPort.IsOpen)
            {
                Console.WriteLine("comport is not open");
                return;
            }

            DateTime now = DateTime.Now;

            byte[] data = new byte[9] { (byte) 170,
                (byte) 85,
                (byte) now.Second,
                (byte) now.Minute,
                (byte) now.Hour,
                (byte) now.Day,
                (byte) now.Month,
                (byte)(now.Year - 2000),
                (byte) 0
            };
            data[8] = CalculateCheckSum(data);
            ComPort.Write(data, 0, data.Length);
        }

        private static List<string> GetUsbPorts()
        {
            List<string> stringList = new List<string>();
            RegistryKey localMachine = Registry.LocalMachine;
            RegistryKey registryKey1;
            try
            {
                registryKey1 = localMachine.OpenSubKey("SYSTEM\\CURRENTCONTROLSET\\ENUM\\USB");
                if (registryKey1 == null)
                    throw new Exception();
            }
            catch (Exception ex1)
            {
                try
                {
                    registryKey1 = localMachine.OpenSubKey("ENUM\\USB");
                    if (registryKey1 == null)
                        throw new Exception();
                }
                catch (Exception ex2)
                {
                    localMachine.Close();
                    return stringList;
                }
            }
            try
            {
                foreach (string subKeyName1 in registryKey1.GetSubKeyNames())
                {
                    try
                    {
                        RegistryKey registryKey2 = registryKey1.OpenSubKey(subKeyName1);
                        foreach (string subKeyName2 in registryKey2.GetSubKeyNames())
                        {
                            RegistryKey registryKey3 = registryKey2.OpenSubKey(subKeyName2);
                            foreach (string valueName in registryKey3.GetValueNames())
                            {
                                if (valueName == "FriendlyName" && registryKey3.GetValue(valueName).ToString().Contains("Dcont") && registryKey3.GetValue(valueName).ToString().Contains("(COM"))
                                {
                                    string str = "";
                                    try
                                    {
                                        str = registryKey3.OpenSubKey("Device Parameters").GetValue("PortName").ToString();
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    if (str != "")
                                        stringList.Add(str);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
            registryKey1.Close();
            localMachine.Close();
            return stringList;
        }

        static GlucoResponse dumpFromComPort(SerialPort ComPort, string selectedPortName)
        {
            GlucoResponse glucoResponse = new GlucoResponse();
            glucoResponse.operationResult = GlucoOperationResultEnum.Failed;
            glucoResponse.error = (string)null;
            bool abortRead = false;
            List<byte> lastRead = new List<byte>();
            byte num1 = 0;
            byte num2 = 0;
            bool needInicializePort = true;
            while (num2.ToString("X") != "AA" && num1.ToString("X") != "55" && !abortRead)
            {
                num2 = num1;
                try
                {
                    if (needInicializePort)
                    {
                        if (ComPort != null && ComPort.IsOpen) ComPort.Close();
                        ComPort = new SerialPort(selectedPortName);
                        ComPort.BaudRate = 2400;
                        ComPort.DataBits = 8;
                        ComPort.Parity = Parity.Odd;
                        ComPort.StopBits = StopBits.One;
                        ComPort.ReadTimeout = 125;
                        ComPort.ReadBufferSize = 4096;
                        ComPort.ParityReplace = (byte)0;
                        try
                        {
                            //  this.stopwatcher.Start();
                            ComPort.Open();
                            needInicializePort = false;
                            ComPort.DiscardInBuffer();

                            SendCurrentDateTime(ComPort);

                        }
                        catch (Exception e)
                        {
                            Console.Write(".");
                        }
                    }
                    if (ComPort.IsOpen) num1 = (byte)ComPort.ReadByte();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception2:" + e);
                }
            }
            if (!abortRead)
            {
                //       this.OnPercentChange(0);
                //     this.OnAddStatusText("Az adatok számítógépre történő feltöltése folyamatban van, amely néhány percet is igénybe vehet.");
                Console.WriteLine("abortRead");
                ComPort.ReadTimeout = 5000;
            }
            int num3 = 21;
            int num4 = num3;
            int num5 = 2;
            while (!abortRead && num5 < num4)
            {
                if ((num5 & 15) == 0)
                {
                    int percentage = Math.Min(100, num5 * 100 / num4);
                    Console.WriteLine("%: " + percentage);
                }
                //  this.OnPercentChange();
                if (num5 == 8) num4 = 5 * (int)(short)((int)lastRead[4] + ((int)lastRead[5] << 8)) + num3;
                try
                {
                    lastRead.Add((byte)ComPort.ReadByte()); ++num5;
                }
                catch (TimeoutException ex)
                {
                    Console.WriteLine("TimeoutException:" + ex);
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception3:" + ex);
                    break;
                }
            }
            if (ComPort.IsOpen)
            {
                ComPort.Close();
                ComPort.Dispose();
            }
            List<byte> byteList = new List<byte>();
            byteList.Add((byte)170);
            byteList.Add((byte)85);
            byteList.AddRange((IEnumerable<byte>)lastRead);
            if (abortRead)
            {
                Console.WriteLine("Felhasználó által megszakítva.");
                return glucoResponse;
            }
            if (!ValidateCheckSum(byteList.ToArray()))
            {
                /*
                glucoResponse.operationResult = GlucoOperationResultEnum.Failed;
                glucoResponse.error = this.CHECKSUM_ERROR;
                this.OnAddStatusText(this.CHECKSUM_ERROR);
                glucoResponse.returnValue = (object)this.lastRead;
                */
                Console.WriteLine("checksum error");
            }
            else if (glucoResponse.error == null)
            {
                glucoResponse.returnValue = (object)lastRead;
                glucoResponse.operationResult = GlucoOperationResultEnum.Success;
            }

            Console.WriteLine("OK!");
            return glucoResponse;

        }

       
        static private GlucoResponse analyze(
        int fieldSize, int SoftwareVersion, float low, float high, List<byte> data)
        {
            GlucoResponse glucoResponse1 = new GlucoResponse();
            glucoResponse1.operationResult = GlucoOperationResultEnum.Failed;
            try
            {
                int num = (data.Count - 19) / fieldSize;
                if (num == 0)
                {
                    glucoResponse1.operationResult = GlucoOperationResultEnum.Success;
                    glucoResponse1.returnValue = (object)null;
                    return glucoResponse1;
                }
                List<DbMeasurementRecord> measurementRecordList = new List<DbMeasurementRecord>();

                int year = SERVER_TIME.Year;
                int month1 = SERVER_TIME.Month;

                for (int index1 = 0; index1 < num; ++index1)
                {
                    DbMeasurementRecord measurementRecord = new DbMeasurementRecord();
                    int index2 = index1 * fieldSize + 10;
                    measurementRecord.Value = (float)(((double)data[index2] + (double)(((int)data[index2 + 1] & 1) << 8)) / 10.0);
                    if ((double)measurementRecord.Value <= (double)low && (double)measurementRecord.Value != 0.0)
                    {
                        measurementRecord.Value = low;
                        measurementRecord.IsLoHi = 1;
                    }
                    if ((double)measurementRecord.Value >= (double)high)
                    {
                        measurementRecord.Value = high;
                        measurementRecord.IsLoHi = 2;
                    }
                    int month2 = ((int)data[index2 + 1] >> 4) + 1;
                    int day = ((int)data[index2 + 3] >> 3) + 1;
                    int hour = (int)data[index2 + 2] >> 6 & 3 | ((int)data[index2 + 3] & 7) << 2;
                    int minute = (int)data[index2 + 2] & 63;
                    measurementRecord.Marker = ((int)data[index2 + 1] & 2) == 2;
                    measurementRecord.IsBloodPlasma = ((int)data[index2 + 1] & 4) == 4;
                    if (SoftwareVersion >= 96U)
                    {
                        if (SoftwareVersion >= 100U)
                        {
                            measurementRecord.IsHypo = new bool?(((int)data[index2 + 1] & 12) >> 2 == 2);
                            switch (((int)data[index2 + 4] & 224) >> 5)
                            {
                                case 1:
                                    measurementRecord.IsPremeal = new bool?(true);
                                    break;
                                case 2:
                                    measurementRecord.IsPostmeal = new bool?(true);
                                    break;
                                case 3:
                                    measurementRecord.IsSport = new bool?(true);
                                    break;
                                case 4:
                                    measurementRecord.IsFasting = new bool?(true);
                                    break;
                                case 5:
                                    measurementRecord.Marker = true;
                                    break;
                            }
                        }
                        else
                        {
                            measurementRecord.IsHypo = new bool?(((int)data[index2 + 1] & 8) == 8);
                            measurementRecord.IsSport = new bool?((int)data[index2 + 4] >> 7 == 1);
                            measurementRecord.IsPostmeal = new bool?(((int)data[index2 + 4] >> 6 & 1) == 1);
                            measurementRecord.IsPremeal = new bool?(((int)data[index2 + 4] >> 5 & 1) == 1);
                        }
                    }
                    if (fieldSize == 5) year = 2008 + ((int)data[index2 + 4] & 31);
                    try
                    {
                        measurementRecord.Date = new DateTime(year, month2, day, hour, minute, 0);
                        measurementRecord.DcontDate = fieldSize != 4 ? measurementRecord.Date : new DateTime(1980, month2, day, hour, minute, 0);
                    }
                    catch (Exception ex)
                    {
                        GlucoResponse glucoResponse2 = glucoResponse1;
                        glucoResponse2.error = glucoResponse2.error + ex.ToString() + "\n\n\n";
                        continue;
                    }
                    measurementRecord.IsVisible = true;
                    if ((double)measurementRecord.Value != 0.0) measurementRecordList.Add(measurementRecord);
                }
                measurementRecordList.Reverse();
                glucoResponse1.returnValue = (object)measurementRecordList;
            }
            catch (Exception ex)
            {
                glucoResponse1.error = ex.ToString();
                return glucoResponse1;
            }
            glucoResponse1.operationResult = GlucoOperationResultEnum.Success;
            return glucoResponse1;
        }

        static float[] getFieldSizes(string response)
        {
            float[] toReturn = new float[3];
            int fieldSize = 0;
            float low = 0;
            float high = 0;

            try
            {
                string[] strArray1 = response.Split('#');
                if (strArray1 == null || strArray1.Length != 3) return toReturn;
                fieldSize = int.Parse(strArray1[0]);
                string[] strArray2 = strArray1[1].Split(new char[2] {
                    ',',
                    '.'
                },
                StringSplitOptions.RemoveEmptyEntries);
                if (strArray2 != null && strArray2.Length > 0)
                {
                    low = (float)Convert.ToInt32(strArray2[0]);
                    if (strArray2.Length > 1) low += (float)Convert.ToInt32(strArray2[1]) / (float)Math.Pow(10.0, (double)strArray2[1].Length);
                }
                string[] strArray3 = strArray1[2].Split(new char[2] {
                    ',',
                    '.'
                },
                StringSplitOptions.RemoveEmptyEntries);
                if (strArray3 == null || strArray3.Length <= 0) return toReturn;
                high = (float)Convert.ToInt32(strArray3[0]);
                if (strArray3.Length <= 1) return toReturn;
                high += (float)Convert.ToInt32(strArray3[1]) / (float)Math.Pow(10.0, (double)strArray3[1].Length);
            }
            catch
            {
                fieldSize = -1;
            }

            toReturn[0] = low;
            toReturn[1] = high;
            toReturn[2] = fieldSize;
            return toReturn;
        }

        private static DbGlucoTransferRecord getTransferInfo(List<byte> data)
        {
            DbGlucoTransferRecord transfer = new DbGlucoTransferRecord();
           // glucoResponse.operationResult = GlucoOperationResultEnum.Failed;
            try
            {
                if (data == null || data.Count == 0)
                    return transfer;
                int count = data.Count;
                uint[] numArray = new uint[data.Count];
                for (int index = 0; index < data.Count; ++index)
                    numArray[index] = (uint)data[index];
                DbGlucoTransferRecord glucoTransferRecord = new DbGlucoTransferRecord();
                glucoTransferRecord.Status = (uint)((int)numArray[0] | (int)numArray[1] << 8 | (int)numArray[2] << 16 | (int)numArray[3] << 24);
                glucoTransferRecord.Code = numArray[8] | numArray[9] << 8;
                glucoTransferRecord.Version = (uint)((int)numArray[count - 6] | (int)numArray[count - 7] << 8 | (int)numArray[count - 8] << 16 | (int)numArray[count - 9] << 24);
                glucoTransferRecord.Dcont_ID = (uint)((int)numArray[count - 5] | (int)numArray[count - 4] << 8 | (int)numArray[count - 3] << 16 | (int)numArray[count - 2] << 24);
            //    SOFTWARE_VERSION = (int)numArray[count - 9]; what the fuck
                return glucoTransferRecord;
            }
            catch (Exception ex)
            {
               // glucoResponse.error = ex.ToString();
                return transfer;
            }
           // glucoResponse.operationResult = GlucoOperationResultEnum.Success;
   //         return transfer;
        }


        private static string getRetardedIpAddress()
        {

            string str = "";
            foreach (IPAddress hostAddress in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    str = hostAddress.ToString();
                    break;
                }
            }
            return str;

        }

        private static void handleError(string text)
        {
            Console.WriteLine(text);
            Console.ReadKey();
            System.Environment.Exit(0);
        }

        static string SERVER_ADRESS = "http://www.dcont.hu/service/Service.asmx";
        static string XML_HEADER = "<?xml version=\"1.0\" encoding=\"utf-8\"?> \r\n <soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"> <soap:Body>";
        static string XML_FOOTER = "</soap:Body></soap:Envelope>";

        public static DateTime SERVER_TIME = DateTime.Now;

        public static void dumpToXmlFile(List<DbMeasurementRecord> fasz)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<DbMeasurementRecord>));
            TextWriter txtWriter = new StreamWriter(Directory.GetCurrentDirectory() + "\\" + SERVER_TIME.ToString("yyyy_MM_dd_HH_mm_ss") + ".xml");
            xs.Serialize(txtWriter, fasz);
            txtWriter.Close();
        }

        public static List<DbMeasurementRecord> xmlFileToObject(string path)
        {
            List<DbMeasurementRecord> result;

            XmlSerializer serializer = new XmlSerializer(typeof(List<DbMeasurementRecord>));
            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
               result = (List<DbMeasurementRecord>)serializer.Deserialize(fileStream);
            }

            return result;
        }


        static void Main(string[] args)
        {

            //todo:
            //
            //auto com port selector, xml serializer, cli input - output, argument commands (u-pload, d-ump), more cs files for the helper functions and the classes

            //init
            string username, password;
            XmlDocument connCheckResponse = invokeService("checkConnection");
            SERVER_TIME = DateTime.Parse(extractTag(connCheckResponse, "ServerDate"));

            username = "";
            password = "";

            if (username == "" || password == "")
            {
                handleError("no username or password given");
            }

            XmlDocument loginResponse = invokeService("login", "<username>" + username + "</username><password>" + password + "</password><client_IPAddress>" + getRetardedIpAddress() + "</client_IPAddress>");
            XmlDocument patientIdResponse = invokeService("getPatientId", "<userName>" + username + "</userName><password>" + password + "</password>");

            string loginString = extractTag(loginResponse, "returnValue");
            string patientId = extractTag(patientIdResponse, "returnValue");

            //dump
            SerialPort comPort = new SerialPort();
            List<string> usbPorts = GetUsbPorts();

            GlucoResponse contents = dumpFromComPort(comPort, usbPorts[0]);

            //analyze
            float low;
            float high;
            int fieldSize;
            int softwareVersion = (int)getSoftVer((List<byte>)contents.returnValue);
            int dataLength = ((List<byte>)contents.returnValue).Count + 2; //wtf

            XmlDocument fieldSizeResponse = invokeService("getDcontFieldSize", "<deviceType>0</deviceType><softwareVersion>" + softwareVersion + "</softwareVersion><dataLength>" + dataLength + "</dataLength>");
            float[] fieldSizes = getFieldSizes(extractTag(fieldSizeResponse, "returnValue"));
            low = fieldSizes[0];
            high = fieldSizes[1];
            fieldSize = (int)fieldSizes[2];

            DbGlucoTransferRecord transferInfo = getTransferInfo(((List<byte>)contents.returnValue));
            GlucoResponse measurements = analyze(fieldSize, softwareVersion, low, high, ((List<byte>)contents.returnValue));


            dumpToXmlFile((List<DbMeasurementRecord>)measurements.returnValue);
            measurements.returnValue = xmlFileToObject([MODDED XML PATH]); //i know, it will fail, a reminder to change the path
    //        Console.ReadLine();
    //        Environment.Exit(0);


            //api stuff
            XmlDocument dumpResponse = invokeService("dump", "<patientId>" + patientId + "</patientId>" + "<data>" + dataToXml(((List<byte>)contents.returnValue)) + "</data>");
            XmlDocument setDcontResponse = invokeService("setDcontToPatient", "<patientId>" + patientId + "</patientId><dcontId>" + transferInfo.Dcont_ID +"</dcontId><forceSetToPatient>false</forceSetToPatient>");
            XmlDocument lastMeasurementsResponse = invokeService("getLastFiveMeasurement", "<patientId>" + patientId + "</patientId><DiiD>" + transferInfo.Dcont_ID + "</DiiD><ret/>");
            XmlDocument getAppSettingsResponse = invokeService("getAppSettings", "<keys><string>afp_maxGap</string><string>afp_maxLastUploadGap</string><string>afp_afterServerTime</string></keys>");

            //transfer
            DateTime lastServerMeasurement = DateTime.Parse(betweenStrings(extractTag(lastMeasurementsResponse, "ret").Split(new string[] { "</DbMeasurementRecord>" }, StringSplitOptions.None)[0], "<Date>", "</Date>"));
            string transferText = "";
            for (int i = 0; i < ((List<DbMeasurementRecord>)measurements.returnValue).Count; i++)
            {
                DbMeasurementRecord currentRecord = ((List<DbMeasurementRecord>)measurements.returnValue)[i];
                if (currentRecord.Date == lastServerMeasurement)
                {
                    Console.WriteLine(i + ". object matches");
                    transferText = measurementsToString((List<DbMeasurementRecord>)measurements.returnValue, i + 1);
                }
            }

            if (transferText.Length < 3)
            {
                handleError("transfertext is invalid, perhaps no matching last record found");
            }
            else
            {

                XmlDocument addTransferResponse = invokeService("addTransfer", "<patientId>" + patientId + "</patientId><transfer><Transfer_ID>" + transferInfo.Transfer_ID + "</Transfer_ID><Status>" +
                       transferInfo.Status + "</Status>" + "<Code>" + transferInfo.Code + "</Code>" + "<Dcont_ID>" + transferInfo.Dcont_ID + "</Dcont_ID>" + "<Version>" + transferInfo.Version + "</Version>" + "</transfer><measure>" +
                      transferText + "</measure><isTemporary>false</isTemporary>");

                handleError("ok?");
            }
        }

        static string betweenStrings(string STR, string FirstString, string LastString)
        {
            string FinalString;
            int Pos1 = STR.IndexOf(FirstString) + FirstString.Length;
            int Pos2 = STR.IndexOf(LastString);
            FinalString = STR.Substring(Pos1, Pos2 - Pos1);
            return FinalString;
        }

        static private string measurementsToString(List<DbMeasurementRecord> measurements, int from)
        {
            string output = "";
            for (int i = from; i < measurements.Count; i++)
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(DbMeasurementRecord));
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UnicodeEncoding(false, false);
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                using (StringWriter textWriter = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                    {
                        serializer.Serialize(xmlWriter, measurements[i]);
                    }
                    output = output + textWriter.ToString(); 
                }
            }

            return output;
        }

        static private string dataToXml(List<byte> input)
        {
            string str = "";
            foreach (int value in input)
            {
                str = str + "<unsignedByte>" + value + "</unsignedByte>";
            }

            return str;
        }

        static HttpWebRequest createSOAPWebRequest(string action)
        {
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(SERVER_ADRESS);
            Req.Headers.Add(@"SOAPAction:http://tempuri.org/" + action);
            Req.ContentType = "text/xml;charset=\"utf-8\"";
            Req.Accept = "text/xml";
            Req.Method = "POST";
            return Req;
        }

        static XmlDocument invokeService(string action, string body = "")
        {
            HttpWebRequest request = createSOAPWebRequest(action);
            string str;
            XmlDocument SOAPReqBody = new XmlDocument();
            SOAPReqBody.LoadXml(XML_HEADER + "<" + action + " xmlns=\"http://tempuri.org/\">" + body + "</" + action + ">" + XML_FOOTER + "\r\n\r\n");

            using (Stream stream = request.GetRequestStream())
            {
                SOAPReqBody.Save(stream);
            }
            using (WebResponse Serviceres = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
                {
                    str = rd.ReadToEnd();
                }
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(str);
            string respBody = extractTag(xmlDoc, "soap:Body");
            xmlDoc.RemoveAll();
            xmlDoc.LoadXml(respBody);
            if (xmlDoc.GetElementsByTagName("operationResult")[0].InnerText == "Success")
            {
                Console.WriteLine(action + " action ok!");
                return xmlDoc;
            }
            else
            {
                handleError(action + " request returned fail status, reason:\n" + xmlDoc.GetElementsByTagName("error")[0].InnerText + "\nexitting..");
                return xmlDoc; //fuck c# tbh
            }

        }

        static string extractTag(XmlDocument xmlDoc, string content)
        {
            return xmlDoc.GetElementsByTagName(content)[0].InnerXml;
        }

    }
}
