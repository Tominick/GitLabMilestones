using System;
using System.Collections.Generic;

public class Namespace
{
    public int id { get; set; }
    public string name { get; set; }
    public string path { get; set; }
    public string kind { get; set; }
    public string full_path { get; set; }
    public int? parent_id { get; set; }
    public string avatar_url { get; set; }
    public string web_url { get; set; }
}

public class Project
{
    public int id { get; set; }
    public string description { get; set; }
    public string name { get; set; }
    public string name_with_namespace { get; set; }
    public string path { get; set; }
    public string path_with_namespace { get; set; }
    public DateTime created_at { get; set; }
    public string default_branch { get; set; }
    public IList<object> tag_list { get; set; }
    public string ssh_url_to_repo { get; set; }
    public string http_url_to_repo { get; set; }
    public string web_url { get; set; }
    public string readme_url { get; set; }
    public string avatar_url { get; set; }
    public int star_count { get; set; }
    public int forks_count { get; set; }
    public DateTime last_activity_at { get; set; }
    public Namespace Namespace { get; set; }
}

public class Milestone
{
    public int id { get; set; }
    public int iid { get; set; }
    public int project_id { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public string state { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public object due_date { get; set; }
    public string start_date { get; set; }
    public string web_url { get; set; }
}

public class ClosedBy
{
    public int id { get; set; }
    public string name { get; set; }
    public string username { get; set; }
    public string state { get; set; }
    public string avatar_url { get; set; }
    public string web_url { get; set; }
}


public class Author
{
    public int id { get; set; }
    public string name { get; set; }
    public string username { get; set; }
    public string state { get; set; }
    public string avatar_url { get; set; }
    public string web_url { get; set; }
}

public class Assignee
{
    public int id { get; set; }
    public string name { get; set; }
    public string username { get; set; }
    public string state { get; set; }
    public string avatar_url { get; set; }
    public string web_url { get; set; }
}

public class TimeStats
{
    public int time_estimate { get; set; }
    public int total_time_spent { get; set; }
    public object human_time_estimate { get; set; }
    public object human_total_time_spent { get; set; }
}

public class TaskCompletionStatus
{
    public int count { get; set; }
    public int completed_count { get; set; }
}

public class Issue
{
    public int id { get; set; }
    public int iid { get; set; }
    public int project_id { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public string state { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public DateTime closed_at { get; set; }
    public ClosedBy closed_by { get; set; }
    public IList<string> labels { get; set; }
    public Milestone milestone { get; set; }
    public IList<Assignee> assignees { get; set; }
    public Author author { get; set; }
    public Assignee assignee { get; set; }
    public int user_notes_count { get; set; }
    public int merge_requests_count { get; set; }
    public int upvotes { get; set; }
    public int downvotes { get; set; }
    public object due_date { get; set; }
    public bool confidential { get; set; }
    public object discussion_locked { get; set; }
    public string web_url { get; set; }
    public TimeStats time_stats { get; set; }
    public TaskCompletionStatus task_completion_status { get; set; }
    public string CsLabels
    {
        get { return string.Join(",", labels); }
    }
}
