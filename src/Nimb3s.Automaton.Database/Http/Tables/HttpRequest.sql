CREATE TABLE [Http].[HttpRequest]
(
	[Id] uniqueidentifier NOT NULL,
	[WorkItemId] uniqueidentifier not null,
	[HttpRequestStatusId] smallint not null default(0),
	[Url] varchar(2000) not null,
	[ContentType] varchar(100) not null,
	[Method] varchar(10) not null,
	[Content] varchar(max) null,
	[RequestHeadersInJson] varchar(max) null,
	[ContentHeadersInJson] varchar(max) null,
	[AuthenticationConfigInJson] varchar(max) null
	
	primary key clustered(id, WorkItemId)
)
