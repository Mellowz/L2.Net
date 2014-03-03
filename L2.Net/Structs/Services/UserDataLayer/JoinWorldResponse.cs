namespace L2.Net.Structs.Services
{
    /// <summary>
    /// Join world failure reasons.
    /// </summary>
    public enum JoinWorldRequestResult : byte
    {
        /// <summary>
        /// System error occurred.
        /// </summary>
        SystemError = 0x01,
        /// <summary>
        /// User login or password is wrong ?..
        /// </summary>
        UserOrPasswordWrong = 0x02,
        /// <summary>
        /// Password is incorrect ?..
        /// </summary>
        PasswordIsIncorrect = 0x03,
        /// <summary>
        /// Access failed.
        /// </summary>
        AccessFailed = 0x04,
        /// <summary>
        /// Too many connected users.
        /// </summary>
        TooManyPlayers = 0x0f,
        /// <summary>
        /// Join world request accepted.
        /// </summary>
        Accepted = 0xff
    }

    /// <summary>
    /// Join world response.
    /// </summary>
    public struct JoinWorldResponse
    {
        /// <summary>
        /// Packet representation opcodes.
        /// </summary>
        public static readonly byte[] Opcodes = 
        { 
            UserDataLayer.Identity, 
            UserDataLayer.JoinWorldResponse
        };

        /// <summary>
        /// <see cref="JoinWorldResponse"/> request unique id.
        /// </summary>
        public readonly long RequestID;

        /// <summary>
        /// Request result.
        /// </summary>
        public readonly JoinWorldRequestResult Result;

        /// <summary>
        /// Initializes new instance of <see cref="JoinWorldResponse"/> struct.
        /// </summary>
        /// <param name="requestID"><see cref="JoinWorldResponse"/> unique id.</param>
        /// <param name="result">Request result.</param>
        public JoinWorldResponse( long requestID, JoinWorldRequestResult result )
        {
            RequestID = requestID;
            Result = result;
        }

        /// <summary>
        /// Initializes new instance of <see cref="JoinWorldResponse"/> struct.
        /// </summary>
        /// <param name="p"><see cref="Packet"/> to initialize from.</param>
        public JoinWorldResponse( Packet p )
        {
            RequestID = p.ReadLong();
            Result = ( JoinWorldRequestResult )p.ReadByte();
        }

        /// <summary>
        /// Converts current struct to it's <see cref="Packet"/> equivalent.
        /// </summary>
        /// <returns><see cref="Packet"/> equivalent of current struct.</returns>
        public Packet ToPacket()
        {
            Packet p = new Packet(Opcodes);
            p.WriteLong(RequestID);
            p.WriteByte(( byte )Result);
            return p;
        }
    }
}