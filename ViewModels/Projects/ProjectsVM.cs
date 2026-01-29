namespace HsoPkipt.ViewModels.Projects;

public class ProjectsVM
{
    public IEnumerable<ProjectItemVm> ProjectItems { get; set; } = Enumerable.Empty<ProjectItemVm>();

    public int PageNumber { get; set; }
    public int TotalPages { get; set; }

    public bool HasNextPage => PageNumber < TotalPages;
}
