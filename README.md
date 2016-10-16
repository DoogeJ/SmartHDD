# SmartHDD
Quick SMART status readout.

Based on source code by Llewellyn Kruger, taken from http://www.know24.net/blog/C+WMI+HDD+SMART+Information.aspx

Modifications by me:
* Added a lot of new fields, mainly for SSD support
* Created a fancy user interface
* Request administrator-permissions on launch

## Screenshot:
![Alt text](/smarthdd.png?raw=true "SmartHDD screenshot")

## License / Copyright:
Please see the '**LICENSE**' file.

## Note by the original author:
> Tested against Crystal Disk Info 5.3.1 and HD Tune Pro 3.5 on 15 Feb 2013.  
> Findings; I do not trust the individual smart register "OK" status reported back frm the drives.  
> I have tested faulty drives and they return an OK status on nearly all applications except HD Tune.  
> After further research I see HD Tune is checking specific attribute values against their thresholds and and making a determination of their own (which is good) for whether the disk is in good condition or not.  
> I recommend whoever uses this code to do the same. For example --> "Reallocated sector count" - the general threshold is 36, but even if 1 sector is reallocated I want to know about it and it should be flagged.  

# Development
This section is focussed on development of SmartHDD, in case you want to build your own version(s).

## Environment
This application was written using Visual Studio 2015 and .NET Framework 4.5.2 on Windows 10 1511.  
It will most likely build fine on different configurations, but might require some modifications.

## Updating the version number
The version number is stored in two locations:
* In the projects assembly info file: **'SmartHDD\Properties\AssemblyInfo.cs'**
