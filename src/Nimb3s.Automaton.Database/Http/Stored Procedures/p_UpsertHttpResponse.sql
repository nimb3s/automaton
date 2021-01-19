CREATE PROCEDURE Http.p_UpsertHttpResponse
 @Id uniqueidentifier,
 @HttpRequestId uniqueidentifier,
 @StatusCode smallint,
 @Body varchar(max),
 @InsertTimeStamp datetimeoffset
as
begin
	begin try
		set nocount on;

		if not exists(select top 1 id from http.HttpResponse where Id = @Id)
		begin
			insert into http.HttpResponse(Id, HttpRequestId, StatusCode, Body, InsertTimeStamp) values(@Id, @HttpRequestId, @StatusCode, @Body, @InsertTimeStamp)
		end
		else
		begin
			update [Http].HttpResponse 
			set 
				StatusCode = @StatusCode,
				Body = @Body
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