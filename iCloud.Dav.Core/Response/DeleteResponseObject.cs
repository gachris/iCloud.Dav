namespace iCloud.Dav.Core.Response;

public class DeleteResponseObject : SuccessfulResponseObject
{
    public DeleteResponseObject() : base()
    {
    }

    public override string Message => "Successful delete.";
}
