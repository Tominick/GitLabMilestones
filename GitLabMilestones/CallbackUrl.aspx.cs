using System;
using System.Configuration;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

public partial class CallbackUrl : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            var code = Request.QueryString["code"];
            if (code == null)
            {
                Response.Write("Invalid Code");
                return;
            }
            var state = Request.QueryString["state"];
            if (state != Page.Session["State"].ToString())
            {
                Response.Write("Invalid State");
                return;
            }
            var clientId = ConfigurationManager.AppSettings["ApplicationID"];
            var appSecret = ConfigurationManager.AppSettings["Secret"];
            var redirectUri = ConfigurationManager.AppSettings["CallbackUrl"];
            var request = ApiWebRequestFactory.Create(string.Format(ConfigurationManager.AppSettings["BaseUrl"] + ConfigurationManager.AppSettings["TockenUrl"], clientId, appSecret, code, redirectUri));
            request.Method = "POST";
            var webResp = (HttpWebResponse)request.GetResponse();

            var answer = webResp.GetResponseStream();
            if (answer == null) return;
            var streamReader = new StreamReader(answer);
            var sAnswer = streamReader.ReadToEnd();
            //Response.Write(sAnswer);

            dynamic jObject = JObject.Parse(sAnswer);
            Page.Session["access_token"] = jObject.access_token;

            Response.Redirect("~/Default.aspx");
        }
    }
}