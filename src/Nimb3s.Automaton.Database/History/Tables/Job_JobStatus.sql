﻿CREATE TABLE [History].[Job_JobStatus]
(
	JobId uniqueidentifier not null,
	JobStatusId smallint not null,
    [StatusTimeStamp] DATETIMEOFFSET NOT NULL,
	[Db_StatusTimeStamp] DATETIMEOFFSET NOT NULL default(SYSUTCDATETIME()), 
    [_SystemRecordStartDateTime] DATETIME2(7) not null,
	[_SystemRecordEndDateTime] DATETIME2(7) not null
);
GO

CREATE INDEX [NCIX_Job_JobStatus] 
ON [History].[Job_JobStatus] ([_SystemRecordStartDateTime], [_SystemRecordEndDateTime], JobId, JobStatusId, StatusTimeStamp,[Db_StatusTimeStamp]) 
WITH (DATA_COMPRESSION = PAGE) ON [JobIndex];
GO
