CREATE PROCEDURE [Http].[p_UpsertHttpRequest]
 @Id uniqueidentifier,
 @WorkItemId uniqueidentifier,
 @Url varchar(2000),
 @ContentType varchar(100),
 @Method varchar(10),
 @Content varchar(max),
 @UserAgent varchar(500),
 @RequestHeadersInJson varchar(max),
 @ContentHeadersInJson varchar(max),
 @AuthenticationConfigInJson varchar(max),
 @InsertTimeStamp datetimeoffset
as
begin
	begin try
		set nocount on;

		if not exists(select top 1 id from http.HttpRequest where Id = @Id)
		begin
			insert into [Http].HttpRequest (Id, WorkItemId, Url, ContentType, Method, Content, UserAgent, RequestHeadersInJson, ContentHeadersInJson, AuthenticationConfigInJson, InsertTimeStamp)
			values(@Id, @WorkItemId, @Url, @ContentType, @Method, @Content, @UserAgent, @RequestHeadersInJson, @ContentHeadersInJson, @AuthenticationConfigInJson, @InsertTimeStamp)
		end
		else
		begin
			update [Http].HttpRequest 
			set 
				Url = @Url,
				ContentType = @ContentType,
				Method = @Method,
				Content = @Content,
				UserAgent = @UserAgent,
				RequestHeadersInJson = @RequestHeadersInJson,
				ContentHeadersInJson = @ContentHeadersInJson,
				AuthenticationConfigInJson = @AuthenticationConfigInJson
			where Id = @Id
		end	

		if @@rowcount = 1
			return 0;
	end try
	begin catch
		DECLARE @ErrorMessageFormat VARCHAR(100), 
			@ErrorSeverity INT, 
			@ErrorState INT,
			@ErrorProcedure NVARCHAR(128),
			@ErrorNumber INT,
			@ErrorMessage VARCHAR(4000),
			@ErrorLine INT;

		SELECT @ErrorMessageFormat = 'Procedure : [Http].%s failed with message: (%i) %s at line %i.',
			@ErrorSeverity = ERROR_SEVERITY(), 
			@ErrorState = ERROR_STATE(),
			@ErrorProcedure = ERROR_PROCEDURE(),
			@ErrorNumber = ERROR_NUMBER(),
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorLine = ERROR_LINE();
        
		IF XACT_STATE() <>  0
            ROLLBACK TRAN;

        RAISERROR(@ErrorMessageFormat, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorNumber, @ErrorMessage, @ErrorLine);
	end catch
end