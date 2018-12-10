IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AudiBE_GetCustomersDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AudiBE_GetCustomersDetails]
GO

	
-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 25/7/2013
-- Description:	Proc to get the customer details against the given customer id.
-- =============================================
CREATE PROCEDURE [dbo].[OLM_AudiBE_GetCustomersDetails]
	@CustomerId BIGINT,
	@Name VARCHAR(100) OUTPUT,
	@Mobile VARCHAR(50) OUTPUT,
	@StateId INT OUTPUT,
	@StateName	VARCHAR(50) OUTPUT,
	@CityId INT OUTPUT,
	@CityName	VARCHAR(50) OUTPUT,
	@Email VARCHAR(50) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 	
		@Name = C.Name,
		@Mobile = C.Mobile,
		@StateId = C.StateId,
		@StateName = ST.Name,
		@CityId = C.CityId,
		@CityName = ABC.Name,
		@Email = C.Email		
	FROM OLM_AudiBE_Customers C
	JOIN OLM_AudiBE_Cities ABC ON C.CityId = ABC.Id AND ABC.IsActive = 1
	JOIN States ST ON C.StateId = ST.ID
	WHERE C.Id = @customerId
END
