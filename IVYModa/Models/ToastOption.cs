namespace IVYModa.Models
{
    public class ToastOption
    {
        public string Status { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;

        public class StatusToast
        {
            public const string Success = "success";
            public const string Error = "error";
            public const string Warning = "warning";
            public const string Info = "info";
            public const string Question = "question";
        }
    }
}
