using System.Runtime.InteropServices;
using System.Windows;

namespace CST_EMI_Shield_Wizard
{
    public static class COMInterop
    {
        [DllImport("oleaut32.dll", PreserveSig = false)]
        public static extern void GetActiveObject(ref Guid rclsid, IntPtr pvReserved, [MarshalAs(UnmanagedType.IUnknown)] out object ppunk);

        [DllImport("ole32.dll")]
        public static extern int CLSIDFromProgID([MarshalAs(UnmanagedType.LPWStr)] string lpszProgID, out Guid pclsid);

        public static object GetActiveCOMObject(string progId)
        {
            CLSIDFromProgID(progId, out Guid clsid);
            GetActiveObject(ref clsid, IntPtr.Zero, out object obj);
            return obj;
        }
    }
    public class CSTController
    {
        public CSTController()
        {
            // Инициализация COM библиотеки
            int hr = CoInitialize(IntPtr.Zero);
            if (hr < 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }
        }

        ~CSTController()
        {
            // Завершение работы с COM библиотекой
            CoUninitialize();
        }

        public void LaunchCST()
        {
            try
            {
                dynamic cstApp;
                try
                {
                    cstApp = COMInterop.GetActiveCOMObject("CSTStudio.Application");
                }
                catch (COMException)
                {
                    Type cstType = Type.GetTypeFromProgID("CSTStudio.Application");
                    if (cstType == null)
                    {
                        throw new Exception("CST Studio Suite не установлен или ProgID неверен.");
                    }
                    cstApp = Activator.CreateInstance(cstType);
                }

                if (cstApp == null)
                {
                    throw new Exception("Не удалось запустить CST Studio Suite.");
                }

                MessageBox.Show("CST Studio Suite успешно запущен", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [DllImport("ole32.dll")]
        private static extern int CoInitialize(IntPtr pvReserved);

        [DllImport("ole32.dll")]
        private static extern void CoUninitialize();
    }

}
