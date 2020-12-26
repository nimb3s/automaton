CREATE TABLE Job.[Job]
(
	Id uniqueidentifier NOT NULL PRIMARY KEY,
	JobName varchar(250) not null,
	JobStatusId smallint not null default(0),
)
