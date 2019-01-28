using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Media;

namespace waveAudio
{
    public class initWaveDriver
    {
        const int INP_BUFFER_SIZE = 512;//��������С
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

        int nIn = 0;       // pBufferIn[2]�У���ǰ���Ż�������
        int nOut = 0;      // pBufferOut[2]�У���ǰ¼����������
        int nOutSend = 0;  // pBufferOutSend[2]�У���ǰ���ͻ�����

        static waveStruct.wavehdr_tag[]     pWaveHdrIn=new waveStruct.wavehdr_tag[2];   // ����¼����PWAVEHDR�ṹ����
        static waveStruct.wavehdr_tag[]     pWaveHdrOut=new waveStruct.wavehdr_tag[2];  // ���ڲ��ŵ�PWAVEHDR�ṹ����
        static waveStruct.tWAVEFORMATEX waveform=new waveStruct.tWAVEFORMATEX();   // ���ڴ���Ƶ�豸��WAVEFORMATEX�ṹ
        static IntPtr      hWaveIn ;        // ¼���豸���
        static IntPtr     hWaveOut ;       // �����豸���


        static byte[][]        pBufferIn=new byte[2][];    // ���ڽ��պͲ��ŵ����黺����
        static byte[][]       pBufferOut=new byte[2][];   // ����¼�������黺����
        public void initWave()
        {
            // ��ʼ��waveform
            waveform.wFormatTag= WAVE_FORMAT_PCM;  // ������ʽ,PCM(����������)
            waveform.nChannels = 1;                 // ������
            waveform.nSamplesPerSec = 8000;            // ������ 8KHz
            waveform.nAvgBytesPerSec = 8000;             // ������ 8KB/s
            waveform.nBlockAlign = 1;   // ��С�鵥Ԫ��wBitsPerSample��nChannels/8
            waveform.wBitsPerSample = 8;                // ������СΪ8bit
            waveform.cbSize = 0;                // ���Ӹ�ʽ��Ϣ

            for (int HdrNum = 0; HdrNum < 2; HdrNum++)
            {
                // Ϊ�����������ڴ�
                pBufferIn[HdrNum] = new byte[INP_BUFFER_SIZE * 2];  // ���Ż�����   
                pBufferOut[HdrNum] = new byte[INP_BUFFER_SIZE];    // ¼��������

                pWaveHdrIn[HdrNum] = new waveStruct.wavehdr_tag();   //  ¼���豸�ṹ
                pWaveHdrOut[HdrNum] = new waveStruct.wavehdr_tag();   //  �����豸�ṹ
                
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
            // �򿪲��Ų�����Ƶ�豸
            result = WaveAPI.waveOutOpen(hWaveOut, WAVE_MAPPER, ref waveform, 0, 0, CALLBACK_WINDOW);

            // ��¼�Ʋ�����Ƶ�豸
            if (result == 0)
                result = WaveAPI.waveInOpen(hWaveIn, WAVE_MAPPER, ref waveform, 0, 0, CALLBACK_WINDOW);
        }
    }
}
