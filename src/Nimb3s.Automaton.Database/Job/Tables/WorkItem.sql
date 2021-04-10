CREATE TABLE Job.[WorkItem]
(
	Id uniqueidentifier NOT NULL,
	JobId uniqueidentifier not null,
	[InsertTimeStamp] DATETIMEOFFSET NOT NULL default (SYSUTCDATETIME()),
	[Db_InsertTimeStamp] DATETIMEOFFSET NOT NULL default (SYSUTCDATETIME()),

	PRIMARY KEY CLUSTERED (Id, JobId) WITH (DATA_COMPRESSION = PAGE) ON [JobData]
)
