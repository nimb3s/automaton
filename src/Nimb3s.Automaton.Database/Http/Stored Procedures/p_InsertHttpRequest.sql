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
	if not exists(select top 1 id from http.HttpRequest where Id = @Id)
	begin
		insert into [Http].HttpRequest values(@Id, @WorkItemId, @HttpRequestStatusId, @Url, @ContentType, @Method, @Content, @RequestHeadersInJson, @ContentHeadersInJson, @AuthenticationConfigInJson)
	end
	else
	begin
		update [Http].HttpRequest 
		set HttpRequestStatusId = @HttpRequestStatusId,
			Url = @Url,
			ContentType = @ContentType,
			Method = @Method,
			Content = @Content,
			RequestHeadersInJson = @RequestHeadersInJson,
			ContentHeadersInJson = @ContentHeadersInJson,
			AuthenticationConfigInJson = @AuthenticationConfigInJson
		where Id = @Id
	end	
end