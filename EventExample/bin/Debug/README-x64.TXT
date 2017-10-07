What is it
----------
This is my self-made BEEP.SYS replacement driver that can
be used to let windows use the normal PC speaker again,
because the functionality of the BEEP.SYS System driver was moved 
to the Usermode Sound Agent.

For further information, see:
http://blogs.msdn.com/larryosterman/archive/2010/01/04/what-s-up-with-the-beep-driver-in-windows-7.aspx

System Requirements
-------------------
It is only needed on Windows 7 or above.

How to Install
--------------
NETPLANET GmbH (http://www.netplanet.at/) was kind enough to provide
me with a driver signature, so Testmode is no longer needed to use
this driver. Thank you, guys! :)

1) Disable Windows' own BEEP.SYS driver

        sc config Beep start= demand

2) Right click onto the beepxp.inf file and choose "Install."
   The driver will now be installed into the system. If there are no errors
   on installation, it will silently proceed.

3) Reboot your machine so that the driver gets loaded on startup.


How to Uninstall
----------------

         sc config BeepXP start= demand
         sc config Beep start= auto


Author
------
Dipl.-Ing. (FH) Ludwig Ertl
mailto: ludwig.ertl@dose.0wnz.at

You can get my other useful DOS-Utilities here:
http://www.waldbauer.com/tmp/reference.php

License
-------
This Application and Sourcecode may be distributed freely.
The Sourcecode is Licensed in Terms of the GNU GPL v3.
See: http://www.gnu.org/licenses/gpl.html
