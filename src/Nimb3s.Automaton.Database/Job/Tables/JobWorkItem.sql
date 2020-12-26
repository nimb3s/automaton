CREATE TABLE Job.[WorkItem]
(
	Id uniqueidentifier NOT NULL PRIMARY KEY,
	JobId int not null,
	WorkItemStatusId smallint not null
)
