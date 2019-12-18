using System;
using Ligg.Base.DataModel;

namespace Ligg.EasyWinApp.Implementation.DataModel
{
    public class LogonResult
    {
        public Int64 UserId
        {
            get;
            set;
        }

        public string UserToken
        {
            get;
            set;
        }

        public UniversalResult UniversalResult
        {
            get;
            set;
        }

    }
}
