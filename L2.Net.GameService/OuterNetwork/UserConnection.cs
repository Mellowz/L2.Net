using System;
using System.Net.Sockets;
using L2.Net.GameService.Properties;
using L2.Net.Network;
using L2.Net.Structs.Client;
using System.Text;
using L2.Net.GameService.OuterNetwork.ClientPackets;

namespace L2.Net.GameService.OuterNetwork
{
    /// <summary>
    /// User connection state types.
    /// </summary>
    internal enum UserConnectionState : byte
    {
        /// <summary>
        /// User is disconnected.
        /// </summary>
        Disconnected = 0,
        /// <summary>
        /// User is connected.
        /// </summary>
        Connected = 1,
        /// <summary>
        /// User is authorized.
        /// </summary>
        Authed = 2,
        /// <summary>
        /// User is in game.
        /// </summary>
        InGame = 3
    }

    /// <summary>
    /// Class, that represents client connection object.
    /// </summary>
    internal class UserConnection : NetworkClient
    {
        /// <summary>
        /// Random object.
        /// </summary>
        // private static readonly L2Random m_Random = new L2Random(( int )DateTime.Now.Ticks);

        /// <summary>
        /// Provides access to user session data.
        /// </summary>
        internal UserSession Session;

        /// <summary>
        /// User connection state indicator.
        /// </summary>
        internal UserConnectionState State;

        /// <summary>
        /// Crypt object.
        /// </summary>
        private GameCrypt m_Crypt;

        /// <summary>
        /// Initializes new instance of <see cref="UserConnection"/> object.
        /// </summary>
        /// <param name="socket">Connection socket.</param>
        internal UserConnection( Socket socket )
            : base(socket)
        {
            //m_Crypt = new GameCrypt(m_Random.NextBytes(), m_Random.NextBytes());
            //BeginReceive();

            State = UserConnectionState.Connected;
            BeginSession();
        }

        /// <summary>
        /// Begins receiving data from client.
        /// </summary>
        public override void BeginReceive()
        {
            m_Socket.BeginReceive(m_ReceiveBuffer, 0, 0, 0, m_ReceiveCallback, null);
        }

        /// <summary>
        /// Handles incoming packet.
        /// </summary>
        /// <param name="packet">Incoming packet.</param>
        protected override void Handle( Packet packet )
        {
            Logger.WriteLine(Source.OuterNetwork, "Received: {0}", packet.ToString());

            //Logger.WriteLine(Source.Debug, "packet.FirstOpcode = {0}", packet.FirstOpcode);

            switch ( State )
            {
                case UserConnectionState.Disconnected:
                    throw new InvalidOperationException();

                case UserConnectionState.Connected:
                    {
                        switch ( packet.FirstOpcode )
                        {
                            case 0x0e: // protocol
                                {
                                    int revision = packet.ReadInt();

                                    //Logger.WriteLine(Source.Debug, "Protocol Revision: {0}", revision);

                                    if ( revision == -2 )
                                    {
                                        CloseConnection();
                                        // remove from active connections list
                                    }

                                    if ( revision < Settings.Default.WorldMinProtoRev || revision > Settings.Default.WorldMaxProtoRev )
                                    {
                                        CloseConnection();
                                        // remove from active connections, log
                                    }

                                    byte[] keyIN = BlowFishKeygen.GetNext();
                                    //byte[] keyOUT = BlowFishKeygen.GetNext();

                                    m_Crypt = new GameCrypt();
                                    m_Crypt.setKey(keyIN);

                                    SendNoCrypt(KeyPacket.Create(keyIN));

                                    BeginReceive();

                                    break;
                                }
                            case 0x2b: // login
                                {
                                    new AuthLogin(packet).RunImpl();
                                    break;
                                }
                            default:
                                {
                                    Logger.WriteLine(Source.OuterNetwork, "Unknown packet on state {0}: Opcode = {1}", State, packet.FirstOpcode);
                                    break;
                                }
                        }

                        return;
                    }

                case UserConnectionState.Authed:
                    {
                        switch (packet.FirstOpcode)
                        {
                            default:
                                {
                                    Logger.WriteLine(Source.OuterNetwork, "Unknown packet on state {0}: Opcode = {1}", State, packet.FirstOpcode);
                                    break;
                                }
                        }
                        return;
                    }
            }
        }

        /// <summary>
        /// Session start method override, instead usual <see cref="BeginReceive"/>.
        /// </summary>
        private void BeginSession()
        {
            m_Socket.BeginReceive(m_ReceiveBuffer, 0, 2, 0, SessionReceiveCallback, null);
        }

