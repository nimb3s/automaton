CREATE TABLE Job.[WorkItem]
(
	Id uniqueidentifier NOT NULL PRIMARY KEY,
	JobId uniqueidentifier not null,
	WorkItemStatusId smallint not null
)
