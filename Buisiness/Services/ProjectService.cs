
using Buisiness.Models;
using Data.Entities;
using Data.Repositories;
using Domain.Extensions;
using Domain.Models;

namespace Buisiness.Services;

public interface IProjectService
{
    Task<ProjectResult> CreateProjectAsync(AddProjectFormData formData);
    Task<ProjectResult<IEnumerable<Project>>> CreateProjectsAsync();
    Task<ProjectResult<Project>> CreateProjectsAsync(string id);
    Task<object> GetAllAsync();
}

public class ProjectService(IProjectRepository projectRepository, IStatusService statusService) //: IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IStatusService _statusService = statusService;

    public async Task<ProjectResult> CreateProjectAsync(AddProjectFormData formData)
    {
        if (formData == null)
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Form data is null." };

        var projectEntity = formData.MapTo<ProjectEntity>();
        var statusResult = await _statusService.GetStatusByIdAsync(1);
        var status = statusResult.Result;
        projectEntity.StatusId = status!.Id;


        var result = await _projectRepository.AddAsync(projectEntity);
        return result.Succeeded
            ? new ProjectResult { Succeeded = true, StatusCode = 201 }
            : new ProjectResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }


    public async Task<ProjectResult<IEnumerable<Project>>> CreateProjectsAsync()
    {
        var response = await _projectRepository.GetAllAsync
            (
                orderByDescending: true,
                sortBy: s => s.Created,
                where: null,
                x => x.User,
                x => x.Status,
                x => x.Client
            );
        return new ProjectResult<IEnumerable<Project>> { Succeeded = true, StatusCode = 201, Result = response.Result };
    }
    public async Task<ProjectResult<Project>> CreateProjectsAsync(string id)
    {
        var response = await _projectRepository.GetAsync
            (where: x => x.Id == id,
                include => include.User,
                include => include.Status,
                include => include.Client
            );
        return response.Succeeded
            ? new ProjectResult<Project> { Succeeded = true, StatusCode = 200, Result = response.Result }
            : new ProjectResult<Project> { Succeeded = false, StatusCode = 404, Error = $"Project '{id}' was not found." };
    }
}
