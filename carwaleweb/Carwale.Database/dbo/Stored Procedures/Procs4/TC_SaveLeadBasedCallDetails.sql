IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SaveLeadBasedCallDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SaveLeadBasedCallDetails]
GO

	
-- ===============================================================================================
-- Author		: Yuga Hatolkar
-- Create date	: 23rd Nov, 2015
-- Description	: Save lead based Call Details.
-- ==================================================================================================
CREATE PROCEDURE [dbo].[TC_SaveLeadBasedCallDetails] 
@LeadId BIGINT = NULL,
@BranchId INT,
@UpdatedBy INT,
@CallType INT,
@CallTime DATETIME,
@CallDuration VARCHAR(1000),
@PhoneNumber VARCHAR(50),
@LeadName VARCHAR(50)

AS
BEGIN		

	SELECT ID FROM TC_LeadBasedCallDetails WITH(NOLOCK)
	WHERE LeadId = @LeadId

	IF @@ROWCOUNT > 0
		UPDATE TC_LeadBasedCallDetails SET CallType = @CallType, CallTime = @CallTime, Duration = @CallDuration, BranchId = @BranchId, UpdatedBy = @UpdatedBy,
										   UpdatedOn = GETDATE(), LeadName = @LeadName, PhoneNumber = @PhoneNumber
		WHERE LeadId = @LeadId

	ELSE
		INSERT INTO TC_LeadBasedCallDetails (LeadId, CallType, CallTime, Duration, UpdatedOn, UpdatedBy, BranchId, LeadName, PhoneNumber)
		VALUES (@LeadId, @CallType, @CallTime, @CallDuration, GETDATE(), @UpdatedBy, @BranchId, @LeadName, @PhoneNumber)
		
END





--------------------------------------------------------------------------------------------------------------------------------------------------




