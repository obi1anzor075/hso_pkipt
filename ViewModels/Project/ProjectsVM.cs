using HsoPkipt.ViewModels.Project;

namespace HsoPkipt.ViewModels.Project;

public class ProjectsVM
{
    public IEnumerable<ProjectItemVM> ProjectItems { get; set; } = Enumerable.Empty<ProjectItemVM>();

    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
}
