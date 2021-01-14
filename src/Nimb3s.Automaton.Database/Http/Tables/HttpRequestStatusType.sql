CREATE TABLE [Http].[HttpRequestStatusType]
(
	Id smallint NOT NULL,
	Enumeration varchar(30) not null,
    [InsertTimeStamp] DATETIMEOFFSET NOT NULL default (SYSUTCDATETIME()),

	PRIMARY KEY CLUSTERED(Id) WITH (DATA_COMPRESSION = PAGE, FILLFACTOR = 100) ON [HttpData],
)

