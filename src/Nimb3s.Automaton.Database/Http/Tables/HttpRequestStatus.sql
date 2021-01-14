create table [Http].[HttpRequestStatus]
(
	Id bigint not null identity(1,1),
	HttpRequestId uniqueidentifier not null,
	HttpRequestStatusTypeId smallint not null,
    [StatusTimeStamp] datetimeoffset NOT NULL default (sysutcdatetime()),
	[DB_StatusTimeStamp] datetimeoffset NOT NULL default (sysutcdatetime()),

    [_SystemRecordStartDateTime] DATETIME2(7) generated always as row start not null constraint DF_HttpRequestStatus__SystemRecordStartDateTime default(sysutcdatetime()),
	[_SystemRecordEndDateTime] DATETIME2(7) generated always as row end not null constraint DF_HttpRequestStatus__SystemRecordEndDateTime default('9999-12-31 23:59:59.9999999'),
	primary key clustered ([HttpRequestId]) with (data_compression = page) on [HttpData],
	foreign key(HttpRequestStatusTypeId) references http.HttpRequestStatusType,
	period for system_time ([_SystemRecordStartDateTime], [_SystemRecordEndDateTime])
)
WITH
(
	system_versioning = on ( HISTORY_TABLE = [History].[Http_HttpRequestStatus] )
);
GO

create index [NCIX_HttpRequestStatus_HttpRequestId] 
on [Http].[HttpRequestStatus] (Id, HttpRequestId, HttpRequestStatusTypeId, StatusTimeStamp, DB_StatusTimeStamp) 
with (data_compression = page) on HttpIndex;
go