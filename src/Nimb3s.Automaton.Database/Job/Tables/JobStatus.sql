create table Job.[JobStatus]
(
	Id bigint identity(1,1),
	JobId uniqueidentifier not null,
	JobStatusTypeId smallint not null,
    [StatusTimeStamp] datetimeoffset not null default (sysutcdatetime()),
	[Db_StatusTimeStamp] datetimeoffset not null default (sysutcdatetime()),
    [_SystemRecordStartDateTime] datetime2(7) generated always as row start not null constraint DF_JobStatus__SystemRecordStartDateTime default(sysutcdatetime()),
	[_SystemRecordEndDateTime] datetime2(7) generated always as row end not null constraint DF_JobStatus__SystemRecordEndDateTime default('9999-12-31 23:59:59.9999999'),

	primary key clustered (JobId) with (data_compression = page) on [JobData],
	foreign key(JobStatusTypeId) references job.JobStatusType,
	period for system_time ([_SystemRecordStartDateTime], [_SystemRecordEndDateTime])
)
with
(
	system_versioning = on ( HISTORY_TABLE = [History].[Job_JobStatus] )
);
go

create index [NCIX_Job_JobStatus] 
on Job.[JobStatus] (JobId, JobStatusTypeId, StatusTimeStamp, [Db_StatusTimeStamp]) 
with (data_compression = page) on [JobIndex];
go
