namespace WDC
{
    class AjRequestNext:AjRequestBase
    {
        public string TemplateId { get; set; }
        public string Url { get; set; }

        public string PostData { get; set; }

        public bool IsPostRequest = false;

    }
    class AjRequestBase{

        public PageInfo Page { get; set; }
    }
    class PageInfo
    {
        string Url { get; set; }
    }
}
