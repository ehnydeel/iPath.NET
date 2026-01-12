namespace iPath.Application.Features.Nodes;

public static class NodeExtensions
{
    extension (NodeDescription Data)
    {

        public bool ValidateInput()
        {
            // if any of the main fields are filled, consider it valid
            if (!string.IsNullOrWhiteSpace(Data.Text)) return true;
            if (!string.IsNullOrWhiteSpace(Data.Title)) return true;
            if (!string.IsNullOrWhiteSpace(Data.AccessionNo)) return true;
            if (Data.Questionnaire is not null) return true;
            return false;
        }
    }
}