using System;
using System.Collections.Generic;
using System.Text;
using AXMMCFGLib;

namespace APTEventAssignment.Message
{
    class CreateSms
    {
        public void SendSMS()
        {
        //static void Main(string[] args)
        //{
            XMessageDB objMessageDB = new XMessageDB();
            XConstants objConstants = new XConstants();

            string strRecipient, strBody;

            //Console.WriteLine("API Module: {0}\nAPI Build: {1}\n", objMessageDB.Module, objMessageDB.Build);

            objMessageDB.Open(true);
            //Console.WriteLine("Open, result: {0} ({1})", objMessageDB.LastError, objMessageDB.GetErrorDescription(objMessageDB.LastError));
            if (objMessageDB.LastError != 0)
                return;

            //Console.WriteLine("Enter recipient's mobile number (like: +4412345678): ");
            //strRecipient = Console.ReadLine();
            strRecipient = "+35679951607";

            //Console.WriteLine("Enter body of the message: ");
            //strBody = Console.ReadLine();
            strBody = "Hellooo :D :D";

            object ob = (object)objMessageDB.Create();
            Console.WriteLine("Create, result: {0} ({1})", objMessageDB.LastError, objMessageDB.GetErrorDescription(objMessageDB.LastError));
            if (objMessageDB.LastError != 0)
                return;

            IXMessage objMessage = (IXMessage)ob;
            Console.WriteLine("RecordID: {0}", objMessage.ID);

            objMessage.DirectionID = objConstants.MESSAGEDIRECTION_OUT;
            objMessage.TypeID = objConstants.MESSAGETYPE_SMS;
            objMessage.StatusID = objConstants.MESSAGESTATUS_PENDING;
            objMessage.ChannelID = 1001;
            objMessage.ScheduledTime = "";
            objMessage.ToAddress = strRecipient;
            objMessage.Body = strBody;

            objMessageDB.Save(ref ob);
            //Console.WriteLine("Save, result: {0} ({1})", objMessageDB.LastError, objMessageDB.GetErrorDescription(objMessageDB.LastError));
            if (objMessageDB.LastError == 0)
                PrintMessage(objMessage);

            objMessageDB.Close();
            //Console.WriteLine("Closed.");

            //Console.WriteLine("Ready.");
        }

        static void PrintMessage(IXMessage objMessage)
        {
            Console.WriteLine("  ID               : {0}", objMessage.ID);
            Console.WriteLine("  Direction        : {0}", objMessage.DirectionID);
            Console.WriteLine("  Type             : {0}", objMessage.TypeID);
            Console.WriteLine("  Status           : {0}", objMessage.StatusID);
            Console.WriteLine("  StatusDetails    : {0}", objMessage.StatusDetailsID);
            Console.WriteLine("  ChannelID        : {0}", objMessage.ChannelID);
            Console.WriteLine("  MessageReference : {0}", objMessage.MessageReference);
            Console.WriteLine("  ScheduledTime    : {0}", objMessage.GetScheduledTimeString());
            Console.WriteLine("  LastUpdate       : {0}", objMessage.GetLastUpdateString());
            Console.WriteLine("  Sender           : {0}", objMessage.FromAddress);
            Console.WriteLine("  Recipient        : {0}", objMessage.ToAddress);
            Console.WriteLine("  Subject          : {0}", objMessage.Subject);
            Console.WriteLine("  BodyFormat       : {0}", objMessage.BodyFormatID);
            Console.WriteLine("  Body             : {0}", objMessage.Body);
            Console.WriteLine("  Trace            : {0}", objMessage.Trace);
        }
    }
}

