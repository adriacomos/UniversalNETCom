using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CLProtocol;
using CLProtocol.Codecs;
using ServiceProvider;
using ServiceProvider.Events;

namespace UniversalNETCom
{
    public class UniversalNETCom
    {
        CCodecBase mCodec;
        IServiceDataProviderConnectionless mNETService;

        CPacket mLastPacket;

        public UniversalNETCom( CCodecBase codec, IServiceDataProviderConnectionless netService  )
        {
            mCodec = codec;
            mNETService = netService;

            mCodec.EventNewPacket += mCodec_EventNewPacket;
            mNETService.OnDataReceived += mNETService_OnDataReceived; ;
        }


        void mNETService_OnDataReceived(object sender, OnDataReceivedArgs e)
        {
            string strBuffer = Encoding.UTF8.GetString(e.Buffer);
            mCodec.Decode(strBuffer);
        }

        void mCodec_EventNewPacket(object sender, CLProtocol.Events.COnNewPacketArgs e)
        {
            mLastPacket = e.Packet;
        }

        public void getData<T>(ref T data)
        {
            CLTypeBuilder.FillClass<T>(out data, mLastPacket);
        }

        public dynamic getData()
        {
            return CLTypeBuilder.CreateNewObject(mLastPacket);

        }


    }
}
