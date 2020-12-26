create procedure job.p_InsertJob
 @Id uniqueidentifier,
 @JobName varchar(10),
 @JobStatusId smallint
as
begin
set nocount off
	insert into job.Job values(@Id, @JobName, @JobStatusId)
end