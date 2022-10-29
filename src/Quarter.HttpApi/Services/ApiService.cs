using Quarter.Core.Commands;
using Quarter.Core.Models;
using Quarter.Core.Repositories;
using Quarter.Core.Utils;
using Quarter.HttpApi.Resources;

namespace Quarter.HttpApi.Services;

public interface IApiService
{
    IAsyncEnumerable<ProjectResourceOutput> ProjectsForUserAsync(OperationContext oc, CancellationToken ct);
    Task CreateProjectAsync(ProjectResourceInput input, OperationContext oc, CancellationToken ct);
    Task DeleteProjectAsync(IdOf<Project> projectId, OperationContext oc, CancellationToken ct);
}

public class ApiService : IApiService
{
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly ICommandHandler _commandHandler;

    public ApiService(IRepositoryFactory repositoryFactory, ICommandHandler commandHandler)
    {
        _repositoryFactory = repositoryFactory;
        _commandHandler = commandHandler;
    }

    public IAsyncEnumerable<ProjectResourceOutput> ProjectsForUserAsync(OperationContext oc, CancellationToken ct)
    {
        var projectRepository = _repositoryFactory.ProjectRepository(oc.UserId);
        return projectRepository.GetAllAsync(ct).Select(ProjectResourceOutput.From);
    }

    public Task CreateProjectAsync(ProjectResourceInput input, OperationContext oc, CancellationToken ct)
    {
        // name and description are validated at controller
        var command = new AddProjectCommand(input.name!, input.description!);
        return _commandHandler.ExecuteAsync(command, oc, ct);
    }

    public Task DeleteProjectAsync(IdOf<Project> projectId, OperationContext oc, CancellationToken ct)
        => _commandHandler.ExecuteAsync(new RemoveProjectCommand(projectId), oc, ct);
}