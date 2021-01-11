CREATE TABLE [Http].[HttpRequestStatus]
(
	HttpRequestId uniqueidentifier not null,
	HttpRequestStatusTypeId smallint not null,
    [StatusTimeStamp] DATETIMEOFFSET NOT NULL default (SYSUTCDATETIME()),
	[DB_StatusTimeStamp] DATETIMEOFFSET NOT NULL default (SYSUTCDATETIME()),

    [_SystemRecordStartDateTime] DATETIME2(7) GENERATED ALWAYS AS ROW START NOT NULL CONSTRAINT DF_HttpRequestStatus__SystemRecordStartDateTime DEFAULT(SYSUTCDATETIME()),
	[_SystemRecordEndDateTime] DATETIME2(7) GENERATED ALWAYS AS ROW END NOT NULL CONSTRAINT DF_HttpRequestStatus__SystemRecordEndDateTime DEFAULT('9999-12-31 23:59:59.9999999'),
	PRIMARY KEY CLUSTERED ([HttpRequestId]) WITH (DATA_COMPRESSION = PAGE) ON [HttpData],
	PERIOD FOR SYSTEM_TIME ([_SystemRecordStartDateTime], [_SystemRecordEndDateTime])
)
WITH
(
	SYSTEM_VERSIONING = ON ( HISTORY_TABLE = [History].[Http_HttpRequestStatus] )
);
GO

CREATE INDEX [NCIX_HttpRequestStatus_HttpRequestId] 
ON [Http].[HttpRequestStatus] (HttpRequestId, HttpRequestStatusTypeId, StatusTimeStamp, DB_StatusTimeStamp) 
WITH (DATA_COMPRESSION = PAGE) ON HttpIndex;
GO