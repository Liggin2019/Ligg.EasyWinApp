using System;
using System.Data;

namespace Ligg.EasyWinApp.ImplInterface
{
    public interface IAdapter
    {
        void Initialize();
        string ResolveConstants(string text);
        string Act(string funcName, string[] paramArray);
        string GetText(string funName, string[] paramArray);
        DataTable GetValueTextDataTable(string funName, string[] paramArray);
    }


}
