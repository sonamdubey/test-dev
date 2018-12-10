IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_SaveAppointmentDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_SaveAppointmentDetails]
GO

	
-- =============================================
-- Author: Vinay Kumar
-- Create date: 18th March 2015
-- Description:  Save inspection scheduled time 
-- =============================================         
CREATE PROCEDURE [dbo].[AbSure_SaveAppointmentDetails]
    -- Add the parameters for the stored procedure here
    @CarId                INT			= NULL,-- carDetailsId
	@UserId               INT           = NULL,
    @EntryDate            DATETIME		= NULL,-- no requirment
    @ScheduledDate        DATE		    = NULL,
    @ScheduledTime        VARCHAR(100)	= NULL,
    @Reason               VARCHAR(500)  = NULL,
	@AppointmentId        INT         OUTPUT,
    @StatusId             TINYINT     OUTPUT
AS
DECLARE @TotalAppointments TINYINT 
BEGIN

 IF @CarId IS NOT NULL AND  EXISTS(SELECT  ACD.Id FROM AbSure_CarDetails AS ACD WITH(NOLOCK) WHERE ACD.Id=@CarId) 
     BEGIN

	       SELECT @TotalAppointments =COUNT(AP.Id) FROM Absure_Appointments AS AP WITH(NOLOCK) WHERE  AP.AbsureCarId =@CarId
		   IF @TotalAppointments < 3
			   BEGIN
					
					UPDATE AbSure_CarDetails SET IsInspectionRescheduled = 1 WHERE Id = @CarId
					
					INSERT INTO Absure_Appointments(AbsureCarId,EntryDate,UserId,ScheduledDate,ScheduledTime,Reason) 
										VALUES(@carId,GETDATE(),@UserId,@ScheduledDate,@ScheduledTime,@Reason)

                     SET @AppointmentId = Scope_Identity()

					--Update Absure car Details
					IF @TotalAppointments < 2
						UPDATE AbSure_CarDetails 
						SET  AppointmentDate=ISNULL(@ScheduledDate ,ISNULL(AppointmentDate,EntryDate)),
							 AppointmentTime=ISNULL(@ScheduledTime ,AppointmentTime)
							WHERE Id=@CarId
			  	
					SET @StatusId = 1
			   END 
		   ELSE
			   BEGIN
					SET @StatusId = 0
					SET @AppointmentId=0
			   END
            -- Autometic cancel Warrenty request when it reshedule more than three times 
			IF	@TotalAppointments >= 2
				BEGIN

					EXEC	Absure_CancelWarranty
							@AbsureCarDetailsId	= @CarId, 
							@StatusId			= 3,
							@Reason				= 'Automatically cancelled as resheduled more than two times',
							@CancelledBy        = @UserId

			        -- Rejection Of car Appointment			  	
					   SET @StatusId = 2
				END
	  END
     ELSE
		 BEGIN
		     SET @StatusId = 0
			 SET @AppointmentId=0
		 END
 END