        /// <summary>
        /// Session start method override, instead usual ReceiveCallback.
        /// </summary>
        private unsafe void SessionReceiveCallback( IAsyncResult ar )
        {
            try
            {
                m_ReceivedLength += m_Socket.EndReceive(ar);


                fixed ( byte* buf = m_ReceiveBuffer )
                {
                    //Logger.WriteLine(Source.Debug, "Recieve :\r\n{0}", L2Buffer.ToString(m_ReceiveBuffer));

                    if ( !m_HeaderReceived ) //get packet capacity
                    {
                        L2Buffer.Extend(ref m_ReceiveBuffer, 0, *( ( short* )( buf ) ) - sizeof(short));
                        m_ReceivedLength = 0;
                        m_HeaderReceived = true;
                    }

                    if ( m_ReceivedLength == m_ReceiveBuffer.Length ) // all data received
                    {
                        Handle(new Packet(1, m_ReceiveBuffer));
                        m_ReceivedLength = 0;
                        m_ReceiveBuffer = m_DefaultBuffer;
                        m_HeaderReceived = false;

                        m_Socket.BeginReceive(m_ReceiveBuffer, 0, 2, 0, ReceiveCallback, null);
                    }
                    else if ( m_ReceivedLength < m_ReceiveBuffer.Length ) // not all data received
                        m_Socket.BeginReceive(m_ReceiveBuffer, m_ReceivedLength, m_ReceiveBuffer.Length - m_ReceivedLength, 0, SessionReceiveCallback, null);
                    else
                        throw new InvalidOperationException();
                }
            }
            catch ( Exception e )
            {
                Logger.Exception(e);
            }
        }

        /// <summary>
        /// Sends packet to current <see cref="UserConnection"/>.
        /// </summary>
        /// <param name="p"><see cref="Packet"/> to send.</param>
        internal void Send( Packet p )
        {
            p.Prepare(sizeof(short));
            byte[] buffer = p.GetBuffer();
            m_Crypt.Encrypt(ref buffer, 0, buffer.Length);
            SendData(buffer);
        }

        /// <summary>
        /// Sends not encrypted packet to current <see cref="UserConnection"/>.
        /// </summary>
        /// <param name="p"><see cref="Packet"/> to send.</param>
        private void SendNoCrypt( Packet p )
        {
            p.Prepare(sizeof(short));
            SendData(p.GetBuffer());
        }

        /// <summary>
        /// Receive method.
        /// </summary>       //72-2min
        protected override unsafe void ReceiveCallback( IAsyncResult ar )
        {
            try
            {
                m_ReceivedLength += m_Socket.EndReceive(ar);
                //Logger.WriteLine(Source.Debug, "m_ReceivedLength == {0}", m_ReceivedLength);

                if ( m_ReceivedLength == 0 )
                {
                    BeginReceive();
                    return;
                }

                fixed ( byte* buf = m_ReceiveBuffer )
                {
                    
                    if ( !m_HeaderReceived ) //get packet capacity
                    {
                        L2Buffer.Extend(ref m_ReceiveBuffer, 0, *( ( short* )( buf ) ) - sizeof(short));
                        m_ReceivedLength = 0;
                        m_HeaderReceived = true;
                    }

                    if ( m_ReceivedLength == m_ReceiveBuffer.Length ) // all data received
                    {
                        m_Crypt.Decrypt(ref m_ReceiveBuffer, 0, m_ReceiveBuffer.Length);
                        Handle(new Packet(1, m_ReceiveBuffer));
                        m_ReceivedLength = 0;
                        m_ReceiveBuffer = m_DefaultBuffer;
                        m_HeaderReceived = false;

                        m_Socket.BeginReceive(m_ReceiveBuffer, 0, 0, 0, ReceiveCallback, null);
                    }
                    else if ( m_ReceivedLength < m_ReceiveBuffer.Length ) // not all data received
                        m_Socket.BeginReceive(m_ReceiveBuffer, m_ReceivedLength, m_ReceiveBuffer.Length - m_ReceivedLength, 0, m_ReceiveCallback, null);
                    else
                        throw new InvalidOperationException();
                }
            }
            catch ( Exception e )
            {
                //if ( e is NullReferenceException )  // user closed connection
                //{
                //    UserConnectionsListener.RemoveFromActiveConnections(this);
                //    return;
                //}

                Logger.Exception(e);
            }
        }

        #region Crypt

        /// <summary>
        /// Helper-class.
        /// </summary>
        private struct BlowFishKeygen
        {
            private static readonly byte[] sk = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xc8, 0x27, 0x93, 0x01, 0xa1, 0x6c, 0x31, 0x97 };
            /// <summary>
            /// Generates next random blowfish key.
            /// </summary>
            /// <returns>Next random blowfish key.</returns>
            internal static byte[] GetNext()
            {
                return L2Buffer.Replace(sk, 0, L2Random.NextBytes(8), 8);
            }
        }


