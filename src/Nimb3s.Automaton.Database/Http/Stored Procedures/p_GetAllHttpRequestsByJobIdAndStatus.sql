CREATE PROCEDURE Http.p_GetAllHttpRequestsByJobIdAndStatus
 @jobId uniqueidentifier,
 @workItemStatusTypeId smallint
as
begin
	begin try
		set nocount on;

		select hr.Id,
			hr.WorkItemId,
			hr.Url,
			hr.ContentType,
			hr.Method,
			hr.Content,
			hr.RequestHeadersInJson,
			hr.ContentHeadersInJson,
			hr.AuthenticationConfigInJson,
			hr.InsertTimeStamp
		from job.WorkItemStatus wis
			join http.HttpRequest hr
				on wis.WorkItemId = hr.WorkItemId
				and wis.WorkItemStatusTypeId = @workItemStatusTypeId
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