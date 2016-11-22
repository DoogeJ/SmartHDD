using System;
using System.Collections.Generic;
using System.Management;

/*
SmartHDD
Quick SMART status readout.

Partial Copyright (c) Jaap-Willem Dooge (GitHub @DoogeJ), 2016 

Based on source code by Llewellyn Kruger, taken from http://www.know24.net/blog/C+WMI+HDD+SMART+Information.aspx

    *** ORIGINAL COPYRIGHT NOTICE ***

Partial Copyright (c) Llewellyn Kruger, 2013
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided
that the following conditions are met:

Redistributions of source code must retain the above copyright notice, this list of conditions and the
following disclaimer.

Redistributions in binary form must reproduce the above copyright notice, this list of conditions and
the following disclaimer in the documentation and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS “AS IS” AND ANY EXPRESS OR IMPLIED
WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR
TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
ADVISED OF THE POSSIBILITY OF SUCH DAMAGE. 

    *** END ORIGINAL COPYRIGHT NOTICE ***

*/

namespace SmartHDD
{

    public class HDD
    {

        public int Index { get; set; }
        public bool IsOK { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public string Serial { get; set; }
        public Dictionary<int, Smart> Attributes = new Dictionary<int, Smart>() {
                {0x00, new Smart("Invalid")},
                {0x01, new Smart("Raw read error rate")},
                {0x02, new Smart("Throughput performance")},
                {0x03, new Smart("Spinup time")},
                {0x04, new Smart("Start/Stop count")},
                {0x05, new Smart("Reallocated sector count")},
                {0x06, new Smart("Read channel margin")},
                {0x07, new Smart("Seek error rate")},
                {0x08, new Smart("Seek time performance")},
                {0x09, new Smart("Power-on hours count")},
                {0x0A, new Smart("Spinup retry count")},
                {0x0B, new Smart("Calibration retry count")},
                {0x0C, new Smart("Power cycle count")},
                {0x0D, new Smart("Soft read error rate")},
                {0x16, new Smart("Current Helium Level (HGST He8 only!)")},
                {0xAA, new Smart("Available Reserved Space (Intel SSD)")},
                {0xAB, new Smart("Program Fail Count (Kingston SSD)")},
                {0xAC, new Smart("Erase Fail Count (Kingston SSD)")},
                {0xAD, new Smart("Wear Leveling Count (SSD)")},
                {0xAE, new Smart("Unexpected power loss count (SSD)")},
                {0xAF, new Smart("Power Loss Protection Failure")},
                {0xB0, new Smart("Erase Fail Count (chip)")},
                {0xB1, new Smart("Wear Range Delta (SSD)")},
                {0xB3, new Smart("Used Reserved Block Count (Samsung)")},
                {0xB4, new Smart("Unused Reserved Block Count (HP)")},
                {0xB5, new Smart("Program Fail Count Total / Non-4K Aligned Access Count")},
                {0xB6, new Smart("Erase Fail Count (Samsung)")},
                {0xB7, new Smart("SATA Downshift Error Count / Runtime Bad Block (Western Digital, Samsung or Seagate)")},
                {0xB8, new Smart("End-to-End error / IOEDC (HP)")},
                {0xB9, new Smart("Head Stability (Western Digital)")},
                {0xBA, new Smart("Induced Op-Vibration Detection (Western Digital)")},
                {0xBB, new Smart("Reported Uncorrectable Errors (Hardware ECC)")},
                {0xBC, new Smart("Command Timeout")},
                {0xBD, new Smart("High Fly Writes (Seagate, Western Digital)")},
                {0xBE, new Smart("Airflow Temperature (WDC) / Airflow Temperature Celsius (HP) / Temperature diff. from 100")},
                {0xBF, new Smart("G-sense error rate")},
                {0xC0, new Smart("Power-off retract count / Emergency Retract Cycle Count (Fujitsu) / Unsafe Shutdown Count")},
                {0xC1, new Smart("Load Cycle Count / Load/Unload cycle count (Fujitsu)")},
                {0xC2, new Smart("HDD temperature")},
                {0xC3, new Smart("Hardware ECC recovered")},
                {0xC4, new Smart("Reallocation count")},
                {0xC5, new Smart("Current pending sector count")},
                {0xC6, new Smart("Offline scan uncorrectable count")},
                {0xC7, new Smart("UDMA CRC error rate")},
                {0xC8, new Smart("Multi-Zone Error Rate / Write error rate (Fujitsu)")},
                {0xC9, new Smart("Soft read error rate / TA Counter Detected")},
                {0xCA, new Smart("Data Address Mark errors / TA Counter Increased")},
                {0xCB, new Smart("Run out cancel")},
                {0xCC, new Smart("Soft ECC correction")},
                {0xCD, new Smart("Thermal asperity rate (TAR)")},
                {0xCE, new Smart("Flying height")},
                {0xCF, new Smart("Spin high current")},
                {0xD0, new Smart("Spin buzz")},
                {0xD1, new Smart("Offline seek performance")},
                {0xD2, new Smart("Vibration During Write (Maxtor)")},
                {0xD3, new Smart("Vibration During Write")},
                {0xD4, new Smart("Shock During Write")},
                {0xDC, new Smart("Disk shift")},
                {0xDD, new Smart("G-sense error rate")},
                {0xDE, new Smart("Loaded hours")},
                {0xDF, new Smart("Load/unload retry count")},
                {0xE0, new Smart("Load friction")},
                {0xE1, new Smart("Load/Unload cycle count")},
                {0xE2, new Smart("Load-in time")},
                {0xE3, new Smart("Torque amplification count")},
                {0xE4, new Smart("Power-off retract cycle")},
                {0xE6, new Smart("GMR head amplitude / Drive Life Protection Status")},
                {0xE7, new Smart("Temperature (HDD) / Life Left (SSD)")},
                {0xE8, new Smart("Endurance Remaining (HDD) / Available Reserved Space (Intel SSD)")},
                {0xE9, new Smart("Power-On Hours (HDD) / Media Wearout Indicator (Intel SSD)")},
                {0xEA, new Smart("Average erase count AND Maximum Erase Count")},
                {0xEB, new Smart("Good Block Count AND System (Free) Block Count")},
                {0xF0, new Smart("Head flying hours / Transfer Error Rate (Fujitsu)")},
                {0xF1, new Smart("Total LBAs Written")},
                {0xF2, new Smart("Total LBAs Read")},
                {0xF3, new Smart("Total LBAs Written Expanded")},
                {0xF4, new Smart("Total LBAs Read Expanded")},
                {0xF9, new Smart("NAND_Writes_1GiB")},
                {0xFA, new Smart("Read error retry rate")},
                {0xFB, new Smart("Minimum Spares Remaining")},
                {0xFC, new Smart("Newly Added Bad Flash Block")},
                {0xFE, new Smart("Free Fall Protection")},

                /* slot in any new codes you find in here */

                // Lots of new codes added (16 Oct. 2016), sourced from https://en.wikipedia.org/wiki/S.M.A.R.T.#Known_ATA_S.M.A.R.T._attributes
            };

    }

