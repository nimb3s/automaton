CREATE TABLE [Http].[HttpResponse]
(
	[Id] uniqueidentifier NOT NULL,
	[HttpRequestId] uniqueidentifier not null,
	[StatusCode] int not null,
	[Body] varchar(max) null,
	
	primary key clustered(Id, HttpRequestId)
)