        /// <summary>
        /// Game crypt object class.
        /// </summary>
        /*private struct GameCrypt
        {
            /// <summary>
            /// Input crypt key.
            /// </summary>
            private unsafe fixed byte m_InputKey[16];

            /// <summary>
            /// Output crypt key.
            /// </summary>
            private unsafe fixed byte m_OutputKey[16];


            /// <summary>
            /// Initializes new instance of <see cref="GameCrypt"/> struct.
            /// </summary>
            /// <param name="input"></param>
            /// <param name="output"></param>
            internal unsafe GameCrypt( byte[] input, byte[] output )
            {
                fixed ( byte* ki = m_InputKey, ko = m_OutputKey, i = input, o = output )
                {
                    L2Buffer.Copy(i, 0, ki, 0, 16);
                    L2Buffer.Copy(o, 0, ko, 0, 16);
                    //for ( int k = 0; k < 16; k++ )
                    //{
                    //    ki[k] = i[k];
                    //    ko[k] = o[k];
                    //}
                }
            }

            /// <summary>
            /// Decrypts provided <see paramref="buffer"/>.
            /// </summary>
            /// <param name="buffer">Array of <see cref="byte"/> values to decrypt.</param>
            internal unsafe void Decrypt( ref byte[] buffer )
            {
                int temp = 0, length = buffer.Length;

                fixed ( byte* key = m_InputKey )
                {
                    for ( int i = 0; i < length; temp = buffer[i], i++ )
                        buffer[i] = ( byte )( buffer[i] ^ key[i & 15] ^ temp );

                    *( int* )( key + 8 ) += *( int* )&length;
                }
            }


            /// <summary>
            /// Encrypts provided <paramref name="buffer"/>.
            /// </summary>
            /// <param name="buffer">Array of <see cref="byte"/> values to encrypt.</param>
            internal unsafe void Encrypt( ref byte[] buffer )
            {
                int temp = 0, length = buffer.Length;

                fixed ( byte* key = m_OutputKey )
                {
                    for ( int i = 0; i < length; temp = buffer[i], i++ )
                        buffer[i] = ( byte )( buffer[i] ^ key[i & 15] ^ temp );

                    *( int* )( key + 8 ) += *( int* )&length;
                }
                /*
                 *                 int temp2 = raw[offset + i] & 0xFF;
                    temp = temp2 ^ m_KeyOut[i & 15] ^ temp;
                    raw[offset + i] = ( byte )temp;

                */
            /*}

            // one more method

            //internal unsafe void Decrypt( ref byte[] buffer )
            //{
            //    int temp = 0, temp2 = 0, length = buffer.Length;

            //    fixed ( byte* key = m_In )
            //    {
            //        for ( int i = 0; i < length; i++, temp = temp2 )
            //        {
            //            temp2 = buffer[i];
            //            buffer[i] = ( byte )( temp2 ^ key[i & 15] ^ temp );
            //        }

            //        *( int* )( key + 8 ) += *( int* )&length;
            //    }
            //}
        }*/

        public class GameCrypt
        {
            /// <summary>
            /// Input crypt key.
            /// </summary>
            private byte[] m_InputKey = new byte[16];

            /// <summary>
            /// Output crypt key.
            /// </summary>
            private byte[] m_OutputKey = new byte[16];

            private static bool _isEnabled = false;

            private static byte[] RC4_KEY = new byte[32];

            static GameCrypt()
	        {
		        RC4_KEY[0] = (byte) 0x48;
		        RC4_KEY[1] = (byte) 0x18;
		        RC4_KEY[2] = (byte) 0x9F;
		        RC4_KEY[3] = (byte) 0x10;
		        RC4_KEY[4] = (byte) 0x9E;
		        RC4_KEY[5] = (byte) 0xB2;
		        RC4_KEY[6] = (byte) 0xD4;
		        RC4_KEY[7] = (byte) 0xAF;
		        RC4_KEY[8] = (byte) 0x08;
		        RC4_KEY[9] = (byte) 0x87;
		        RC4_KEY[10] = (byte) 0xD9;
		        RC4_KEY[11] = (byte) 0x70;
		        RC4_KEY[12] = (byte) 0xCF;
		        RC4_KEY[13] = (byte) 0xA2;
		        RC4_KEY[14] = (byte) 0xFC;
		        RC4_KEY[15] = (byte) 0xC2;
		        RC4_KEY[16] = (byte) 0x58;
		        RC4_KEY[17] = (byte) 0x86;
		        RC4_KEY[18] = (byte) 0xFB;
		        RC4_KEY[19] = (byte) 0xE8;
		        RC4_KEY[20] = (byte) 0x97;
		        RC4_KEY[21] = (byte) 0x48;
		        RC4_KEY[22] = (byte) 0xEB;
		        RC4_KEY[23] = (byte) 0x5D;
		        RC4_KEY[24] = (byte) 0xC7;
		        RC4_KEY[25] = (byte) 0x8E;
		        RC4_KEY[26] = (byte) 0x9D;
		        RC4_KEY[27] = (byte) 0xA0;
		        RC4_KEY[28] = (byte) 0xB8;
		        RC4_KEY[29] = (byte) 0xAB;
		        RC4_KEY[30] = (byte) 0x14;
		        RC4_KEY[31] = (byte) 0x70;
	        }

