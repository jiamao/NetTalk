using System;
using System.Collections.Generic;
using System.Text;

namespace waveAudio
{
    public class waveStruct
    {
        public struct tWAVEFORMATEX
        {
            public int wFormatTag;         /* format type */
            public int nChannels;          /* number of channels (i.e. mono, stereo...) */
            public int nSamplesPerSec;     /* sample rate */
            public int nAvgBytesPerSec;    /* for buffer estimation */
            public int nBlockAlign;        /* block size of data */
            public int wBitsPerSample;     /* number of bits per sample of mono data */
            public int cbSize;             /* the count in bytes of the size of */           
        } 
        /* wave data block header */
        public struct wavehdr_tag
        {
            public byte[] lpData;                 /* pointer to locked data buffer */
            public int dwBufferLength;         /* length of data buffer */
            public int dwBytesRecorded;        /* used for input only */
            public int dwUser;                 /* for client's use */
            public long dwFlags;                /* assorted flags (see defines) */
            public int dwLoops;                /* loop control counter */
            public IntPtr lpNext;     /* reserved for driver */
            public int reserved;               /* reserved for driver */
        }
    }
}
