using System;
using System.Data;

namespace Ligg.EasyWinApp.ImplInterface
{
    public interface IAdapter
    {
        void Initialize();
        string ResolveConstants(string text);
        string GetText(string funName, string[] paramArray);
        string Act(string funcName, string[] paramArray);

    }


}
