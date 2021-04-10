CREATE TABLE Job.[Job]
(
	Id uniqueidentifier NOT NULL,
	JobName varchar(250),
    [InsertTimeStamp] DATETIMEOFFSET NOT NULL default (SYSUTCDATETIME()),
	[Db_InsertTimeStamp] DATETIMEOFFSET NOT NULL default (SYSUTCDATETIME()),
	
	PRIMARY KEY CLUSTERED (id) WITH (DATA_COMPRESSION = PAGE) ON [JobData]
)
