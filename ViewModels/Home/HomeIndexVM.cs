using HsoPkipt.ViewModels.News;
using HsoPkipt.ViewModels.Projects;

namespace HsoPkipt.ViewModels.Home;

public class HomeIndexVM
{
  public IEnumerable<NewsItemVM> LatestNews { get; set; } = Enumerable.Empty<NewsItemVM>();
  public IEnumerable<ProjectItemVm> LatestProjects { get; set; } = Enumerable.Empty<ProjectItemVm>();
}