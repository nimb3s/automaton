CREATE TABLE Job.[WorkItemStatusType]
(
	Id smallint NOT NULL,
	Enumeration varchar(30) not null,
	[InsertTimeStamp] DATETIMEOFFSET NOT NULL default (SYSUTCDATETIME()),

	CONSTRAINT [CPK_WorkItemStatusType_Id] PRIMARY KEY CLUSTERED (Id ASC) WITH (DATA_COMPRESSION = PAGE, FILLFACTOR = 100) ON [JobData]
)
