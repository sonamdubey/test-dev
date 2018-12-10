IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_SaveSummerCampaign]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_SaveSummerCampaign]
GO

	
-- =============================================
-- Author:		Vaibhav Kale
-- Create date: 05-Apr-2013
-- Description:	saves record for SKODA Summer campaign 2013
-- =============================================
CREATE PROCEDURE [dbo].[OLM_SaveSummerCampaign]
	-- Add the parameters for the stored procedure here
	@Name			VARCHAR(50),
	@Mobile			VARCHAR(15),
	@Email			VARCHAR(100),
	@PreferredDate	DATE,
	@Id				NUMERIC(18,0) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    
    SET @Id = -1
    
	INSERT INTO OLM_SummerCampaign (Name, Mobile, Email ,PreferredDate)
	VALUES (@Name, @Mobile, @Email, @PreferredDate)
	
	SET @Id = SCOPE_IDENTITY()
END

