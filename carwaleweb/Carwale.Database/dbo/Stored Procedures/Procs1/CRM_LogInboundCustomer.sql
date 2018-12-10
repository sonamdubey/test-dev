IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_LogInboundCustomer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_LogInboundCustomer]
GO

	-- =============================================
-- Author      : Chetan Navin	
-- Create date : 3rd Oct 2014
-- Description : To Log inbound customer data
-- =============================================
CREATE PROCEDURE [dbo].[CRM_LogInboundCustomer] 
	-- Add the parameters for the stored procedure here
	@CusName VARCHAR(50),
	@CusMobile VARCHAR(10),
	@CusEmail VARCHAR(50),
	@VersionId INT,
	@CityId INT,
	@PurposeId SMALLINT,
	@LeadId BIGINT,
	@CreatedBy INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO CRM_InboundCallLog (CusName,CusMobile,CusEmail,VersionId,CityId,PurposeId,LeadId,CreatedBy) 
	VALUES (@CusName,@CusMobile,@CusEmail,@VersionId,@CityId,@PurposeId,@LeadId,@CreatedBy)

END
