CREATE TABLE Job.[WorkItem]
(
	Id uniqueidentifier NOT NULL,
	JobId uniqueidentifier not null,
	WorkItemStatusId smallint not null

	PRIMARY KEY CLUSTERED(id, JobId)
)
