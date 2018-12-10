IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ChkLeadAssignment]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ChkLeadAssignment]
GO

	-- =============================================
-- Author:		Yuga Hatolkar
-- Create date: 14th Dec, 2014
-- Description:	Check if lead assignment already done or not.
-- EXEC TC_ChkLeadAssignment 14407
-- =============================================
CREATE PROCEDURE [dbo].[TC_ChkLeadAssignment]
	-- Add the parameters for the stored procedure here

	@LeadId				VARCHAR(MAX) = NULL,
	@Status				BIT = 0 OUTPUT,
	@AssignedTo			VARCHAR(200) = NULL OUTPUT
AS
BEGIN
			
	SET @Status = 0	

	IF EXISTS( SELECT TOP 1 TC_LeadId FROM TC_InquiriesLead WITH(NOLOCK)
	WHERE (TC_LeadStageId = 1 OR TC_LeadStageId IS NULL) AND TC_UserId IS NULL AND TC_LeadId = @LeadId)	
	BEGIN
		SET @Status = 1		
	END
	ELSE
	BEGIN
		SET @Status = 0	

		SELECT @AssignedTo = UserName FROM TC_InquiriesLead TIL WITH(NOLOCK)
		INNER JOIN TC_Users TU WITH(NOLOCK) ON TU.Id = TIL.TC_UserId
		WHERE TIL.TC_LeadId = @LeadId
	END		
END




--------------------------------------------------------------------------------------------------------------------------------------