    public class Smart
    {
        public bool HasData
        {
            get
            {
                if (Current == 0 && Worst == 0 && Threshold == 0 && Data == 0)
                    return false;
                return true;
            }
        }
        public string Attribute { get; set; }
        public int Current { get; set; }
        public int Worst { get; set; }
        public int Threshold { get; set; }
        public int Data { get; set; }
        public bool IsOK { get; set; }

        public Smart()
        {

        }

        public Smart(string attributeName)
        {
            this.Attribute = attributeName;
        }
    }

    /// <summary>
    /// Tested against Crystal Disk Info 5.3.1 and HD Tune Pro 3.5 on 15 Feb 2013.
    /// Findings; I do not trust the individual smart register "OK" status reported back frm the drives.
    /// I have tested faulty drives and they return an OK status on nearly all applications except HD Tune. 
    /// After further research I see HD Tune is checking specific attribute values against their thresholds
    /// and and making a determination of their own (which is good) for whether the disk is in good condition or not.
    /// I recommend whoever uses this code to do the same. For example -->
    /// "Reallocated sector count" - the general threshold is 36, but even if 1 sector is reallocated I want to know about it and it should be flagged.   
    /// </summary>

    public static class Program
    {
        /*
         * This method writes an entire line to the console with the string provided, and the optional colors.
         * Taken from https://www.dotnetperls.com/console-color (and slightly modified).
         */
        static void WriteFullLine(string value = "", ConsoleColor backgroundColor = ConsoleColor.Blue, ConsoleColor foregroundColor = ConsoleColor.White)
        {
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine(value.PadRight(Console.WindowWidth - 1)); // <-- see note                   
            Console.ResetColor(); // Reset the color.
        }

