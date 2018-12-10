IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_MOBSaveLead]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_MOBSaveLead]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 06-July-2012
-- Description:	Insert record for new lead in table CRM_MobDealerLeads
-- =============================================
CREATE PROCEDURE [dbo].[CRM_MOBSaveLead]
	-- Add the parameters for the stored procedure here
	@Name			VARCHAR(100),
	@Mobile			VARCHAR(15),
	@Email			VARCHAR(100),
	@Car			NUMERIC,
	@City			NUMERIC,
	@DealerId		NUMERIC,
	@Source			INT,
	@UserId			INT
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO CRM_MobDealerLeads (Name,Mobile,Email,versionid,CityId,Source,DealerId,UserId)
	VALUES (@Name,@Mobile,@Email,@Car,@City,@Source,@DealerId, @UserId)
END
