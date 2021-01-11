CREATE PROCEDURE Http.p_InsertHttpResponse
 @Id uniqueidentifier,
 @HttpRequestId uniqueidentifier,
 @StatusCode smallint,
 @Body varchar(max)
as
begin
set nocount off
	insert into http.HttpResponse values(@Id, @HttpRequestId, @StatusCode, @Body)
end