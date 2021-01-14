CREATE TABLE History.[Job_WorkItemStatus]
(
	Id bigint not null,
	WorkItemId uniqueidentifier not null,
	WorkItemStatusTypeId smallint not null,
    [StatusTimeStamp] DATETIMEOFFSET NOT NULL,
	[Db_StatusTimeStamp] DATETIMEOFFSET NOT NULL, 
	[_SystemRecordStartDateTime] DATETIME2(7) not null,
	[_SystemRecordEndDateTime] DATETIME2(7) not null,
);
GO

CREATE INDEX [NCIX_Job_WorkItemStatus] 
ON History.[Job_WorkItemStatus] ([_SystemRecordStartDateTime], [_SystemRecordEndDateTime], Id, WorkItemId, WorkItemStatusTypeId, [StatusTimeStamp], [Db_StatusTimeStamp]) 
WITH (DATA_COMPRESSION = PAGE) ON [JobIndex];
GO