<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
</head>
<body>
    <form id="form1" runat="server">
    <asp:Label ID="lblError" runat="server" ForeColor="Red" />
    <asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" Text="GitLab Sign in" />
    <asp:Panel ID="pnlMain" runat="server" Visible="False">
        <asp:Repeater ID="DataList" runat="server" DataSourceID="ObjectDataSource2" OnItemDataBound="DataList_ItemDataBound">
            <ItemTemplate>
                <table border="0" width="970px" cellspacing="0">
                    <tr>
                        <td style="background-color: #507CD1; color: white; font-weight: bold; font-size: 20px;"
                            height="50px">
                            <%= Session["projectName"]%>
                            <%# DataBinder.Eval(Container.DataItem, "title")%>
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("id", "{0}") %>' Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color: #EFF3FB">
                            <%# DataBinder.Eval(Container.DataItem, "description")%>
                        </td>
                    </tr>
                </table>
                <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="Select"
                    TypeName="IssuesFactory" OnSelecting="ObjectDataSource1_Selecting">
                    <SelectParameters>
                        <asp:Parameter Name="accessToken" Type="String" />
                        <asp:ControlParameter ControlID="Label1" Name="milestoneId" PropertyName="Text" DefaultValue="999"
                            Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    DataSourceID="ObjectDataSource1" ForeColor="#333333" GridLines="None" Width="970px">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="title" HeaderText="Title" SortExpression="title" />
                        <%--<asp:BoundField DataField="state" HeaderText="State" SortExpression="state" />--%>
                        <asp:BoundField DataField="CsLabels" HeaderText="Labels" />
                        <asp:HyperLinkField HeaderText="ID" DataNavigateUrlFields="web_url" DataTextField="iid" />
                    </Columns>
                    <EditRowStyle BackColor="#2461BF" />
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#EFF3FB" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                </asp:GridView>
                <br />
            </ItemTemplate>
            <FooterTemplate>
                <asp:Label ID="lblEmptyData" Text="No Milestones found" runat="server" Visible="false" />
            </FooterTemplate>
        </asp:Repeater>
        <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SelectMethod="Select"
            TypeName="MilestonesFactory" OnSelecting="ObjectDataSource2_Selecting">
            <SelectParameters>
                <asp:Parameter Name="accessToken" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
    </form>
</body>
</html>
