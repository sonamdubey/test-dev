IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CheckTestDriveAvailability]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CheckTestDriveAvailability]
GO

	
-- =============================================
-- Author:			Vikas
-- Create date:	24/08/2012
-- Description:		To Check if Test drive facility can be offered for a particular model in a city
-- =============================================
CREATE PROCEDURE [dbo].[CheckTestDriveAvailability] 
	-- Add the parameters for the stored procedure here
	@MakeId Int = 0, 
	@ModelId Int = 0,
	@CityId Int = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TOP 1 Id FROM CRM_ADM_Queues WHERE AcceptNewLead = 1 And IsActive = 1 AND ID IN 
    (Select QueueId FROM CRM_ADM_QueueRuleParams WHERE 
    (MakeId = @MakeId AND ModelId = @ModelId AND CityId = @CityId) OR 
    (MakeId = @MakeId AND ModelId = @ModelId AND CityId = -1) OR 
    (MakeId = @MakeId AND ModelId = -1 AND CityId = @CityId) OR 
    (MakeId = @MakeId AND ModelId = -1 AND CityId = -1) OR 
    (MakeId = -1 AND ModelId = @ModelId AND CityId = @CityId) OR 
    (MakeId = -1 AND ModelId = @ModelId AND CityId = -1) OR 
    (MakeId = -1 AND ModelId = -1 AND CityId = @CityId) OR 
    (MakeId = -1 AND ModelId = -1 AND CityId = -1) 
    ) Order By Rank DESC
END

