CREATE TABLE Job.[WorkItemStatus]
(
	Id bigint not null identity(1,1),
	WorkItemId uniqueidentifier not null,
	WorkItemStatusTypeId smallint not null,
    [StatusTimeStamp] DATETIMEOFFSET NOT NULL default(SYSUTCDATETIME()), 
	[Db_StatusTimeStamp] DATETIMEOFFSET NOT NULL default(SYSUTCDATETIME()), 
	[_SystemRecordStartDateTime] DATETIME2(7) GENERATED ALWAYS AS ROW START NOT NULL CONSTRAINT DF_WorkItemStatus__SystemRecordStartDateTime DEFAULT(SYSUTCDATETIME()),
	[_SystemRecordEndDateTime] DATETIME2(7) GENERATED ALWAYS AS ROW END NOT NULL CONSTRAINT DF_WorkItemStatus__SystemRecordEndDateTime DEFAULT('9999-12-31 23:59:59.9999999'),
	PERIOD FOR SYSTEM_TIME ([_SystemRecordStartDateTime], [_SystemRecordEndDateTime]),

    CONSTRAINT [CPK_WorkItemStatus_WorkItemId] PRIMARY KEY CLUSTERED ([WorkItemId] ASC) WITH (DATA_COMPRESSION = PAGE) ON [JobData],
	foreign key(WorkItemStatusTypeId) references job.WorkItemStatusType
	
)
WITH
(
	SYSTEM_VERSIONING = ON ( HISTORY_TABLE = History.Job_WorkItemStatus )
);
GO

CREATE INDEX [NCIX_Job_WorkItemStatus] 
ON Job.[WorkItemStatus] (Id, WorkItemId, WorkItemStatusTypeId, [StatusTimeStamp], Db_StatusTimeStamp) 
WITH (DATA_COMPRESSION = PAGE) ON [JobIndex];
GO