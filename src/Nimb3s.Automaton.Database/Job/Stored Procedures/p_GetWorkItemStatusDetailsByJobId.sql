CREATE PROCEDURE Job.p_GetWorkItemStatusDetailsByJobId
 @JobId uniqueidentifier
AS
begin
	begin try
		set nocount on;

		SELECT w.Id, w.JobId, ws.WorkItemStatusTypeId
		FROM Job.WorkItem w
		INNER JOIN Job.WorkItemStatus ws
		ON w.Id = ws.WorkItemId
		WHERE w.JobId = @JobId;
	end try
	begin catch
			DECLARE @ErrorMessageFormat VARCHAR(100), 
				@ErrorSeverity INT, 
				@ErrorState INT,
				@ErrorProcedure NVARCHAR(128),
				@ErrorNumber INT,
				@ErrorMessage VARCHAR(4000),
				@ErrorLine INT;

			SELECT @ErrorMessageFormat = 'Procedure : [Job].%s failed with message: (%i) %s at line %i.',
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