using System;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;

namespace Me.Utils
{

    public static class MyString
    {
        public static string SwapChars(char[] chars)
        {
            for (int i = 0; i <= chars.Length - 2; i += 2)
            {
                Array.Reverse(chars, i, 2);
            }
            return new string(chars).Trim();
        }

    }

    public static class MyWmi
    {

        public static WmiDonneesDisque GetSerialNumberWmi(int index)
        {
            WmiDonneesDisque dd = new WmiDonneesDisque();

            try
            {
                ManagementObjectSearcher objSearcherMedia = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PhysicalMedia");
                foreach (ManagementObject objQueryMedia in objSearcherMedia.Get())
                {
                    dd.DeviceID = objQueryMedia["Tag"].ToString();
                    if (dd.DeviceID.Contains(index.ToString()))
                    {
                        dd.NumeroSerie = objQueryMedia["SerialNumber"] != null ? objQueryMedia["SerialNumber"].ToString().Trim() : "";
                        if (dd.NumeroSerie != string.Empty)
                        {
                            ManagementObjectSearcher objSearcherDrive = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DiskDrive WHERE DeviceID='\\\\\\\\.\\\\" + dd.DeviceID.Substring(4) + "'");
                            foreach (ManagementObject objQueryDrive in objSearcherDrive.Get())
                            {
                                dd.Modele = objQueryDrive["Model"].ToString();
                                dd.Type = objQueryDrive["InterfaceType"].ToString();
                            }
                            break;
                        }

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dd;
        }
        public class WmiDonneesDisque
        {
            public string DeviceID;
            public string NumeroSerie;
            public string Modele;
            public string Type;
        }

    }

    public static class Win32Api
    {

        public const Int64 INVALID_HANDLE_VALUE = -1;
        public const int DFP_RECEIVE_DRIVE_DATA = 0x7c088;
        public const int CREATE_NEW = 1;
        public const int OPEN_EXISTING = 3;
        public const uint GENERIC_READ = 0x80000000;
        public const int GENERIC_WRITE = 0x40000000;
        public const int FILE_SHARE_READ = 0x1;
        public const int FILE_SHARE_WRITE = 0x2;
        public const int VER_PLATFORM_WIN32_NT = 2;

        [StructLayout(LayoutKind.Sequential)]
        public class IDSECTOR
        {

            //0
            public short GenConfig;
            //1
            public short NumberCylinders;
            //2
            public short Reserved;
            //3
            public short NumberHeads;
            //4
            public short BytesPerTrack;
            //5
            public short BytesPerSector;
            //6
            public short SectorsPerTrack;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            //7
            public short[] VendorUnique;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            //10
            public char[] SerialNumber;
            //20
            public short BufferClass;
            //21
            public short BufferSize;
            //22
            public short ECCSize;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            //23
            public char[] FirmwareRevision;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
            //27
            public char[] ModelNumber;
            //47
            public short MoreVendorUnique;
            //48
            public short DoubleWordIO;
            //49
            public short Capabilities;
            //50
            public short Reserved1;
            //51
            public short PIOTiming;
            //52
            public short DMATiming;
            //53
            public short BS;
            //54
            public short NumberCurrentCyls;
            //55
            public short NumberCurrentHeads;
            //56
            public short NumberCurrentSectorsPerTrack;
            //57
            public int CurrentSectorCapacity;
            //59
            public short MultipleSectorCapacity;
            //60
            public short MultipleSectorStuff;
            //61
            public int TotalAddressableSectors;
            //63
            public short SingleWordDMA;
            //64
            public short MultiWordDMA;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 382)]
            //65
            public byte[] Reserved2;

            public IDSECTOR()
            {
                VendorUnique = new short[3];
                Reserved2 = new byte[382];
                FirmwareRevision = new char[8];
                SerialNumber = new char[20];
                ModelNumber = new char[40];
            }

        }

        [StructLayout(LayoutKind.Sequential, Size = 12)]
        public class DRIVERSTATUS
        {

            public byte DriveError;
            public byte IDEStatus;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] Reserved;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]

            public int[] Reserved2;
            public DRIVERSTATUS()
            {
                Reserved = new byte[2];
                Reserved2 = new int[2];
            }

        }

        [StructLayout(LayoutKind.Sequential)]
        public class SENDCMDOUTPARAMS
        {

            public int BufferSize;
            public DRIVERSTATUS Status;

