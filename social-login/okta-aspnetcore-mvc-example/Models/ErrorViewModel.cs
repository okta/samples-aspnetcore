#pragma warning disable SA1300 // Element should begin with upper-case letter
namespace okta_aspnetcore_mvc_example.Models
#pragma warning restore SA1300 // Element should begin with upper-case letter
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
