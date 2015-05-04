using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace APTEventAssignment.HtmlHelperRepository.Images
{
    //Contains the Html Helper method for Rendering Image 
    public static class ImageHelper 
    { 
        // method used to make calls to the db and get data from the table by executing a query. 
        // also reads the byte array and convert it to bitmap
        public static MvcHtmlString ImageData(this HtmlHelper helper, int eventID) 
        { 
            TagBuilder imageData = null; //To Build the Image Tag 
            var imgUrl = new UrlHelper(helper.ViewContext.RequestContext);
            
            SqlConnection Conn = new SqlConnection("Data Source= localhost\\SQLEXPRESS;initial catalog=APTEvents;user id=csi2015users;password=csi2015p@$$!;MultipleActiveResultSets=True;App=EntityFramework"); 
            Conn.Open(); 
            SqlCommand Cmd = new SqlCommand(); 
            Cmd.Connection = Conn; 
            Cmd.CommandText = "Select [Event_Image] From Event where Event_ID=@Event_ID"; 
            Cmd.Parameters.AddWithValue("@Event_ID", eventID);
            
            SqlDataReader Reader = Cmd.ExecuteReader(); 
            
            if (Reader.HasRows) 
            { 
                while (Reader.Read()) 
                { 
                    long imgData = Reader.GetBytes(0, 0, null, 0, int.MaxValue); 
                    byte[] imageArray = new byte[imgData]; 
                    Reader.GetBytes(0, 0, imageArray, 0, Convert.ToInt32(imgData)); 
                    
                    //Convert to Image 
                    TypeConverter bmpConverter = TypeDescriptor.GetConverter(typeof(Bitmap)); 
                    Bitmap imageReceived = (Bitmap)bmpConverter.ConvertFrom(imageArray);
                    
                    //Now Generate the Image Tag for Mvc Html String 
                    imageReceived.Save(HostingEnvironment.MapPath("~/Images")+@"\I" + eventID.ToString() + ".jpg"); 
                    imageData = new TagBuilder("img"); 
                    
                    //Set the Image Url for <img> tag as <img src=""> 
                    imageData.MergeAttribute("src", imgUrl.Content("~/Images") + @"/I" + eventID.ToString() + ".jpg");  
                    imageData.Attributes.Add("height", "50"); 
                    imageData.Attributes.Add("width", "50");
                } 
            } 
            Reader.Close(); 
            Conn.Close(); 
            Cmd.Dispose(); 
            Conn.Dispose(); 
            //The <img> tag will have auto closing as <img src="<Image Path>"
            // height="50" width="50" /> 
            return MvcHtmlString.Create(imageData.ToString(TagRenderMode.SelfClosing));  
        } 
    } 
} 
