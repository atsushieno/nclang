This sample is created to demonstrate how nclang can be used to generate
P/Invoke helpers.
I wrote it to generate OpenSLES API wrapper using Android NDK headers.
And I believe that how to generate P/Invoke wrappers depends on the 
actual libraries. It still generates useless code for SLES because it
doesn't match how to deal with pointers.
But you'd get the idea on how to use nclang here.

