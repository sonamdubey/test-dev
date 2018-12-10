IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_IB_SaveInboundCallData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_IB_SaveInboundCallData]
GO

	CREATE PROCEDURE [dbo].[CRM_IB_SaveInboundCallData]
@mobileNumber		VARCHAR(20),
@InboundCallDataId	NUMERIC OUTPUT
AS 
BEGIN
	INSERT INTO CRM_InboundCallData(MobileNumber) VALUES(@mobileNumber)
	SET @InboundCallDataId=SCOPE_IDENTITY()	
	PRINT '@InboundCallDataId' + Convert(VarChar,@InboundCallDataId) 
END
