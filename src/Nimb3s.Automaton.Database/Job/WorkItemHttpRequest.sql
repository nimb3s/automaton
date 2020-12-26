CREATE TABLE Job.[WorkItemHttpRequest]
(
	Id uniqueidentifier NOT NULL PRIMARY KEY,
	JobWorkItemId uniqueidentifier not null,
	WorkItemHttpRequestStatusId smallint not null
)
