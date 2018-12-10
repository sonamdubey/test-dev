IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_IsFeedbackGiven]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_IsFeedbackGiven]
GO

	

-- =============================================
-- Author:		Vinay Kumar
-- Create date: 29th july 2015
-- Description:	 Feedback of surveyor is given by dealer 
-- =============================================
CREATE PROCEDURE [dbo].[Absure_IsFeedbackGiven]
	@AbSureCarId		NUMERIC(18, 0),
	@Status				BIT = 0 OUTPUT
AS
BEGIN
		
	SET @Status = 0	
	SELECT AIF.Absure_InspectionFeedbackId FROM Absure_InspectionFeedback AS AIF WITH(NOLOCK)  WHERE AIF.AbSure_CarDetailsId=@AbSureCarId
	IF @@ROWCOUNT<>0
		BEGIN		
			SET @Status = 1   -- Feedback is already given ...
		END
	ELSE
	    SET @Status = 0      --- No feedback
END


	  
