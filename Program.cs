using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jenx.Bluetooth.GattServer.Common;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace App3
{
    class Program
    {
        static private ILogger _logger;
        static private IGattServer _gattServer;
        ///static private INotify _notifyserver;
        
        public static string deneme="empty";
        private static async Task Main(string[] args)
        {
            
            InitializeLogger();
            InitializeGattServer();
            await StartGattServer();
            await StartLooping();

        }

        private static void InitializeLogger()
        {
            _logger = new ConsoleLogger();
        }

        private static void InitializeGattServer()
        {
            _gattServer = new Jenx.Bluetooth.GattServer.Common.GattServer(GattCharacteristicIdentifiers.ServiceId, _logger);
            _gattServer.OnChararteristicWrite += GattServerOnChararteristicWrite; 
        }

        private static async Task StartGattServer()
        {
            try
            {
                
                await _gattServer.Initialize();

            }
            catch
            {
                
                throw;
            }
            Console.WriteLine("Bir mesaj giriniz:");
            string mesaj = Console.ReadLine();
            await _gattServer.AddReadWriteCharacteristicAsync(GattCharacteristicIdentifiers.DataExchange,"Data exchange");
            await _gattServer.AddReadCharacteristicAsync(GattCharacteristicIdentifiers.FirmwareVersion, mesaj, "Server mesajı");
            await _gattServer.AddWriteCharacteristicAsync(GattCharacteristicIdentifiers.InitData, "Init info");
            await _gattServer.AddReadCharacteristicAsync(GattCharacteristicIdentifiers.ManufacturerName, "empty", "Manufacturer");

            _gattServer.Start();
            
            string title, content;

            content = "BLE serverı açıldı !";

            title = "BLE server durumu:";

            ToastNotifier notifier = ToastNotificationManager.CreateToastNotifier();
            XmlDocument toastXDoc = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            XmlNodeList toastNodes = toastXDoc.GetElementsByTagName("text");
            toastNodes.Item(0).AppendChild(toastXDoc.CreateTextNode(title));
            toastNodes.Item(1).AppendChild(toastXDoc.CreateTextNode(content));
            IXmlNode toastNode = toastXDoc.SelectSingleNode("/toast");
            XmlElement audioElem = toastXDoc.CreateElement("audio");
            audioElem.SetAttribute("src", "ms-winsoundevent:Notification.Reminder");

            ToastNotification toast = new ToastNotification(toastXDoc);
            notifier.Show(toast);


        }

        private static async Task StartLooping()
        {
            System.ConsoleKeyInfo cki;
            System.Console.CancelKeyPress += new System.ConsoleCancelEventHandler(KeyPressHandler);

            while (true)
            {
                
                //cki = System.Console.ReadKey(true);
                //await _logger.LogMessageAsync($"  Key pressed: {cki.Key}\n");
                //if (cki.Key == System.ConsoleKey.B)
                //{
                    Console.WriteLine("Bir mesaj giriniz:");
                    string mesaj = Console.ReadLine();
                    if (mesaj=="exit")
                {
                    break;
                }
                    else
                {
                   //_gattServer.Stop();
                   //await _gattServer.Initialize();

                   // await _gattServer.AddReadWriteCharacteristicAsync(GattCharacteristicIdentifiers.DataExchange, "Data exchange");
                    await _gattServer.AddNotifyWriteCharacteristicAsync(GattCharacteristicIdentifiers.FirmwareVersion, mesaj, "Server mesajı");
                    //await _gattServer.AddWriteCharacteristicAsync(GattCharacteristicIdentifiers.InitData, "Init info");
                    //await _gattServer.AddReadCharacteristicAsync(GattCharacteristicIdentifiers.ManufacturerName, deneme, "Manufacturer");


                   //_gattServer.Start();

                }

                //}
                // Exit if the user pressed the 'X' key.
                //if (cki.Key == System.ConsoleKey.X) break;
            }
        }

        private static async void KeyPressHandler(object sender, System.ConsoleCancelEventArgs args)
        {
            await _logger.LogMessageAsync("\nThe read operation has been interrupted.");
            await _logger.LogMessageAsync($"  Key pressed: {args.SpecialKey}");
            await _logger.LogMessageAsync($"  Cancel property: {args.Cancel}");
            await _logger.LogMessageAsync("Setting the Cancel property to true...");
            args.Cancel = true;

            await _logger.LogMessageAsync($"  Cancel property: {args.Cancel}");
            await _logger.LogMessageAsync("The read operation will resume...\n");
        }

        private static async void GattServerOnChararteristicWrite(object myObject, CharacteristicEventArgs myArgs)
        {
            
            await _logger.LogMessageAsync($"Characteristic with Guid: {myArgs.Characteristic.ToString()} changed: !.?{myArgs.Value.ToString()}?");
            deneme=myArgs.Value.ToString();


        }





        private static void StopGattServer()
        {
            _gattServer.Stop();

        }


    }
}
