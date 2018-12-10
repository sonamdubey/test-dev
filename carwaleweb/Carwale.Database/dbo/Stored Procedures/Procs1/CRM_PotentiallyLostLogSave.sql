IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_PotentiallyLostLogSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_PotentiallyLostLogSave]
GO

	
-- =============================================
-- Author:		Amit Kumar
-- Create date: 5th Nov 2012
-- Description:	This function will save the log if the lead is marked as potentially Lost in cardetails page.This procedure is used in RMPanle followupdetails and OPR cardetails
-- =============================================
CREATE PROCEDURE [dbo].[CRM_PotentiallyLostLogSave] 
@CbdId			NUMERIC(18,0),
@Comments		VARCHAR(1500),
@TaggedOn		DATETIME,
@TaggedBy		NUMERIC(18,0),
@UpdatedOn		DATETIME,
@UpdatedBy		NUMERIC(18,0),
@DealerId		NUMERIC(18,0)	
AS
BEGIN
	UPDATE CRM_PotentiallyLostCase SET Comment=@Comments
		WHERE CBDId = @CbdId
	IF @@ROWCOUNT=0
		BEGIN
			INSERT INTO CRM_PotentiallyLostCase(CBDId,Comment,TaggedBy,TaggedOn,UpdatedBy,UpdatedOn,DealerId) VALUES (@CbdId,@Comments,@TaggedBy,@TaggedOn,@UpdatedBy,@UpdatedOn,@DealerId)
		END
END

