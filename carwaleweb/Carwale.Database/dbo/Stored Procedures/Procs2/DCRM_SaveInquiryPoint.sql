IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_SaveInquiryPoint]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_SaveInquiryPoint]
GO

	

-- =============================================
-- Author      : Vinay Kumar Prajapati
-- Create date : 22 APRIL 2014
-- Description : Save Inquiry point details
-- =============================================

CREATE PROCEDURE [dbo].[DCRM_SaveInquiryPoint]
    (
	@DealerId		INT,
	@Point          INT,
	@Amount         INT,
	@UpdatedBy		NUMERIC = NULL,
	@UpdatedOn		DATETIME,
	@Status			BIT OUTPUT
	)
 AS
	DECLARE @CurrentPoint INT

    BEGIN
        SELECT  @CurrentPoint= DP.CurrentPoint FROM TC_MMDealersPoint AS DP WITH(NOLOCK) WHERE DP.DealerId=@DealerId 
        IF @@ROWCOUNT <> 0
			BEGIN 
				--updating CurrentPoint colomn by adding previous value
				UPDATE TC_MMDealersPoint SET  LastUpdatedOn=@UpdatedOn, CurrentPoint= @CurrentPoint+@Point WHERE DealerId=@DealerId
				INSERT INTO  TC_MMDealersPointLogs(DealerId,Amount,Points,CreatedOn,CreatedBy) VALUES(@DealerId,@Amount,@Point,@UpdatedOn,@UpdatedBy)
			    SET  @Status = 0
			END
		ELSE
			BEGIN 
				INSERT INTO  TC_MMDealersPoint(DealerId,CurrentPoint,CreatedOn) VALUES(@DealerId,@Point,@UpdatedOn)
			    INSERT INTO  TC_MMDealersPointLogs(DealerId,Amount,Points,CreatedOn,CreatedBy) VALUES(@DealerId,@Amount,@Point,@UpdatedOn,@UpdatedBy)			
			    SET  @Status = 1           
			END
      
    END


