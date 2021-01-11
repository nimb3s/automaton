CREATE TABLE [Http].[HttpResponse]
(
	[Id] uniqueidentifier NOT NULL,
	[HttpRequestId] uniqueidentifier not null,
	[StatusCode] int not null,
	[Body] varchar(max) null,
    [InsertTimeStamp] DATETIMEOFFSET NOT NULL default (SYSUTCDATETIME()),
	[Db_InsertTimeStamp] DATETIMEOFFSET NOT NULL default (SYSUTCDATETIME()),
	
	PRIMARY KEY CLUSTERED([Id]) WITH (DATA_COMPRESSION = PAGE) ON [HttpData],
) TEXTIMAGE_ON [HttpLargeBinaryObjects];
GO


CREATE INDEX [NCIX_HttpResponse_Id] 
ON [Http].[HttpResponse] (Id, HttpRequestId, StatusCode, InsertTimeStamp) 
WITH (DATA_COMPRESSION = PAGE) ON HttpIndex;
GO