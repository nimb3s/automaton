namespace Nimb3s.Automaton.Core.Repositories.Sql
{
    public interface IAutomatonDatabaseContext
    {
        HttpRequestRepository HttpRequestRepository { get; }
        HttpRequestStatusRepository HttpRequestStatusRepository { get; }
        HttpResponseRepository HttpResponseRepository { get; }
        JobRepository JobRepository { get; }
        JobStatusRepository JobStatusRepository { get; }
        WorkItemRepository WorkItemRepository { get; }
        WorkItemStatusRepository WorkItemStatusRepository { get; }
    }
}