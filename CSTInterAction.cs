using System;
using System.Runtime.InteropServices;
using System.Windows;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

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
        private dynamic cstApp;

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

        public void RunSimulation()
        {
            try
            {
                ScriptEngine engine = Python.CreateEngine();
                ScriptScope scope = engine.CreateScope();

                string pythonScript = @"
import sys
import clr
import os
import matplotlib.pyplot as plt

# Путь к библиотекам CST Studio Suite
sys.path.append(r'C:\Program Files (x86)\CST STUDIO SUITE 2020\AMD64')
clr.AddReference('Interop.CSTStudio')

from CSTStudio import Application

def create_cst_project():
    # Создание объекта приложения CST
    cst = Application.CreateObject()

    # Создание нового проекта
    project = cst.NewMWS()
    project.Store('Project', 'NewProject')

    # Добавление слоев экрана
    layers = [
        {'name': 'ScreenLayer1', 'material': 'PEC', 'x1': 0, 'x2': 10, 'y1': 0, 'y2': 10, 'z1': 0, 'z2': 0.1},
        {'name': 'ScreenLayer2', 'material': 'PEC', 'x1': 0, 'x2': 10, 'y1': 0, 'y2': 10, 'z1': 0.2, 'z2': 0.3},
    ]

    for layer in layers:
        project.Store('AddBrick')
        project.BrickName(layer['name'])
        project.SetMaterial(layer['material'])
        project.Brick(layer['x1'], layer['x2'], layer['y1'], layer['y2'], layer['z1'], layer['z2'])

    # Установка граничных условий
    project.Store('SetBoundaryCondition')
    project.BoundaryType('Open (add space)')

    # Создание плоской волны
    project.Store('AddPlaneWave')
    project.FieldSourceName('PlaneWave1')
    project.PlaneWaveDirection('z')

    # Установка датчиков электрического и магнитного полей
    project.Store('AddMonitor')
    project.MonitorType('E-field')
    project.MonitorName('EFieldMonitor')

    project.Store('AddMonitor')
    project.MonitorType('H-field')
    project.MonitorName('HFieldMonitor')

    # Настройка параметров симуляции
    project.Store('AddSimulationControl')
    project.SimulationType('FrequencyDomain')
    project.SetFrequencyRange(1e9, 10e9)

    # Запуск симуляции
    project.RunSolver()

    # Экспорт результатов
    results = project.GetResultTree()
    e_field_results = results.Get2DField('E-field')
    h_field_results = results.Get2DField('H-field')

    # Сохранение графиков
    plt.figure()
    plt.plot(e_field_results)
    plt.title('E-Field Results')
    plt.savefig(r'C:\Path\To\Your\EFieldGraph.png')

    plt.figure()
    plt.plot(h_field_results)
    plt.title('H-Field Results')
    plt.savefig(r'C:\Path\To\Your\HFieldGraph.png')

    return [r'C:\Path\To\Your\EFieldGraph.png', r'C:\Path\To\Your\HFieldGraph.png']

# Запуск функции создания проекта и получения графиков
create_cst_project()
";

                engine.Execute(pythonScript, scope);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выполнении Python-скрипта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [DllImport("ole32.dll")]
        private static extern int CoInitialize(IntPtr pvReserved);

        [DllImport("ole32.dll")]
        private static extern void CoUninitialize();
    }
}
