using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tic_ipg203_c4_homework1
{
    public delegate void DeviceStatusChangedHandler(
        homemachines device,
        string oldStatus,
        string newStatus
    );

    public delegate void MaintenanceAlertHandler(
        homemachines device,
        string message
    );

    public interface Fixable
    {
        void displayinfo();
        double calculateFixCost();
        void updatestatus(string status);
    }

    public static class DeviceValidator
    {
        public static bool IsValidDeviceID(int id)
        {
            return id > 0;
        }

        public static bool IsValidText(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static bool IsValidStatus(string status)
        {
            return status == "Fixed" ||
                   status == "Not Fixed" ||
                   status == "In Progress" ||
                   status == "Delayed";
        }
    }

    public abstract class homemachines : Fixable
    {
        private int deviceID;
        private string brand;
        private string model;
        private string status;
        private double fixCost;

        private static int totalDevices = 0;
        private static int fixedDevicesCount = 0;

        public static int TotalDevices
        {
            get { return totalDevices; }
        }

        public static int FixedDevicesCount
        {
            get { return fixedDevicesCount; }
        }

        public int DeviceID
        {
            get { return deviceID; }
            private set { deviceID = value; }
        }

        public string Brand
        {
            get { return brand; }
            private set { brand = value; }
        }

        public string Model
        {
            get { return model; }
            private set { model = value; }
        }

        public string Status
        {
            get { return status; }
            private set { status = value; }
        }

        public double FixCost
        {
            get { return fixCost; }
            protected set { fixCost = value; }
        }

        public event DeviceStatusChangedHandler StatusChanged;
        public event MaintenanceAlertHandler MaintenanceAlert;

        public homemachines(int deviceID, string brand, string model)
        {
            if (!DeviceValidator.IsValidDeviceID(deviceID))
                throw new ArgumentException("Invalid Device ID");

            if (!DeviceValidator.IsValidText(brand))
                throw new ArgumentException("Invalid Brand");

            if (!DeviceValidator.IsValidText(model))
                throw new ArgumentException("Invalid Model");

            DeviceID = deviceID;
            Brand = brand;
            Model = model;
            Status = "Not Fixed";
            FixCost = 0;

            totalDevices++;
        }

        public abstract void displayinfo();

        public abstract double calculateFixCost();

        public void updatestatus(string status)
        {
            if (!DeviceValidator.IsValidStatus(status))
                throw new ArgumentException("Invalid device status");

            string oldStatus = Status;

            if (oldStatus == status)
                return;

            if (oldStatus == "Fixed")
                fixedDevicesCount--;

            if (status == "Fixed")
                fixedDevicesCount++;

            Status = status;

            StatusChanged?.Invoke(this, oldStatus, Status);

            if (Status == "Fixed")
            {
                MaintenanceAlert?.Invoke(
                    this,
                    "Device repair has been completed."
                );
            }
            else if (Status == "Delayed")
            {
                MaintenanceAlert?.Invoke(
                    this,
                    "Device delivery is delayed."
                );
            }
        }
    }

    public class WashingMachine : homemachines
    {
        public WashingMachine(int deviceID, string brand, string model)
            : base(deviceID, brand, model)
        {
        }

        public override double calculateFixCost()
        {
            FixCost = 95;
            return FixCost;
        }

        public override void displayinfo()
        {
            Console.WriteLine($"ID: {DeviceID}");
            Console.WriteLine($"Brand: {Brand}");
            Console.WriteLine($"Model: {Model}");
            Console.WriteLine($"Status: {Status}");
            Console.WriteLine($"Fix Cost: {FixCost}");
        }
    }

    public class dishwasher : homemachines
    {
        public dishwasher(int deviceID, string brand, string model)
            : base(deviceID, brand, model)
        {
        }

        public override double calculateFixCost()
        {
            FixCost = 162;
            return FixCost;
        }

        public override void displayinfo()
        {
            Console.WriteLine($"ID: {DeviceID}");
            Console.WriteLine($"Brand: {Brand}");
            Console.WriteLine($"Model: {Model}");
            Console.WriteLine($"Status: {Status}");
            Console.WriteLine($"Fix Cost: {FixCost}");
        }
    }

    public class refrigerator : homemachines
    {
        public refrigerator(int deviceID, string brand, string model)
            : base(deviceID, brand, model)
        {
        }

        public override double calculateFixCost()
        {
            FixCost = 185;
            return FixCost;
        }

        public override void displayinfo()
        {
            Console.WriteLine($"ID: {DeviceID}");
            Console.WriteLine($"Brand: {Brand}");
            Console.WriteLine($"Model: {Model}");
            Console.WriteLine($"Status: {Status}");
            Console.WriteLine($"Fix Cost: {FixCost}");
        }
    }

    class Program
    {
        static void Main()
        {
            List<homemachines> devices = new List<homemachines>
            {
                new WashingMachine(1, "Samsung", "WM100"),
                new dishwasher(2, "alhafez", "AH55"),
                new refrigerator(3, "wattar", "WA40")
            };

            foreach (homemachines device in devices)
            {
                device.StatusChanged += OnDeviceStatusChanged;
                device.MaintenanceAlert += OnMaintenanceAlert;
            }

            devices[0].updatestatus("Fixed");
            devices[1].updatestatus("Delayed");
            devices[2].updatestatus("In Progress");

            foreach (homemachines device in devices)
            {
                device.calculateFixCost();
                device.displayinfo();

                Console.WriteLine("------------------------");
            }

            Console.WriteLine("\n===== System Statistics =====");

            Console.WriteLine("Total Devices: " + homemachines.TotalDevices);
            Console.WriteLine("Fixed Devices: " + homemachines.FixedDevicesCount);
        }

        static void OnDeviceStatusChanged(
            homemachines device,
            string oldStatus,
            string newStatus)
        {
            Console.WriteLine(
                $"Device {device.DeviceID} status changed " +
                $"from {oldStatus} to {newStatus}"
            );
        }

        static void OnMaintenanceAlert(
            homemachines device,
            string message)
        {
            Console.WriteLine(
                $"MAINTENANCE ALERT - Device {device.DeviceID}: {message}"
            );
        }
    }
}