            private RC4 rc4In = new RC4(RC4_KEY);
	        private RC4 rc4Out = new RC4(RC4_KEY);

	        public void setKey(byte[] key)
	        {
		        Array.Copy(key, 0, m_InputKey, 0, 16);
                Array.Copy(key, 0, m_OutputKey, 0, 16);
                _isEnabled = true;
	        }

	        public void setKey(byte[] key, bool value)
	        {
		        setKey(key);
	        }

	        public bool Decrypt(ref byte[] raw, int offset, int size)
	        {
		        if(!_isEnabled)
		        {
			        return true;
		        }

		        int temp = 0;
		        for(int i = 0; i < size; i++)
		        {
			        int temp2 = raw[offset + i] & 0xFF;
			        raw[offset + i] = (byte) (temp2 ^ m_InputKey[i & 15] ^ temp);
			        temp = temp2;
		        }

		        long old = m_InputKey[8] & 0xff;
		        old |= m_InputKey[9] << 8 & 0xff00;
		        old |= m_InputKey[10] << 0x10 & 0xff0000;
		        old |= m_InputKey[11] << 0x18 & 0xff000000;

		        old += size;

		        m_InputKey[8] = (byte) (old & 0xff);
		        m_InputKey[9] = (byte) (old >> 0x08 & 0xff);
		        m_InputKey[10] = (byte) (old >> 0x10 & 0xff);
		        m_InputKey[11] = (byte) (old >> 0x18 & 0xff);
		        return true;
	        }

	        public void Encrypt(ref byte[] raw, int offset, int size)
	        {
		        if(!_isEnabled)
		        {
			        _isEnabled = true;
			        return;
		        }

		        int temp = 0;
		        for(int i = 0; i < size; i++)
		        {
			        int temp2 = raw[offset + i] & 0xFF;
                    temp = temp2 ^ m_OutputKey[i & 15] ^ temp;
			        raw[offset + i] = (byte) temp;
		        }

                long old = m_OutputKey[8] & 0xff;
                old |= m_OutputKey[9] << 8 & 0xff00;
                old |= m_OutputKey[10] << 0x10 & 0xff0000;
                old |= m_OutputKey[11] << 0x18 & 0xff000000;

		        old += size;

                m_OutputKey[8] = (byte)(old & 0xff);
                m_OutputKey[9] = (byte)(old >> 0x08 & 0xff);
                m_OutputKey[10] = (byte)(old >> 0x10 & 0xff);
                m_OutputKey[11] = (byte)(old >> 0x18 & 0xff);
	        }
        }

        public class RC4
        {
            private byte[] state = new byte[256];
	        private int x;
	        private int y;

            public RC4(String key)
                : this(Encoding.Default.GetBytes(key))
	        {
	        }

            public RC4(byte[] key)
	        {

		        for(int i = 0; i < 256; i++)
		        {
			        state[i] = (byte) i;
		        }

		        x = 0;
		        y = 0;

		        int index1 = 0;
		        int index2 = 0;

		        byte tmp;

		        if(key == null || key.Length == 0)
		        {
			        throw new NullReferenceException();
		        }

		        for(int i = 0; i < 256; i++)
		        {

			        index2 = (key[index1] & 0xff) + (state[i] & 0xff) + index2 & 0xff;

			        tmp = state[i];
			        state[i] = state[index2];
			        state[index2] = tmp;

                    index1 = (index1 + 1) % key.Length;
		        }
	        }

            public void rc4(byte[] buf, int offset, int size)
	        {
		        int xorIndex;
		        byte tmp;

		        for(int i = 0; i < size; i++)
		        {
			        x = x + 1 & 0xff;
			        y = (state[x] & 0xff) + y & 0xff;

			        tmp = state[x];
			        state[x] = state[y];
			        state[y] = tmp;

			        xorIndex = (state[x] & 0xff) + (state[y] & 0xff) & 0xff;
			        buf[offset + i] ^= state[xorIndex];
		        }
	        }
        }

        #endregion
    }
}