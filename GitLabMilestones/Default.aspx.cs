using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Page.Session["access_token"] != null)
            {
                btnLogin.Visible = false;
                pnlMain.Visible = true;

                var p = ApiQuery(string.Format(ConfigurationManager.AppSettings["BaseUrl"] + ConfigurationManager.AppSettings["ProjectUrl"], ConfigurationManager.AppSettings["SelectedProject"]));
                if (p == string.Empty) return;
                var project = JsonConvert.DeserializeObject<Project>(p);
                Page.Session["projectName"] = project.name;
                Page.Title = project.name + " " + project.description;
            }
        }
    }

    private string ApiQuery(string url)
    {
        try
        {
            lblError.Text = string.Empty;

            var request = ApiWebRequestFactory.Create(url + (url.Contains("?") ? "&" : "?") + "access_token=" + Page.Session["access_token"]);
            var webResp = (HttpWebResponse)request.GetResponse();

            var answer = webResp.GetResponseStream();
            if (answer == null) return null;
            var streamReader = new StreamReader(answer);
            return streamReader.ReadToEnd();
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            return string.Empty;
        }        
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        var state = Guid.NewGuid();
        Page.Session["State"] = state.ToString();
        Response.Redirect(
            string.Format(
                ConfigurationManager.AppSettings["BaseUrl"] + ConfigurationManager.AppSettings["AuthorizeUrl"],
                ConfigurationManager.AppSettings["ApplicationID"], ConfigurationManager.AppSettings["CallbackUrl"], state));
    }


    protected void ObjectDataSource1_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["accessToken"] = Page.Session["access_token"].ToString();
    }

    protected void ObjectDataSource2_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["accessToken"] = Page.Session["access_token"].ToString();
    }

    protected void DataList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        //https://www.dotnetcurry.com/ShowArticle.aspx?ID=271
        if (DataList.Items.Count != 0) return;
        if (e.Item.ItemType != ListItemType.Footer) return;
        var lblFooter = e.Item.FindControl("lblEmptyData") as Label;
        if (lblFooter != null) lblFooter.Visible = true;
    }
}