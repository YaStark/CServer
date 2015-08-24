using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.ComponentModel;
using CCommon;
using NAudio;
using NAudio.Wave;
using System.Net.Sockets;
using System.IO;

namespace CClient
{
    public class UdpClientListener : UdpListener
    {
        WaveInEvent waveIn;
        WaveOut waveOut;
        BufferedWaveProvider provider;
        NAudio.Wave.WaveFormat format = new NAudio.Wave.WaveFormat(8000, 8, 1);
        
        public IPEndPoint RemoteServer;

        public UdpClientListener()
            : base()
        {
            Name = "UdpClient";
            OnConnect += ClientUDPListener_OnConnect;
        }

        void ClientUDPListener_OnConnect(IPEndPoint Client, byte[] Data)
        {
            byte[] decodedData = Decode(Data);
            Play(decodedData);
        }

        void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            byte[] encodedData = Encode(e.Buffer, e.BytesRecorded);
            Send(RemoteServer, encodedData);
        }

        public void Stop()
        {
            if (waveIn != null)
            {
                waveIn.StopRecording();
                waveIn.Dispose();
                waveIn = null;
            }
            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }
            if (provider != null)
            {
                provider.ClearBuffer();
                provider = null;
            }
        }

        public void Start(IPEndPoint RemoteUdpPoint)
        {
            RemoteServer = RemoteUdpPoint;

            provider = new BufferedWaveProvider(format);

            if (waveIn == null)
            {
                waveIn = new WaveInEvent();
                waveIn.WaveFormat = format;
                waveIn.BufferMilliseconds = 500;
                waveIn.DataAvailable += waveIn_DataAvailable;
                waveIn.StartRecording();
            }
            if (waveOut == null)
            {
                waveOut = new WaveOut();
                waveOut.DesiredLatency = 500;
                waveOut.Init(provider);
                waveOut.Play();
            }
        }

        private void Play(byte[] pcmData)
        {
            if(provider != null) provider.AddSamples(pcmData, 0, pcmData.Length);
        }

        private byte[] Decode(byte[] SoundData)
        {
            return SoundData;
        }

        private byte[] Encode(byte[] SoundData, int Size)
        {
            return SoundData.Take(Size).ToArray();
        }
    }
}