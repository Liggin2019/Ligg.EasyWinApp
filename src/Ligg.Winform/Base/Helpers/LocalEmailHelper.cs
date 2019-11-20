using System;
using System.Diagnostics;

namespace Ligg.Base.Helpers
{
    public static class LocalEmailHelper
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
        public static string MailTo;
        public static string Subject;
        public static string Body;

        public static void Send()
        {
            Send(MailTo, Subject, Body);
        }

        public static void Send(string mailTo, string subject, string body)
        {
            mailTo = string.IsNullOrEmpty(mailTo) ? MailTo : mailTo;
            subject = string.IsNullOrEmpty(subject) ? Subject : subject;
            body = string.IsNullOrEmpty(body) ? Body : body;
            var mailContent = string.Format("mailto:{0}?subject={1}&body={2}", mailTo, subject, body);
            try
            {
                var process = new Process();
                process.SynchronizingObject = null;
                process.StartInfo.FileName = mailContent;
                process.StartInfo.LoadUserProfile = false;
                process.StartInfo.StandardErrorEncoding = null;
                process.StartInfo.StandardOutputEncoding = null;
                process.Start();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".Send Error: " + ex.Message);
            }
        }
    }
}
