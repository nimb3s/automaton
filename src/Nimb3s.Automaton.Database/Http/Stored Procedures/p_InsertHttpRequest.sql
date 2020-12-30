CREATE procedure [Http].[p_InsertHttpRequest]
 @Id uniqueidentifier,
 @WorkItemId uniqueidentifier,
 @HttpRequestStatusId smallint,
 @Url varchar(2000),
 @ContentType varchar(100),
 @Method varchar(10),
 @Content varchar(max),
 @RequestHeadersInJson varchar(max),
 @ContentHeadersInJson varchar(max),
 @AuthenticationConfigInJson varchar(max)
as
begin
set nocount off
	insert into [Http].HttpRequest values(@Id, @WorkItemId, @HttpRequestStatusId, @Url, @ContentType, @Method, @Content, @RequestHeadersInJson, @ContentHeadersInJson, @AuthenticationConfigInJson)
end