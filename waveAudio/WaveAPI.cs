using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace waveAudio
{
    class WaveAPI
    {
        [DllImport("winmm.dll")]
        public static extern int waveInOpen(IntPtr lphwavein, uint udeviceid, ref waveStruct.tWAVEFORMATEX lpformat, int dwcallback, int dwinstance, long dwflags);
        [DllImport("winmm.dll")]
        public static extern int waveOutOpen(IntPtr lphwavein, uint udeviceid, ref waveStruct.tWAVEFORMATEX lpformat, int dwcallback, int dwinstance, long dwflags);
        [DllImport("winmm.dll")]
        public static extern int waveInPrepareHeader(IntPtr hwave,ref waveStruct.wavehdr_tag wavehdr,int usize);
        [DllImport("winmm.dll")]
        public static extern int waveOutPrepareHeader(IntPtr hwave,ref waveStruct.wavehdr_tag wavehdr, int usize);

    }
}