        /*
         * This method writes the string provided to the console, with the optional colors.
         * Adapted from the method above.
         */
        static void WritePartial(string value = "", ConsoleColor backgroundColor = ConsoleColor.Blue, ConsoleColor foregroundColor = ConsoleColor.White)
        {
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.Write(value);
            Console.ResetColor(); // Reset the color.
        }

        /*
         * Extends strings with a Truncate() option.
         * Taken from http://stackoverflow.com/questions/6724840/how-can-i-truncate-my-strings-with-a-if-they-are-too-long (top comment, slightly modified).
         */
        public static string Truncate(this string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars - 3) + "...";
        }

        public static void Main()
        {
            Console.Title = "SmartHDD by DoogeJ";
            Console.SetWindowSize(157, 60); // fits 1280x1024 easily and gives us room to use long names

            try
            {

                // retrieve list of drives on computer (this will return both HDD's and CDROM's and Virtual CDROM's)                    
                var dicDrives = new Dictionary<int, HDD>();

                var wdSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

                // extract model and interface information
                int iDriveIndex = 0;
                foreach (ManagementObject drive in wdSearcher.Get())
                {
                    var hdd = new HDD();
                    hdd.Model = drive["Model"].ToString().Trim();
                    hdd.Type = drive["InterfaceType"].ToString().Trim();
                    dicDrives.Add(iDriveIndex, hdd);
                    iDriveIndex++;
                }

                var pmsearcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");

                // retrieve hdd serial number
                iDriveIndex = 0;
                foreach (ManagementObject drive in pmsearcher.Get())
                {
                    // because all physical media will be returned we need to exit
                    // after the hard drives serial info is extracted
                    if (iDriveIndex >= dicDrives.Count)
                        break;

                    dicDrives[iDriveIndex].Serial = drive["SerialNumber"] == null ? "None" : drive["SerialNumber"].ToString().Trim();
                    iDriveIndex++;
                }

                // get wmi access to hdd 
                var searcher = new ManagementObjectSearcher("Select * from Win32_DiskDrive");
                searcher.Scope = new ManagementScope(@"\root\wmi");

                // check if SMART reports the drive is failing
                searcher.Query = new ObjectQuery("Select * from MSStorageDriver_FailurePredictStatus");
                iDriveIndex = 0;
                foreach (ManagementObject drive in searcher.Get())
                {
                    dicDrives[iDriveIndex].IsOK = (bool)drive.Properties["PredictFailure"].Value == false;
                    iDriveIndex++;
                }

                // retrive attribute flags, value worste and vendor data information
                searcher.Query = new ObjectQuery("Select * from MSStorageDriver_FailurePredictData");
                iDriveIndex = 0;
                foreach (ManagementObject data in searcher.Get())
                {
                    Byte[] bytes = (Byte[])data.Properties["VendorSpecific"].Value;
                    for (int i = 0; i < 30; ++i)
                    {
                        try
                        {
                            int id = bytes[i * 12 + 2];

                            int flags = bytes[i * 12 + 4]; // least significant status byte, +3 most significant byte, but not used so ignored.
                                                           //bool advisory = (flags & 0x1) == 0x0;
                            bool failureImminent = (flags & 0x1) == 0x1;
                            //bool onlineDataCollection = (flags & 0x2) == 0x2;

                            int value = bytes[i * 12 + 5];
                            int worst = bytes[i * 12 + 6];
                            int vendordata = BitConverter.ToInt32(bytes, i * 12 + 7);
                            if (id == 0) continue;

                            var attr = dicDrives[iDriveIndex].Attributes[id];
                            attr.Current = value;
                            attr.Worst = worst;
                            attr.Data = vendordata;
                            attr.IsOK = failureImminent == false;
                        }
                        catch
                        {
                            // given key does not exist in attribute collection (attribute not in the dictionary of attributes)
                        }
                    }
                    iDriveIndex++;
                }

                // retreive threshold values foreach attribute
                searcher.Query = new ObjectQuery("Select * from MSStorageDriver_FailurePredictThresholds");
                iDriveIndex = 0;

                foreach (ManagementObject data in searcher.Get())
                {
                    Byte[] bytes = (Byte[])data.Properties["VendorSpecific"].Value;
                    for (int i = 0; i < 30; ++i)
                    {
                        try
                        {

                            int id = bytes[i * 12 + 2];
                            int thresh = bytes[i * 12 + 3];
                            if (id == 0) continue;

                            var attr = dicDrives[iDriveIndex].Attributes[id];
                            attr.Threshold = thresh;
                        }
                        catch
                        {
                            // given key does not exist in attribute collection (attribute not in the dictionary of attributes)
                        }
                    }

                    iDriveIndex++;
                }

                WriteFullLine("┌──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────┐");
                WriteFullLine("│ SmartHDD by DoogeJ - https://github.com/DoogeJ/SmartHDD  - (c) 2016 Jaap-Willem Dooge, 2013 Llewellyn Kruger                             version 1.0.0.1 │");
                WriteFullLine("└──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────┘");
                Console.WriteLine();

                foreach (var drive in dicDrives)
                {

                    WriteFullLine("┌──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────┐");
                    WriteFullLine(string.Format("{0, -155}", string.Format("│ DRIVE ({0}): " + drive.Value.Model + " (S/N: " + drive.Value.Serial + ") - " + drive.Value.Type, ((drive.Value.IsOK) ? "OK" : "BAD"))) + "│");
                    WriteFullLine("├──────────────────────────────────────────────────────────────────────────────────────────────┬───────────┬───────────┬───────────┬──────────────┬────────┤");
                    WriteFullLine("│ ID                                                                                           │   Current │     Worst │ Threshold │         Data │ Status │");

                    bool oddLine = false;
                    ConsoleColor rowColor = ConsoleColor.DarkBlue;

                    foreach (var attr in drive.Value.Attributes)
                    {

                        if (attr.Value.HasData)
                        {

                            WritePartial("│");
                            WritePartial(" " + String.Format("{0, -92}", attr.Value.Attribute.Truncate(92)), rowColor);
                            WritePartial(" │ ", rowColor);
                            WritePartial(String.Format("{0, 9}", attr.Value.Current.ToString().Truncate(9)), rowColor);
                            WritePartial(" │ ", rowColor);
                            WritePartial(String.Format("{0, 9}", attr.Value.Worst.ToString().Truncate(9)), rowColor);
                            WritePartial(" │ ", rowColor);
                            WritePartial(String.Format("{0, 9}", attr.Value.Threshold.ToString().Truncate(9)), rowColor);
                            WritePartial(" │ ", rowColor);
                            WritePartial(String.Format("{0, 12}", attr.Value.Data.ToString().Truncate(12)), rowColor);
                            WritePartial(" │ ", rowColor);
                            WritePartial(String.Format("{0, -7}", ((attr.Value.IsOK) ? "OK" : "")), rowColor);
                            WritePartial("│" + Environment.NewLine);

                            if (oddLine)
                            {
                                rowColor = ConsoleColor.DarkBlue;
                                oddLine = false;
                            }
                            else
                            {
                                rowColor = ConsoleColor.Blue;
                                oddLine = true;
                            }

                        }

                    }

                    WriteFullLine("└──────────────────────────────────────────────────────────────────────────────────────────────┴───────────┴───────────┴───────────┴──────────────┴────────┘");
                    Console.WriteLine();
                }

                WritePartial(" Press enter to continue...", ConsoleColor.Black, ConsoleColor.White);
                Console.ReadLine();
            }

            catch (ManagementException e)
            {
                Console.WriteLine("An error occurred while querying for WMI data: " + e.Message);
                Console.ReadLine();
            }

        }

    }

}
