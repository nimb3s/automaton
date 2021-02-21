CREATE PROCEDURE Job.p_GetJobStatusDetailsByJobId
 @JobId uniqueidentifier
AS
begin
	set nocount on;

	SELECT j.Id, JobId, j.JobName, JobStatusTypeId 
	FROM Job.Job j
	INNER JOIN Job.JobStatus js
	ON j.Id = js.JobId
	INNER JOIN Job.JobStatusType jst
	ON js.JobStatusTypeId = jst.Id
	WHERE j.Id = @JobId
end