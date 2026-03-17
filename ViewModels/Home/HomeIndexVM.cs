using HsoPkipt.ViewModels.News;
using HsoPkipt.ViewModels.Project;

namespace HsoPkipt.ViewModels.Home;

public class HomeIndexVM
{
  public IEnumerable<NewsItemVM> LatestNews { get; set; } = Enumerable.Empty<NewsItemVM>();
  public IEnumerable<ProjectItemVM> LatestProjects { get; set; } = Enumerable.Empty<ProjectItemVM>();
}