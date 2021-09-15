namespace iCloud.Dav.Core.Response
{
    public class UpdateResponseObject : SuccessfulResponseObject
    {
        public UpdateResponseObject() : base()
        {
        }

        public override string Message => "Successful update.";
    }
}
