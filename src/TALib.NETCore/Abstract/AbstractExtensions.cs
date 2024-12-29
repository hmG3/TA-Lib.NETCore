using System.Linq;
using System.Text;

namespace TALib;

public static class AbstractExtensions
{
    public static string ToFormattedGroupList(this Abstract a)
    {
        var maxNameLength = a.Select(f => f.Name.Length).Max();
        var sb = new StringBuilder();
        foreach (var groups in Abstract.All.GroupBy(f => f.Group))
        {
            var divider = new string('—', groups.Key.Length + 1);
            sb.AppendLine(divider).AppendLine(groups.Key).AppendLine(divider);
            foreach (var f in groups)
            {
                sb.Append(f.Name.PadRight(maxNameLength + 3));
                sb.AppendLine(f.Description);
            }
        }

        return sb.ToString();
    }
}