IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_ResetLeadsForMFCCities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_ResetLeadsForMFCCities]
GO

	-- =============================================
-- Author	:	Sachin Bharti(4th Aug 2014)
-- Description	:	Reset send leads for the mapped mahindra cities
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_ResetLeadsForMFCCities]
	@Cities		VARCHAR(1000),
	@UpdatedBy	INT , 
	@Result		SMALLINT OUTPUT
AS
BEGIN
	
	SET NOCOUNT ON;
	SET @Result = 0;
	UPDATE DCRM_MFCMappedCities SET LeadsSent = 0 , 
									UpdatedBy = @UpdatedBy,
									UpdatedOn = GETDATE()
	WHERE Id IN (SELECT *FROM SplitText(@Cities,','))

	IF @@ROWCOUNT <> 0 
		SET @Result = 1
END
