IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveAutoAssignLead]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveAutoAssignLead]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 26 Sept 2014
-- Description:	Log auto assigned lead / car details into a table CRM_AutoAssignedLeads
-- =============================================
CREATE PROCEDURE [dbo].[CRM_SaveAutoAssignLead]
	-- Add the parameters for the stored procedure here
	@CRMLeadId			NUMERIC(18,0),
	@CRMCustId			NUMERIC(18,0),
	@CWCustId			NUMERIC(18,0),
	@CBDId				NUMERIC(18,0),
	@CityId				INT,
	@DealerId			INT,
	@CustFirstName		VARCHAR(250),
	@CustMobile			VARCHAR(50),
	@CustEmail			VARCHAR(250),
	@CarMakeId			INT,
	@CarModelId			INT,
	@CarName			VARCHAR(250),
	@LeadSourceId		INT,
	@IsTdRequest		BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	UPDATE CRM_Leads SET Owner = 13 WHERE ID = @CRMLeadId
	
	INSERT INTO CRM_AutoAssignedLeads
		(
			CRMLeadId, CRMCustId, CWCustId, CBDId, CityId, DealerId, CustFirstName, CustMobile, CustEmail, CarMakeId, CarModelId, CarName, LeadSourceId, IsTdRequest
		)
	VALUES
		(
			@CRMLeadId, @CRMCustId, @CWCustId, @CBDId, @CityId, @DealerId, @CustFirstName, @CustMobile, @CustEmail, @CarMakeId, @CarModelId, @CarName, @LeadSourceId, @IsTdRequest
		)
		
	
END
