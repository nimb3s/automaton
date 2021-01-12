CREATE TABLE [History].[Http_HttpRequestStatus]
(
	Id bigint not null,
	HttpRequestId uniqueidentifier not null,
	HttpRequestStatusTypeId smallint not null,
    [StatusTimeStamp] DATETIMEOFFSET NOT NULL,
	[Db_StatusTimeStamp] DATETIMEOFFSET NOT NULL default(SYSUTCDATETIME()), 
	[_SystemRecordStartDateTime] DATETIME2(7) NOT NULL,
	[_SystemRecordEndDateTime] DATETIME2(7) NOT NULL,
);
GO

CREATE INDEX [NCIX_HttpRequestStatus_HttpRequestId] 
ON [History].[Http_HttpRequestStatus] (
[_SystemRecordStartDateTime],
[_SystemRecordEndDateTime],
Id,
HttpRequestId,
HttpRequestStatusTypeId,
StatusTimeStamp,
[Db_StatusTimeStamp]
) 
WITH (DATA_COMPRESSION = PAGE) ON HttpIndex;
GO