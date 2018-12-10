IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMSendforApproval]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMSendforApproval]
GO

	-- =============================================
-- Author	    :	Vinayak Patil
-- Create date	:	25-11-2013
-- Description	:	To send updated target data by Area Manager to Regional Manager For Approval
-- Modified By  :	Vinayak Patil on 11-12-13 Added parameter @AMComments
-- =============================================
 CREATE PROCEDURE [dbo].[TC_TMSendforApproval]
 @TC_AMId INT,
 @Year SMALLINT = NULL,
 @AMComments VARCHAR(1000) = NULL


 AS
   BEGIN
   IF @Year IS NULL
   BEGIN
		SET @Year = YEAR(GETDATE())
   END

		DECLARE @ApprovalMonth SMALLINT  ------------To select the month of which data is to be send for approval
				                         		

		------------------- Inerting entry into [TC_TMAMTargetChangeMaster] table to -----------------------------
		------------------- to indicate that data is sent for approval--------------------------------------------
		INSERT INTO [TC_TMAMTargetChangeMaster]
												(TC_AMId,
												 Year,
												 SentForApprovalDate,
												 IsActive,
												 IsAprrovedByRM,
												 IsAprrovedByNSC,
												 RMActionDate,
												 NSCActionDate,
												 RMId,
												 NSCId,
												 AMComments)
									VALUES      (@TC_AMId,
												 @Year,
												 GETDATE(),
												 1,
												 NULL,
												 NULL,
												 NULL,
												 NULL,
												 NULL,
												 NULL,
												 @AMComments)  


		-----------------To Delete data from [TC_TMAMTargetChangeApprovalReq] for Area Manager-----------
		----------------- for all previous months--------------------------------------------------------


		IF @Year = YEAR(GETDATE())
		BEGIN
			SET @ApprovalMonth = MONTH(GETDATE())

			DELETE FROM [TC_TMAMTargetChangeApprovalReq]
					WHERE  TC_AMId = @TC_AMId
						AND  [Month] < @ApprovalMonth
						   AND [Year] = @Year
		END

					
						  
END