            public IDSECTOR IDS;
            public SENDCMDOUTPARAMS()
            {
                Status = new DRIVERSTATUS();
                IDS = new IDSECTOR();
            }

        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SENDCMDINPARAMS
        {

            // Buffer size in bytes
            public int cBufferSize;
            // Structure with drive register values.
            public IDEREGS irDriveRegs;
            // Physical drive number to send command to (0,1,2,3).
            public byte bDriveNumber;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            // Bytes reserved
            public byte[] bReserved;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            // DWORDS reserved
            public int[] dwReserved;
            // Input buffer.
            public byte bBuffer;

        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IDEREGS
        {

            // // Used for specifying SMART "commands".
            public byte bFeaturesReg;
            // // IDE sector count register
            public byte bSectorCountReg;
            // // IDE sector number register
            public byte bSectorNumberReg;
            // // IDE low order cylinder value
            public byte bCylLowReg;
            // // IDE high order cylinder value
            public byte bCylHighReg;
            // // IDE drive/head register
            public byte bDriveHeadReg;
            // // Actual IDE command.
            public byte bCommandReg;
            // // reserved for future use.  Must be zero.
            public byte bReserved;

        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Int64 CreateFile(
             [MarshalAs(UnmanagedType.LPTStr)] string filename,
             [MarshalAs(UnmanagedType.U4)] FileAccess access,
             [MarshalAs(UnmanagedType.U4)] FileShare share,
             IntPtr securityAttributes, // optional SECURITY_ATTRIBUTES struct or IntPtr.Zero
             [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
             [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
             int templateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(int hObject);

        [DllImport("Kernel32.dll", SetLastError = false, CharSet = CharSet.Auto)]
        public static extern bool DeviceIoControl(
                int hDevice,
                int IoControlCode,
                [MarshalAs(UnmanagedType.AsAny)]
                [In] object InBuffer,
                        uint nInBufferSize,
                        [MarshalAs(UnmanagedType.AsAny)]
                [Out] object OutBuffer,
                uint nOutBufferSize,
                ref uint pBytesReturned,
                [In]  IntPtr Overlapped);

    }

}

namespace Me.Utils.MySystem
{
    public class DriveInfo
    {

        private int _bufferSize;
        private object _driveType;
        private object _firmware;
        private object _model;
        private object _numberCylinders;
        private object _numberHeads;
        private object _sectorsPerTrack;
        private object _serialNumber;

        public DriveInfo(int driveNumber)
        {
            int handle = 0;
            uint returnSize = 0;

            Win32Api.SENDCMDINPARAMS sci = new Win32Api.SENDCMDINPARAMS();
            Win32Api.SENDCMDOUTPARAMS sco = new Win32Api.SENDCMDOUTPARAMS();

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                handle = (int)Win32Api.CreateFile("\\\\.\\PhysicalDrive" + Convert.ToString(driveNumber), System.IO.FileAccess.Read | System.IO.FileAccess.Write, System.IO.FileShare.Read | System.IO.FileShare.Write, IntPtr.Zero, System.IO.FileMode.Open, System.IO.FileAttributes.ReadOnly, 0);
                // 9X
            }
            else {
                handle = (int)Win32Api.CreateFile("\\\\.\\Smartvsd", 0, 0, IntPtr.Zero, System.IO.FileMode.CreateNew, 0, 0);
            }


            if (handle != Win32Api.INVALID_HANDLE_VALUE)
            {
                sci.bDriveNumber = Convert.ToByte(driveNumber);
                sci.cBufferSize = Marshal.SizeOf(sco);
                sci.irDriveRegs.bDriveHeadReg = Convert.ToByte(0xa0 | (driveNumber << 4));
                sci.irDriveRegs.bCommandReg = 0xec;
                sci.irDriveRegs.bSectorCountReg = 1;
                sci.irDriveRegs.bSectorNumberReg = 1;

                if (Win32Api.DeviceIoControl(handle, Win32Api.DFP_RECEIVE_DRIVE_DATA, sci, (uint)Marshal.SizeOf(sci), sco, (uint)Marshal.SizeOf(sco), ref returnSize, IntPtr.Zero))
                {
                    _serialNumber = MyString.SwapChars(sco.IDS.SerialNumber);
                    _model = MyString.SwapChars(sco.IDS.ModelNumber);
                    _firmware = MyString.SwapChars(sco.IDS.FirmwareRevision);
                    _numberCylinders = sco.IDS.NumberCylinders;
                    _numberHeads = sco.IDS.NumberHeads;
                    _sectorsPerTrack = sco.IDS.SectorsPerTrack;
                    _bufferSize = sco.IDS.BufferSize * 512;
                    if ((sco.IDS.GenConfig & 0x80) == 0x80)
                    {
                        _driveType = DriveTypes.Removable;
                    }
                    else if ((sco.IDS.GenConfig & 0x40) == 0x40)
                    {
                        _driveType = DriveTypes.Fixed;
                    }
                    else {
                        _driveType = DriveTypes.Unknown;
                    }
                }

                Win32Api.CloseHandle(handle);

            }

        }

        public int BufferSize
        {
            get
            {
                return _bufferSize;
            }

            set
            {
                _bufferSize = value;
            }
        }

        public object DriveType
        {
            get
            {
                return _driveType;
            }

            set
            {
                _driveType = value;
            }
        }

        public object Firmware
        {
            get
            {
                return _firmware;
            }

            set
            {
                _firmware = value;
            }
        }

        public object Model
        {
            get
            {
                return _model;
            }

            set
            {
                _model = value;
            }
        }

        public object NumberCylinders
        {
            get
            {
                return _numberCylinders;
            }

            set
            {
                _numberCylinders = value;
            }
        }

        public object NumberHeads
        {
            get
            {
                return _numberHeads;
            }

            set
            {
                _numberHeads = value;
            }
        }

        public object SectorsPerTrack
        {
            get
            {
                return _sectorsPerTrack;
            }

            set
            {
                _sectorsPerTrack = value;
            }
        }

        public object SerialNumber
        {
            get
            {
                return _serialNumber;
            }

            set
            {
                _serialNumber = value;
            }
        }

        private enum DriveTypes
        {
            Fixed,
            Removable,
            Unknown
        }

    }

}
