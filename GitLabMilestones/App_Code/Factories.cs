using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static class ApiWebRequestFactory
{
    public static HttpWebRequest Create(string url)
    {
        //https://stackoverflow.com/questions/10822509/the-request-was-aborted-could-not-create-ssl-tls-secure-channel
        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
        return (HttpWebRequest)WebRequest.Create(url);
    }
}

public static class IssuesFactory
{
    public static IEnumerable<Issue> Select(string accessToken, int milestoneId)
    {
        var page = 1;
        var url = string.Format(ConfigurationManager.AppSettings["BaseUrl"] + ConfigurationManager.AppSettings["IssuesUrl"], ConfigurationManager.AppSettings["SelectedProject"], milestoneId);
        var nextPage = string.Empty;
        var issues = new List<Issue>();
        do
        {
            var request = ApiWebRequestFactory.Create(url + (url.Contains("?") ? "&" : "?") + "page=" + page + "&access_token=" + accessToken);
            var webResp = (HttpWebResponse)request.GetResponse();

            var answer = webResp.GetResponseStream();
            if (answer == null) return null;
            string sAnswer;
            using (var streamReader = new StreamReader(answer))
            {
                sAnswer = streamReader.ReadToEnd();
            }

            issues.AddRange(JsonConvert.DeserializeObject<List<Issue>>(sAnswer));
            nextPage = webResp.GetResponseHeader("X-Next-Page");
            if (nextPage != string.Empty)
            {
                page = int.Parse(nextPage);
            }
        } while (nextPage != "");

        return issues.OrderByDescending(x => x.CsLabels);
    }
}

public static class MarkdownFactory
{
    public static string GetHtml(string s)
    {
        var url = ConfigurationManager.AppSettings["BaseUrl"] + ConfigurationManager.AppSettings["MarkdownUrl"];
        //https://gitlab.com/gitlab-org/gitlab-foss/-/issues/45741
        //HACK Two spaces at the end of a line are parsed as <br/>
        var data = "{\"text\":\"" + s.Replace("  \r\n", "<br/>").Replace("\r\n", string.Empty) + "\", \"gfm\":true}";        
        var request = ApiWebRequestFactory.Create(url);

        request.Method = "POST";
        request.ContentType = "application/json";
        var d = System.Text.Encoding.UTF8.GetBytes(data);
        request.ContentLength = d.Length;
        using (var stream = request.GetRequestStream())
        {
            stream.Write(d, 0, d.Length);
        }

        var webResp = (HttpWebResponse)request.GetResponse();

        var answer = webResp.GetResponseStream();
        if (answer == null) return null;
        string sAnswer;
        using (var streamReader = new StreamReader(answer))
        {
            sAnswer = streamReader.ReadToEnd();
        }

        dynamic r = JObject.Parse(sAnswer);
        return Convert.ToString(r.html);
    }
}

public static class MilestonesFactory
{
    public static IEnumerable<Milestone> Select(string accessToken)
    {
        var page = 1;
        var url = string.Format(ConfigurationManager.AppSettings["BaseUrl"] + ConfigurationManager.AppSettings["MilestoneUrl"], ConfigurationManager.AppSettings["SelectedProject"]);
        var nextPage = string.Empty;
        var issues = new List<Milestone>();
        do
        {
            var request = ApiWebRequestFactory.Create(url + (url.Contains("?") ? "&" : "?") + "page=" + page + "&access_token=" + accessToken);
            var webResp = (HttpWebResponse)request.GetResponse();

            var answer = webResp.GetResponseStream();
            if (answer == null) return null;
            string sAnswer;
            using (var streamReader = new StreamReader(answer))
            {
                sAnswer = streamReader.ReadToEnd();
            }

            var newItems = JsonConvert.DeserializeObject<List<Milestone>>(sAnswer);
            foreach (var milestone in newItems)
            {
                milestone.description = !string.IsNullOrEmpty(milestone.description) ? MarkdownFactory.GetHtml(milestone.description) : "&nbsp;";
                
            }
            issues.AddRange(newItems);
            nextPage = webResp.GetResponseHeader("X-Next-Page");
            if (nextPage != string.Empty)
            {
                page = int.Parse(nextPage);
            }
        } while (nextPage != "");

        return issues;
    }
}