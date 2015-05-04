using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace APTEventAssignment.SeatingPlan
{
	public partial class WEB : System.Web.UI.Page
	{
        public enum Seating
        {
            Code = 0,
            Title = 1,
            Rows = 2,
            Columns = 3
        }


        int intZoneCount;
        string[,] arrSeating = new string[1, 6];

        string[,] arrBookedSeats = new string[1, 3];


        private bool seatBookedCheck(string ZoneSeat)
        {
            //Try and locate the seat
            if (lstSeatingSelected.Items.FindByText(ZoneSeat) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private object padCode(string Code)
        {
            string paddedCode = "";
            if (Code.Length < 3)
            {
                for (int i = 1; i <= 3 - Code.Length; i++)
                {
                    paddedCode = paddedCode + "0";
                }

                return paddedCode + Code;
            }
            else
            {
                return Code;
            }
        }


        private void DrawSeatingMap(Array Zones)
        {
            int intColumnCount = 0;
            int intSeatingWidth = 0;
            int intZonePaddingWidth = 5;
            int intSeatWidth = 24;


            if (Zones.GetType().IsArray)
            {

                //Create Table with Zones
                Table tblZones = new Table();
                tblZones.Width = Unit.Percentage(100);
                tblZones.ID = "tblZones";
                TableRow trZones = new TableRow();

                //Calculate Seating Width
                for (int i = 0; i <= Zones.GetUpperBound(0); i++)
                {
                    intColumnCount = intColumnCount + int.Parse(Zones.GetValue(i, (int)Seating.Columns).ToString());

                }

                double d = (100 - (intZonePaddingWidth * (Zones.GetUpperBound(0) + 1)));
                intSeatingWidth = (int)Math.Floor(d / intColumnCount);



                for (int i = 0; i <= Zones.GetUpperBound(0); i++)
                {
                    TableCell tdZones = new TableCell();
                    int w = int.Parse(Zones.GetValue(i, (int)Seating.Columns).ToString()) * intSeatingWidth;
                    tdZones.Width = Unit.Percentage(w);
                    tdZones.BorderStyle = BorderStyle.Solid;
                    tdZones.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                    tdZones.BorderWidth = Unit.Pixel(1);

                    Table tblZone = new Table();
                    tblZone.Width = Unit.Percentage(100);

                    for (int Rows = 0; Rows <= int.Parse(Zones.GetValue(i, (int)Seating.Rows).ToString()) - 1; Rows++)
                    {
                        TableRow trZoneRow = new TableRow();
                        trZoneRow.Width = Unit.Percentage(100);
                        trZoneRow.BorderWidth = Unit.Pixel(0);

                        for (int cols = 0; cols <= int.Parse(Zones.GetValue(i, (int)Seating.Columns).ToString()) - 1; cols++)
                        {
                            TableCell tdZoneSeat = new TableCell();
                            tdZoneSeat.Width = Unit.Percentage(intSeatingWidth);
                            tdZoneSeat.Height = Unit.Pixel(32);

                            tdZoneSeat.Style.Add("text-align", "center");
                            tdZoneSeat.HorizontalAlign = HorizontalAlign.Center;

                            LinkButton thisZoneSeat = new LinkButton();
                            thisZoneSeat.Width = Unit.Pixel(intSeatWidth);
                            thisZoneSeat.Height = Unit.Pixel(intSeatWidth);
                            thisZoneSeat.BorderStyle = BorderStyle.Solid;
                            thisZoneSeat.BorderWidth = Unit.Pixel(1);
                            thisZoneSeat.BorderColor = System.Drawing.ColorTranslator.FromHtml("#CECECE");

                            string thisSeatCode = "";

                            thisSeatCode = padCode(((Rows * int.Parse(Zones.GetValue(i, (int)Seating.Columns).ToString()) + cols)).ToString()).ToString();
                            thisSeatCode = Zones.GetValue(i, (int)Seating.Code) + thisSeatCode;

                            switch (seatBookedCheck(thisSeatCode))
                            {
                                case false:
                                    thisZoneSeat.CssClass = "seatingAvailable";
                                    thisZoneSeat.Text = thisSeatCode;
                                    thisZoneSeat.ID = thisSeatCode;
                                    thisZoneSeat.Click += seatClick;
                                    break;
                                case true:
                                    thisZoneSeat.CssClass = "seatingBooked";
                                    thisZoneSeat.Text = thisSeatCode;
                                    thisZoneSeat.Enabled = false;
                                    break;
                            }

                            tdZoneSeat.Controls.Add(thisZoneSeat);
                            trZoneRow.Controls.Add(tdZoneSeat);
                        }
                        tblZone.Controls.Add(trZoneRow);
                        tdZones.Controls.Add(tblZone);
                    }

                    trZones.Controls.Add(tdZones);
                    if (i != Zones.GetUpperBound(1))
                    {
                        TableCell tdZonePad = new TableCell();
                        tdZonePad.Width = Unit.Percentage(1);
                        trZones.Controls.Add(tdZonePad);
                    }
                }

                tblZones.Rows.Add(trZones);

                TableRow trZoneTitles = new TableRow();

                for (int i = 0; i <= Zones.GetUpperBound(0); i++)
                {
                    TableCell tdZoneTitle = new TableCell();
                    Label lblZoneTitle = new Label();
                    lblZoneTitle.Text = Zones.GetValue(i, (int)Seating.Title).ToString();
                    tdZoneTitle.Controls.Add(lblZoneTitle);
                    tdZoneTitle.CssClass = "seatingZoneTitle";

                    trZoneTitles.Controls.Add(tdZoneTitle);
                    if (i != Zones.GetUpperBound(1))
                    {
                        TableCell tdZonePad = new TableCell();
                        tdZonePad.Width = Unit.Percentage(intZonePaddingWidth);
                        trZoneTitles.Controls.Add(tdZonePad);
                    }
                }
                tblZones.Controls.Add(trZoneTitles);

                pnlSeating.Controls.Add(tblZones);
                Session["pnlSeating"] = tblZones;
            }
        }

        int row, col;

        public void setRow(int row)
        {
            this.row = row;
        }

        protected override void OnPreInit(EventArgs e)
        {
            switch (Request.QueryString["V"])
            {
                case "A":
                    //Three Zones
                    intZoneCount = 3;
                    arrSeating = new string[intZoneCount, 6];

                    arrSeating[0, (int)Seating.Code] = "L";
                    arrSeating[0, (int)Seating.Title] = "LEFT WING";
                    arrSeating[0, (int)Seating.Rows] = "20";
                    arrSeating[0, (int)Seating.Columns] = "10";

                    arrSeating[1, (int)Seating.Code] = "M";
                    arrSeating[1, (int)Seating.Title] = "MIDDLE";
                    arrSeating[1, (int)Seating.Rows] = "14";
                    arrSeating[1, (int)Seating.Columns] = "12";

                    arrSeating[2, (int)Seating.Code] = "R";
                    arrSeating[2, (int)Seating.Title] = "RIGHT WING";
                    arrSeating[2, (int)Seating.Rows] = "14";
                    arrSeating[2, (int)Seating.Columns] = "6";

                    DrawSeatingMap(arrSeating);
                    break;
                case "B":
                    //Two Zones
                    intZoneCount = 2;
                    arrSeating = new string[intZoneCount, 6];

                    arrSeating[0, (int)Seating.Code] = "L";
                    arrSeating[0, (int)Seating.Title] = "LEFT WING";
                    arrSeating[0, (int)Seating.Rows] = "5";
                    arrSeating[0, (int)Seating.Columns] = "10";

                    arrSeating[1, (int)Seating.Code] = "R";
                    arrSeating[1, (int)Seating.Title] = "RIGHT WING";
                    arrSeating[1, (int)Seating.Rows] = "5";
                    arrSeating[1, (int)Seating.Columns] = "10";

                    DrawSeatingMap(arrSeating);

                    break;
                case "C":
                    //One Zones
                    intZoneCount = 1;
                    arrSeating = new string[intZoneCount, 6];

                    arrSeating[0, (int)Seating.Code] = "M";
                    arrSeating[0, (int)Seating.Title] = "MAIN";
                    arrSeating[0, (int)Seating.Rows] = "8";
                    arrSeating[0, (int)Seating.Columns] = "15";

                    DrawSeatingMap(arrSeating);

                    break;
                case "D":
                    //Four Zones
                    intZoneCount = 4;
                    arrSeating = new string[intZoneCount, 6];


                    arrSeating[0, (int)Seating.Code] = "L";
                    arrSeating[0, (int)Seating.Title] = "LEFT WING";
                    arrSeating[0, (int)Seating.Rows] = "14";
                    arrSeating[0, (int)Seating.Columns] = "6";

                    arrSeating[1, (int)Seating.Code] = "M1";
                    arrSeating[1, (int)Seating.Title] = "MIDDLE1";
                    arrSeating[1, (int)Seating.Rows] = "14";
                    arrSeating[1, (int)Seating.Columns] = "4";

                    arrSeating[2, (int)Seating.Code] = "M2";
                    arrSeating[2, (int)Seating.Title] = "MIDDLE2";
                    arrSeating[2, (int)Seating.Rows] = "14";
                    arrSeating[2, (int)Seating.Columns] = "4";

                    arrSeating[3, (int)Seating.Code] = "R";
                    arrSeating[3, (int)Seating.Title] = "RIGHT WING";
                    arrSeating[3, (int)Seating.Rows] = "14";
                    arrSeating[3, (int)Seating.Columns] = "6";

                    DrawSeatingMap(arrSeating);
                    break;
            }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (Page.IsPostBack)
            {
                //Mark Booked Seats
                for (int i = 0; i <= lstSeatingSelected.Items.Count - 1; i++)
                {
                    LinkButton thisSeat = new LinkButton();
                    thisSeat = (LinkButton)FindControl(lstSeatingSelected.Items[i].Text);
                    thisSeat.CssClass = "seatingBooked";
                    thisSeat.Enabled = false;
                }
            }
        }

        private void seatClick(object sender, EventArgs e)
        {
            LinkButton theSelectedSeat = new LinkButton();

            theSelectedSeat = (LinkButton)sender;
            lstSeatingSelected.Items.Add(theSelectedSeat.ID);
            theSelectedSeat.CssClass = "seatingBooked";
            theSelectedSeat.Enabled = false;
        }



        private LinkButton findSeat(string seat)
        {
            Table tblZones = new Table();
            LinkButton thisSeat = new LinkButton();

            //Find the containing Table 
            tblZones = (Table)pnlSeating.FindControl("tblZones");
            //Find the Seat
            thisSeat = (LinkButton)FindControlRecursively(tblZones, seat);

            return thisSeat;
        }


        public System.Web.UI.Control FindControlRecursively(System.Web.UI.Control parentControl, string controlID)
        {
            //If the control sought for is the parent then return
            if (parentControl.ID == controlID)
            {
                return parentControl;
            }

            //Loop through the controls to check if the control is the one being sought
            foreach (System.Web.UI.Control c in parentControl.Controls)
            {
                System.Web.UI.Control child = FindControlRecursively(c, controlID);
                if (child != null)
                {
                    return child;
                }
            }
            return null;
        }




        protected void btnRemoveAll_Click(object sender, EventArgs e)
        {
            //Set the Seating Plan Status
            for (int i = 0; i <= lstSeatingSelected.Items.Count - 1; i++)
            {
                LinkButton theSelectedSeat = new LinkButton();
                theSelectedSeat = findSeat(lstSeatingSelected.Items[i].Text);
                theSelectedSeat.CssClass = "seatingAvailable";
                theSelectedSeat.Enabled = true;
            }
            //Clear the list
            lstSeatingSelected.Items.Clear();
        }


        protected void btnVenueA_Click(object sender, EventArgs e)
        {
            Response.Redirect("web.aspx?V=A");
        }

        protected void btnVenueB_Click(object sender, EventArgs e)
        {
            Response.Redirect("web.aspx?V=B");
        }

        protected void btnVenueC_Click(object sender, EventArgs e)
        {
            Response.Redirect("web.aspx?V=C");
        }

        protected void btnVenueD_Click(object sender, EventArgs e)
        {
            Response.Redirect("web.aspx?V=D");
        }

        protected void btnRemoveSelected_Click(object sender, EventArgs e)
        {
            LinkButton theSelectedSeat = new LinkButton();
            //Set the Seating Plan Status
            if (lstSeatingSelected.SelectedIndex > -1)
            {
                theSelectedSeat = findSeat(lstSeatingSelected.Items[lstSeatingSelected.SelectedIndex].Value);
                if (theSelectedSeat.ID == lstSeatingSelected.Items[lstSeatingSelected.SelectedIndex].Value)
                {
                    theSelectedSeat.CssClass = "seatingAvailable";
                    theSelectedSeat.Enabled = true;
                    //Remove from the list
                    lstSeatingSelected.Items.RemoveAt(lstSeatingSelected.SelectedIndex);
                    lstSeatingSelected.SelectedIndex = -1;
                }
            }
        }

        
        public String[] getSeats()
        {
            String[] seatsArray = null;

            for (int i = 0; i <= lstSeatingSelected.Items.Count - 1; i++)
            {
                i++;
            }

            return seatsArray;
        }
	}
}