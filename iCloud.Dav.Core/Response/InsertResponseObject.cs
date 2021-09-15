namespace iCloud.Dav.Core.Response
{
    public class InsertResponseObject : SuccessfulResponseObject
    {
        public InsertResponseObject() : base()
        {
        }

        public override string Message => "Successful insert.";
    }
}
