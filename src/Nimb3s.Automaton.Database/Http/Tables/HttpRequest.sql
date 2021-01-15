CREATE TABLE [Http].[HttpRequest]
(
	[Id] uniqueidentifier NOT NULL,
	[WorkItemId] uniqueidentifier not null,
	[Url] varchar(2000) not null,
	[ContentType] varchar(100) not null,
	[UserAgent] varchar(500),
	[Method] varchar(10) not null,
	[Content] varchar(max) null,
	[RequestHeadersInJson] varchar(max) null,
	[ContentHeadersInJson] varchar(max) null,
	[AuthenticationConfigInJson] varchar(max) null,
    [InsertTimeStamp] DATETIMEOFFSET NOT NULL default (SYSUTCDATETIME()),
	[Db_InsertTimeStamp] DATETIMEOFFSET NOT NULL default (SYSUTCDATETIME()),
	
	PRIMARY KEY CLUSTERED(Id, WorkItemId) WITH (DATA_COMPRESSION = PAGE) ON [HttpData],
) TEXTIMAGE_ON [HttpLargeBinaryObjects]
GO
