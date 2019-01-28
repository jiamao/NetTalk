using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Media;

namespace waveAudio
{
    public class initWaveDriver
    {
        const int INP_BUFFER_SIZE = 512;//缓冲区大小
        const int WAVE_FORMAT_PCM =    1;
        const long WHDR_DONE = 0x00000001;  /* done bit */
        const long WHDR_PREPARED = 0x00000002;  /* set if this header has been prepared */
        const long WHDR_BEGINLOOP = 0x00000004;  /* loop start block */
        const long WHDR_ENDLOOP = 0x00000008; /* loop end block */
        const long WHDR_INQUEUE = 0x00000010;  /* reserved for driver */
        
         const long  CALLBACK_TYPEMASK   =0x00070000l;    /* callback type mask */
         const long CALLBACK_NULL = 0x00000000l;   /* no callback */
         const long CALLBACK_WINDOW = 0x00010000l;    /* dwCallback is a HWND */
         const long CALLBACK_TASK = 0x00020000l;   /* dwCallback is a HTASK */
         const long CALLBACK_FUNCTION = 0x00030000l;   /* dwCallback is a FARPROC */
        const uint WAVE_MAPPER = 0;

        int nIn = 0;       // pBufferIn[2]中，当前播放缓冲区号
        int nOut = 0;      // pBufferOut[2]中，当前录音缓冲区号
        int nOutSend = 0;  // pBufferOutSend[2]中，当前发送缓冲区

        static waveStruct.wavehdr_tag[]     pWaveHdrIn=new waveStruct.wavehdr_tag[2];   // 用于录音的PWAVEHDR结构数组
        static waveStruct.wavehdr_tag[]     pWaveHdrOut=new waveStruct.wavehdr_tag[2];  // 用于播放的PWAVEHDR结构数组
        static waveStruct.tWAVEFORMATEX waveform=new waveStruct.tWAVEFORMATEX();   // 用于打开音频设备的WAVEFORMATEX结构
        static IntPtr      hWaveIn ;        // 录音设备句柄
        static IntPtr     hWaveOut ;       // 播放设备句柄


        static byte[][]        pBufferIn=new byte[2][];    // 用于接收和播放的两块缓冲区
        static byte[][]       pBufferOut=new byte[2][];   // 用于录音的两块缓冲区
        public void initWave()
        {
            // 初始化waveform
            waveform.wFormatTag= WAVE_FORMAT_PCM;  // 采样方式,PCM(脉冲编码调制)
            waveform.nChannels = 1;                 // 单声道
            waveform.nSamplesPerSec = 8000;            // 采样率 8KHz
            waveform.nAvgBytesPerSec = 8000;             // 数据率 8KB/s
            waveform.nBlockAlign = 1;   // 最小块单元，wBitsPerSample×nChannels/8
            waveform.wBitsPerSample = 8;                // 样本大小为8bit
            waveform.cbSize = 0;                // 附加格式信息

            for (int HdrNum = 0; HdrNum < 2; HdrNum++)
            {
                // 为缓冲区分配内存
                pBufferIn[HdrNum] = new byte[INP_BUFFER_SIZE * 2];  // 播放缓冲区   
                pBufferOut[HdrNum] = new byte[INP_BUFFER_SIZE];    // 录音缓冲区

                pWaveHdrIn[HdrNum] = new waveStruct.wavehdr_tag();   //  录音设备结构
                pWaveHdrOut[HdrNum] = new waveStruct.wavehdr_tag();   //  播放设备结构
                
                pWaveHdrOut[HdrNum].lpData = pBufferOut[HdrNum];
                pWaveHdrOut[HdrNum].dwBufferLength = INP_BUFFER_SIZE * 2;
                pWaveHdrOut[HdrNum].dwBytesRecorded = 0;
                pWaveHdrOut[HdrNum].dwUser = 0;
                pWaveHdrOut[HdrNum].dwFlags = WHDR_BEGINLOOP | WHDR_ENDLOOP;
                pWaveHdrOut[HdrNum].dwLoops = 1;
                pWaveHdrOut[HdrNum].lpNext = IntPtr.Zero;
                pWaveHdrOut[HdrNum].reserved = 0;

                pWaveHdrIn[HdrNum].lpData = pBufferOut[HdrNum];
                pWaveHdrIn[HdrNum].dwBufferLength = INP_BUFFER_SIZE;
                pWaveHdrIn[HdrNum].dwBytesRecorded = 0;
                pWaveHdrIn[HdrNum].dwUser = 0;
                pWaveHdrIn[HdrNum].dwFlags = WHDR_BEGINLOOP | WHDR_ENDLOOP;
                pWaveHdrIn[HdrNum].dwLoops = 1;
                pWaveHdrIn[HdrNum].lpNext = IntPtr.Zero;
                pWaveHdrIn[HdrNum].reserved = 0;
            }
            
            int result = 0;
            // 打开播放波形音频设备
            result = WaveAPI.waveOutOpen(hWaveOut, WAVE_MAPPER, ref waveform, 0, 0, CALLBACK_WINDOW);

            // 打开录制波形音频设备
            if (result == 0)
                result = WaveAPI.waveInOpen(hWaveIn, WAVE_MAPPER, ref waveform, 0, 0, CALLBACK_WINDOW);
        }
    }
}
