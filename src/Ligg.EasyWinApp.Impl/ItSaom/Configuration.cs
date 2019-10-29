using System;
using System.IO;
using Ligg.Base.Handlers;
using Ligg.Base.Helpers;
using Ligg.EasyWinApp.Implementation.DataModel;
using Ligg.EasyWinApp.Implementation.Services;
using Ligg.EasyWinApp.ImplInterface;

namespace Ligg.EasyWinApp.Implementation
{

    internal static class Configuration
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;

        //#property
        internal static string OrganizationCode;
        internal static OrganizationSetting OrganizationSetting = null;

        internal static string StartUpDirectory;
        internal static string AppCode;
        internal static string ImplDir;
        internal static string DataDir;


        internal static void Init()
        {
            try
            {
                OrganizationCode = GlobalConfiguration.OrganizationCode;
                EncryptionHelper.Key1 = GlobalConfiguration.GlobalKey1;
                EncryptionHelper.Key2 = GlobalConfiguration.GlobalKey2;
                AnnexHelper.DefaultLanguageCode = GlobalConfiguration.DefaultLanguageCode;
                AppCode = GlobalConfiguration.AppCode;
                Configuration.SetPaths();
                InitOrganizationSetting();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".Init Error:" + ex.Message);
            }
        }


        private static void SetPaths()
        {
            try
            {
                StartUpDirectory = DirectoryHelper.DeleteLastSlashes(Directory.GetCurrentDirectory());
                ImplDir = GlobalConfiguration.ImplementationDir;
                DataDir = ImplDir + "\\Data";
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".SetPaths Error:" + ex.Message);
            }
        }


        private static void InitOrganizationSetting()
        {
            try
            {
                var xmlPath = DataDir + "\\OrganizationSetting";
                var xmlMgr = new XmlHandler(xmlPath);
                OrganizationSetting = xmlMgr.ConvertToObject<OrganizationSetting>();
                //+2
                //OrganizationSetting.RunAsAdminLocalAccountPassword = EncryptionHelper.SmDecrypt(OrganizationSetting.RunAsAdminLocalAccountPassword);
                //OrganizationSetting.RunAsAdminDomainAccountPassword = EncryptionHelper.SmDecrypt(OrganizationSetting.RunAsAdminDomainAccountPassword);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".InitOrganizationSetting Error:" + ex.Message);
            }
        }



    }

}






