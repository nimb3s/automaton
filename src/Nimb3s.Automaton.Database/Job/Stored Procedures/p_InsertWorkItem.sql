CREATE PROCEDURE job.p_InsertWorkItem
 @Id uniqueidentifier,
 @JobId uniqueidentifier,
 @WorkItemStatusId smallint
as
begin
set nocount off
	insert into job.WorkItem values(@Id, @JobId, @WorkItemStatusId)
end