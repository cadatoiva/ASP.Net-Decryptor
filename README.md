# ASP.Net Core Protected String Decryption

This project gives a way for users to retrieve protected settings & saved data from applications using the ASP.Net Core DataProtection library.
The project currently assumes decryption keys are stored in a file on disk, which is only true for non-Windows systems.
This project is currently for use with the Jackett Indexer passwords specifically, and includes embedded data to that effect.
