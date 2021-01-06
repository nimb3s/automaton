CREATE TABLE Job.[Job]
(
	Id uniqueidentifier NOT NULL,
	JobName varchar(250),
	JobStatusId smallint not null default(0),

	PRIMARY KEY CLUSTERED(id)
)
