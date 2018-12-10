IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_VisitAppointmentSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_VisitAppointmentSave]
GO

	
-- =============================================
-- Author:		Nilesh Utture
-- Create date: 30 Jan 2013 
-- Description:	Schedule dealer visit appointment
-- Modified By: Nilesh Utture on 24th April, 2013 Added parameter @LeadOwnerId
-- =============================================
CREATE PROCEDURE [dbo].[TC_VisitAppointmentSave]
	-- Add the parameters for the stored procedure here
	@UserId BIGINT,
	@BranchId BIGINT,
	@LeadId BIGINT,
	@VisitDate DATETIME,
	@VisitTime VARCHAR(20),
	@Purpose VARCHAR(500),
	@Iscancelled INT = 0,
	@LeadOwnerId INT = NULL,
	@RetVal TINYINT OUTPUT
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	DECLARE @TC_UserId INT
	SET NOCOUNT ON;
	SET @RetVal=0
	BEGIN TRY
	IF @Iscancelled<>1
		BEGIN
			IF EXISTS(SELECT TOP 1 TC_AppointmentsId FROM TC_Appointments WITH(NOLOCK) WHERE TC_LeadId = @LeadId )
			BEGIN
				UPDATE TC_Appointments SET VisitDate=@VisitDate,VisitTime=@VisitTime,Purpose=@Purpose,
				TC_UsersId=@LeadOwnerId,LastUpdatedDate=GETDATE(), TC_LeadDispositionId=36
				WHERE TC_LeadId=@LeadId	
				
				EXEC TC_DispositionLogInsert @UserId,36,@LeadId,1,@LeadId	
				SET @RetVal=1
			
			END	
			ELSE
			BEGIN
				INSERT INTO TC_Appointments (VisitDate,VisitTime,Purpose,TC_LeadId,EntryDate,LastUpdatedDate,TC_UsersId, TC_LeadDispositionId) 
				VALUES (@VisitDate,@VisitTime,@Purpose,@LeadId ,GETDATE(),GETDATE(),@LeadOwnerId, 37)
				
				EXEC TC_DispositionLogInsert @UserId,37,@LeadId,1,@LeadId
				SET @RetVal=1
			END
		END
	ELSE
		BEGIN
			UPDATE TC_Appointments SET TC_LeadDispositionId=38
			WHERE TC_LeadId=@LeadId	
			EXEC TC_DispositionLogInsert @UserId,38,@LeadId,1,@LeadId
			SET @RetVal=1
		END
	
	
	--COMMIT TRANSACTION ProcessInquiry
	END TRY
	
	BEGIN CATCH
		--ROLLBACK TRAN ProcessInquiry
		INSERT INTO TC_Exceptions(Programme_Name,TC_Exception,TC_Exception_Date)
     VALUES('TC_ScheduleVisitAppointments',ERROR_MESSAGE(),GETDATE())
	END CATCH;

    -- Insert statements for procedure here
END


