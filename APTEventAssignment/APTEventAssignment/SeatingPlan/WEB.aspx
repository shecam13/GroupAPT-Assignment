<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WEB.aspx.cs" Inherits="APTEventAssignment.SeatingPlan.WEB" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>


<style type="text/css">

.button100 {
    border: 1px solid #808080;
    }

.buttonSize150 {
    border: 1px solid #808080;
    width: 150px;
    }

.seatingAvailable{
    font-family: Verdana, Arial, 'Times New Roman';
    color: #303030;
    font-size: 8px;
    text-align: center;
    vertical-align: middle;
    background-color : #9BCF46; 
    text-decoration : none;
}

.seatingBooked{
    font-family: Verdana, Arial, 'Times New Roman';
    color: #C0C0C0;
    font-size: 8px;
    text-align: center;
    vertical-align: middle;
    background-color : #971C1C;
    text-decoration: line-through;  
}

.seatingZoneTitle {
    font-family: Verdana, Arial, 'Times New Roman';
    color: #FCFCFC;
    font-size: 12px;
    text-align: center; 
    background-color: #303030;
}

.seatingPlan {
    float: left;
}

.seatingSelected {
    border: 1px solid #808080;   
    float: right; 
}

</style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Button ID="btnVenueA" runat="server" Text="Venue A" CssClass="buttonSize150" OnClick="btnVenueA_Click" />
        <asp:Button ID="btnVenueB" runat="server" Text="Venue B" CssClass="buttonSize150" OnClick="btnVenueB_Click"/>
        <asp:Button ID="btnVenueC" runat="server" Text="Venue C" CssClass="buttonSize150" OnClick="btnVenueC_Click"/>
        <asp:Button ID="btnVenueD" runat="server" Text="Venue D" CssClass="buttonSize150" OnClick="btnVenueD_Click"/>
    <div>
        <table width="100%">
            <tr>
                <td width="85%" style="vertical-align: top;">
                       <asp:Panel ID="pnlSeating" runat="server" Width="100%" CssClass="seatingPlan" ></asp:Panel>
                </td>
                <td style="vertical-align: top;">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:ListBox ID="lstSeatingSelected" runat="server" Rows="15" Width="100%" CssClass="seatingSelected" ondblclick="lstSeatingSelected_DoubleClick()"></asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnRemoveSelected" runat="server" Text="Remove Selected" Width="100%" CssClass="button100" OnClick="btnRemoveSelected_Click" />
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnRemoveAll" runat="server" Text="Remove All" Width="100%" CssClass="button100" OnClick="btnRemoveAll_Click" />
                        </tr>
                    </table>
                    
                </td>
            </tr>

        </table>
         

    </div>

    </form>
</body>
</html>
