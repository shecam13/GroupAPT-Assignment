using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace APTEventAssignment.Models
{
    //public class ModelClasses
    //{
    //}

    public class ImageTable 
    {
        [Key]
        public int Event_ID { get; set; } 
        public byte[] Event_Image { get; set; } 
    }
    public class DataAcceess 
    { 
        public ImageTable[] GetImages() 
        { 
            ImageTable[] Images = null; 
            SqlConnection Conn = new SqlConnection("Data Source= localhost\\SQLEXPRESS;initial catalog=APTEvents;user id=csi2015users;password=csi2015p@$$!;MultipleActiveResultSets=True;App=EntityFramework"); 
            Conn.Open(); 
            SqlCommand Cmd = new SqlCommand("Select Event_Image From Event", Conn); 
            SqlDataReader Reader = Cmd.ExecuteReader(); 
            DataTable dt = new DataTable(); 
            dt.Load(Reader); 
            Images = new ImageTable[dt.Rows.Count]; 
            int i = 0; 
            foreach (DataRow Dr in dt.Rows) 
            { 
                Images[i] = new ImageTable() 
                {
                    Event_ID = Convert.ToInt32(Dr["ImageId"]),
                    Event_Image = (byte[])Dr["Image"] 
                }; 
                i = i + 1; 
            } 
            Conn.Close(); 
            return Images; 
        } 
    } 

}