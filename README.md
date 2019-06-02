# dcontUploader
**first actual working build, with a very lot todo-s**

my plans are to have a command line tool which can either dump a device from usb and save the contents to a file, or can upload the previously dumped file

other notes:
- you will need the latest drivers installed from https://www.dcont.hu/securedAdatfeltolto/DcontAFP.application
- for the dumping process i dont think we really need a login, but the server with the responses is very buggy, and often fails
- currently im using http, for debugging purposes, it will change in the future
- since i dont like c#, and the original program was written in that, i couldnt figure out the public and private and static stuff which c# offers, so currently everything is in one file with static private prefixes, again, this will change
