CREATE TABLE Job.[JobStatus]
(
	Id bigint identity(1,1),
	JobId uniqueidentifier not null,
	JobStatusId smallint not null,
    [StatusTimeStamp] DATETIMEOFFSET NOT NULL default (SYSUTCDATETIME()),
	[Db_StatusTimeStamp] DATETIMEOFFSET NOT NULL default (SYSUTCDATETIME()),
    [_SystemRecordStartDateTime] DATETIME2(7) GENERATED ALWAYS AS ROW START NOT NULL CONSTRAINT DF_JobStatus__SystemRecordStartDateTime DEFAULT(SYSUTCDATETIME()),
	[_SystemRecordEndDateTime] DATETIME2(7) GENERATED ALWAYS AS ROW END NOT NULL CONSTRAINT DF_JobStatus__SystemRecordEndDateTime DEFAULT('9999-12-31 23:59:59.9999999'),

	PRIMARY KEY CLUSTERED (JobId) WITH (DATA_COMPRESSION = PAGE) ON [JobData],
	PERIOD FOR SYSTEM_TIME ([_SystemRecordStartDateTime], [_SystemRecordEndDateTime])
)
WITH
(
	SYSTEM_VERSIONING = ON ( HISTORY_TABLE = [History].[Job_JobStatus] )
);
GO

CREATE INDEX [NCIX_Job_JobStatus] 
ON Job.[JobStatus] (JobId, JobStatusId, StatusTimeStamp, [Db_StatusTimeStamp]) 
WITH (DATA_COMPRESSION = PAGE) ON [JobIndex];
GO
